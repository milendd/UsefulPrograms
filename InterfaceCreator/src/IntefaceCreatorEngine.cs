namespace OOP_Interfaces_Creator
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.IO;
    using OOP_Interfaces_Creator.Contracts;

    public class InterfacesCreatorEngine : IInterfacesCreatorEngine
    {
        public const string InputFileName = "input.txt";
        public const string OutputDirectoryName = "Models";

        private static IInterfacesCreatorEngine instance;

        private readonly string[] separators;
        private readonly StreamReader reader;
        private readonly IDictionary<string, IClassNode> nodes;
        private IList<string> lines;

        private string namespaceName;
        private string currentNodeName;
        private ClassNode currentNode;

        private InterfacesCreatorEngine()
        {
            this.separators = new string[] { " ", "(", ")", "get;", "set;", "{", "}", ";", ":", "\t" };
            this.reader = new StreamReader(InputFileName);
            this.lines = new List<string>();
            this.nodes = new Dictionary<string, IClassNode>();
            this.Stopwatch = new Stopwatch();
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

        public Stopwatch Stopwatch { get; private set; }

        public void Run(ReadMethod readMethod)
        {
            this.InputNamespaceName();
            this.Stopwatch.Start();
            switch (readMethod)
            {
                case ReadMethod.TextFile:
                    this.ReadLinesFromTextFile();
                    break;
                case ReadMethod.Directory:
                    this.ReadLinesFromDirectory();
                    break;
                default:
                    throw new ArgumentException("The read method should be file or directory");
            }

            this.ProcessLines();
            this.WriteLines();
            this.Stopwatch.Stop();
        }

        public TimeSpan ElapsedTime()
        {
            return this.Stopwatch.Elapsed;
        }
        
        private void InputNamespaceName()
        {
            Console.WriteLine("Please enter the namespace in which you want to create the classes:");
            this.NamespaceName = Console.ReadLine();
        }

        private void ReadLinesFromTextFile()
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

        private void ReadLinesFromDirectory()
        {
            // TODO:
        }

        private void ProcessLines()
        {
            var reorder = new LinesReorder(this.lines);
            this.lines = reorder.Execute();

            foreach (var inputLine in this.lines)
            {
                string line = inputLine;
                if (line.IndexOf("//") >= 0)
                {
                    line = line.Substring(0, line.IndexOf("//"));
                }

                bool isPublicSetter = line.Contains("set;");
                var inputElements = line.Split(this.separators, StringSplitOptions.RemoveEmptyEntries);

                if (line.Contains("interface"))
                {
                    this.ProcessInterfaceCommand(inputElements);
                }
                else if (line.Contains("class"))
                {
                    inputElements[2] = "I" + inputElements[2];
                    this.ProcessInterfaceCommand(inputElements);
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
            foreach (var node in this.nodes)
            {
                var outputFileName = OutputDirectoryName + "/" + node.Key.Substring(1) + ".cs";
                var writer = new StreamWriter(outputFileName);
                using (writer)
                {
                    writer.Write(node.Value);
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

        private void ProcessPropertyCommand(string[] inputElements, bool isPublicSetter)
        {
            var propertyType = inputElements[0];
            var propertyName = inputElements[1];

            // Hack for dictionary
            if (inputElements.Length == 3)
            {
                propertyType += " " + propertyName;
                propertyName = inputElements[2];
            }

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
