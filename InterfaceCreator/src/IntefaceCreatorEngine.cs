﻿namespace OOP_Interfaces_Creator
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using OOP_Interfaces_Creator.Contracts;

    public class InterfacesCreatorEngine : IInterfacesCreatorEngine
    {
        private const string InputFileName = "input.txt";

        private static IInterfacesCreatorEngine instance;

        private readonly string[] separators;
        private readonly StreamReader reader;
        private readonly IList<string> lines;
        private readonly IDictionary<string, IClassNode> nodes;
        private readonly IDictionary<string, string> childs;

        private string namespaceName;
        private string currentNodeName;
        private ClassNode currentNode;

        private InterfacesCreatorEngine()
        {
            this.separators = new string[] { " ", "(", ")", ",", "get", "set", "{", "}", ";", ":", "\t" };
            this.reader = new StreamReader(InputFileName);
            this.lines = new List<string>();
            this.nodes = new Dictionary<string, IClassNode>();
            this.childs = new Dictionary<string, string>();
        }

        public static IInterfacesCreatorEngine Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new InterfacesCreatorEngine();
                }

                return instance;
            }
        }

        public string NamespaceName
        {
            get
            {
                return this.namespaceName;
            }

            private set
            {
                if (string.IsNullOrEmpty(value))
                {
                    throw new ArgumentException("The namespace cannot be null or empty");
                }

                this.namespaceName = value;
            }
        }

        public void Run()
        {
            this.InputNamespaceName();
            this.ReadLines();
            this.ProcessLines();
            this.WriteLines();
        }

        private void InputNamespaceName()
        {
            Console.WriteLine("Please enter the namespace in which you want to create the classes:");
            this.NamespaceName = Console.ReadLine();
        }

        private void ReadLines()
        {
            using (this.reader)
            {
                string line = this.reader.ReadLine();

                while (line != null)
                {
                    line = line.Trim();
                    this.lines.Add(line);
                    line = this.reader.ReadLine();
                }
            }
        }

        private void ProcessLines()
        {
            foreach (var inputLine in this.lines)
            {
                string line = inputLine;
                if (line.IndexOf("//") >= 0)
                {
                    line = line.Substring(0, line.IndexOf("//"));
                }

                bool isPublicSetter = line.Contains("set");
                var inputElements = line.Split(this.separators, StringSplitOptions.RemoveEmptyEntries);

                if (line.Contains("interface"))
                {
                    this.ProcessInterfaceCommand(inputElements);
                }
                else if (line.Contains("class"))
                {
                    this.ProcessClassCommand(inputElements);
                }
                else if (line.Contains("{") && line.Contains("}"))
                {
                    this.ProcessPropertyCommand(inputElements, isPublicSetter);
                }
                else if (line.Contains("(") && line.Contains(")"))
                {
                    this.ProcessMethodCommand(inputElements);
                }
            }
        }

        private void WriteLines()
        {
            foreach (var item in this.nodes)
            {
                var writer = new StreamWriter("Models/" + item.Key.Substring(1) + ".cs");
                using (writer)
                {
                    writer.Write(item.Value);
                }
            }
        }

        private void ProcessInterfaceCommand(string[] inputElements)
        {
            this.currentNodeName = inputElements[2];
            this.currentNode = new ClassNode(this.currentNodeName, this.NamespaceName);
            this.nodes[this.currentNodeName] = this.currentNode;

            if (inputElements.Length > 3)
            {
                var parentName = inputElements[3];
                this.nodes[parentName].AddChild(this.currentNode);
            }
        }

        private void ProcessClassCommand(string[] inputElements)
        {
            this.currentNodeName = "I" + inputElements[2];
            this.currentNode = new ClassNode(this.currentNodeName, this.NamespaceName);
            this.nodes[this.currentNodeName] = this.currentNode;

            if (inputElements.Length > 3)
            {
                var parentName = inputElements[3];
                this.nodes[parentName].AddChild(this.currentNode);
            }
        }

        private void ProcessPropertyCommand(string[] inputElements, bool isPublicSetter)
        {
            var propertyType = inputElements[0];
            var propertyName = inputElements[1];
            var property = new Property(propertyType, propertyName);
            if (isPublicSetter)
            {
                property.IsPublicSetter = true;
            }

            this.currentNode.AddProperty(property);
        }

        private void ProcessMethodCommand(string[] inputElements)
        {
            var methodType = inputElements[0];
            var methodName = inputElements[1];
            var method = new Method(methodType, methodName);
            if (inputElements.Length > 2)
            {
                for (int i = 2; i < inputElements.Length; i += 2)
                {
                    var parameter = inputElements[i] + " " + inputElements[i + 1];
                    method.AddParameter(parameter);
                }
            }

            this.currentNode.AddMethod(method);
        }
    }
}
