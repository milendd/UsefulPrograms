namespace OOP_Interfaces_Creator
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    using OOP_Interfaces_Creator.Contracts;

    public class Method : Component, IMethod
    {
        private const string MethodFormat = "\tpublic {0} {1}({2})";

        private IList<string> parameters;

        public Method(string type, string name)
            : base(type, name)
        {
            this.parameters = new List<string>();
        }

        public void AddParameter(string parameter)
        {
            if (string.IsNullOrEmpty(parameter))
            {
                throw new ArgumentException("The parameter cannot be null or empty");
            }

            this.parameters.Add(parameter);
        }

        public override string ToString()
        {
            var result = new StringBuilder();
            string joinedParameters = string.Join(", ", this.parameters);

            result.AppendLine();
            result.AppendLine(string.Format(MethodFormat, this.Type, this.Name, joinedParameters));
            result.AppendLine("\t{");
            result.AppendLine("\t\t// TODO: implement this method");
            result.AppendLine("\t}");

            return result.ToString().TrimEnd();
        }
    }
}
