namespace OOP_Interfaces_Creator
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

        public void Run()
        {
            this.ReadLines();
            this.ProcessLines();
            this.WriteLines();
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
            string currentName = null;
            ClassNode currentNode = null;

            foreach (var inputLine in this.lines)
            {
                string line = inputLine;
                if (line.IndexOf("//") >= 0)
                {
                    line = line.Substring(0, line.IndexOf("//"));
                }

                bool isPublicSetter = line.Contains("set");
                var elements = line.Split(this.separators, StringSplitOptions.RemoveEmptyEntries);
                var result = string.Join(" ", elements);

                if (line.Contains("interface"))
                {
                    currentName = elements[2];
                    currentNode = new ClassNode(currentName);
                    this.nodes[currentName] = currentNode;

                    if (elements.Length > 3)
                    {
                        var parentName = elements[3];
                        this.nodes[parentName].AddChild(currentNode);
                    }
                }
                else if (line.Contains("{") && line.Contains("}"))
                {
                    var propertyType = elements[0];
                    var propertyName = elements[1];
                    var property = new Property(propertyType, propertyName);
                    if (isPublicSetter)
                    {
                        property.IsPublicSetter = true;
                    }

                    currentNode.AddProperty(property);
                }
                else if (line.Contains("(") && line.Contains(")"))
                {
                    var methodType = elements[0];
                    var methodName = elements[1];
                    var method = new Method(methodType, methodName);
                    if (elements.Length > 2)
                    {
                        for (int i = 2; i < elements.Length; i += 2)
                        {
                            var parameter = elements[i] + " " + elements[i + 1];
                            method.AddParameter(parameter);
                        }
                    }

                    currentNode.AddMethod(method);
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
    }
}
