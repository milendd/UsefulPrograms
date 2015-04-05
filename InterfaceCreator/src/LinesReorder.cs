namespace OOP_Interfaces_Creator
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    public class LinesReorder
    {
        private IList<string> inputLines;
        private IList<string> allNames;
        private IDictionary<string, IList<string>> nameToEntireNodeLines;
        private IList<string> nameToParentName;

        public LinesReorder(IList<string> inputLines)
        {
            this.inputLines = inputLines;
            this.allNames = new List<string>();
            this.nameToEntireNodeLines = new Dictionary<string, IList<string>>();
            this.nameToParentName = new List<string>();
        }

        public IList<string> Execute()
        {
            var outputLines = new List<string>();
            string currentName = null;
            var currentNodeList = new List<string>();

            foreach (var line in this.inputLines)
            {
                if (line.Contains("interface") || line.Contains("class"))
                {
                    var elements = line.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                    currentName = elements[2];
                    if (elements.Length > 4)
                    {
                        var parentName = elements[4];
                        this.nameToParentName.Add(currentName + "-->" + parentName);
                    }
                    else
                    {
                        this.nameToParentName.Add(currentName + "-->");
                    }

                    currentNodeList.Clear();
                }

                currentNodeList.Add(line);

                if (currentName != null)
                {
                    this.nameToEntireNodeLines[currentName] = currentNodeList.ToList();
                }
            }

            this.allNames = this.nameToParentName
                .Select(x => x.Split(new string[] { "-->" }, StringSplitOptions.RemoveEmptyEntries)[0])
                .ToList();

            this.Reorder();

            foreach (var item in this.allNames)
            {
                var currentList = this.nameToEntireNodeLines[item];
                foreach (var line in currentList)
                {
                    outputLines.Add(line);
                }
            }

            return outputLines;
        }

        private void Reorder()
        {
            for (int i = 0; i < this.allNames.Count; i++)
            {
                var item = this.nameToParentName[i];
                var elements = item.Split(new string[] { "-->" }, StringSplitOptions.RemoveEmptyEntries);
                var name = elements[0];
                if (elements.Length == 1)
                {
                    this.Swap(0, i);
                }
            }

            for (int i = 0; i < this.allNames.Count; i++)
            {
                var item = this.nameToParentName[i];
                var elements = item.Split(new string[] { "-->" }, StringSplitOptions.RemoveEmptyEntries);
                var name = elements[0];

                if (elements.Length > 1)
                {
                    var currentIndex = this.allNames.IndexOf(elements[0]);
                    var parentIndex = this.allNames.IndexOf(elements[1]);
                    if (currentIndex < parentIndex)
                    {
                        this.Swap(currentIndex, parentIndex);
                    }
                }
            }
        }

        private void Swap(int currentIndex, int parentIndex)
        {
            var parentName = this.allNames[parentIndex];
            for (int i = parentIndex; i > currentIndex; i--)
            {
                this.allNames[i] = this.allNames[i - 1];
            }

            this.allNames[currentIndex] = parentName;
        }
    }
}
