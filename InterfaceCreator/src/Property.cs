﻿namespace OOP_Interfaces_Creator
{
    using System;
    using System.Text;
    using OOP_Interfaces_Creator.Contracts;

    public class Property : Component, IProperty
    {
        private const string ToStringFormat = "{0} {1}";
        private const string FieldFormat = "private {0} {1};";
        private const string PropertyFormat = "\tpublic {0} {1}";
        private const bool DefaultIsGenericType = false;
        private const bool DefaultIsPublicSetter = false;

        private bool isGeneric;

        public Property(string type, string name)
            : base(type, name)
        {
            this.IsGeneric = DefaultIsGenericType;
            this.IsPublicSetter = DefaultIsPublicSetter;
        }

        public string FieldName
        {
            get
            {
                return char.ToLower(this.Name[0]) + this.Name.Substring(1);
            }
        }

        public bool IsGeneric
        {
            get
            {
                return this.isGeneric;
            }

            private set
            {
                if (this.Type.Contains("<") && this.Type.Contains(">"))
                {
                    this.isGeneric = true;
                }
                else
                {
                    this.isGeneric = false;
                }
            }
        }

        public bool IsPublicSetter { get; set; }

        public string CreateField()
        {
            return string.Format(FieldFormat, this.Type, this.FieldName);
        }

        public string CreateProperty()
        {
            var result = new StringBuilder();
            result.AppendLine();
            result.AppendLine(string.Format(PropertyFormat, this.Type, this.Name));
            result.AppendLine("\t{");
            result.AppendLine(this.GenerateGetter());
            result.AppendLine();
            result.AppendLine(this.GenerateSetter());
            result.AppendLine("\t}");

            return result.ToString().TrimEnd();
        }

        public override string ToString()
        {
            return string.Format(ToStringFormat, this.Type, this.FieldName);
        }

        private string GenerateGetter()
        {
            var result = new StringBuilder();
            result.AppendLine("\t\tget");
            result.AppendLine("\t\t{");
            if (this.IsGeneric)
            {
                result.AppendLine(string.Format("\t\t\treturn new {0}(this.{1});", this.Type.Substring(1), this.FieldName));
            }
            else
            {
                result.AppendLine(string.Format("\t\t\treturn this.{0};", this.FieldName));
            }

            result.AppendLine("\t\t}");
            return result.ToString().TrimEnd();
        }

        private string GenerateSetter()
        {
            var result = new StringBuilder();
            if (this.IsPublicSetter)
            {
                result.AppendLine("\t\tset");
            }
            else
            {
                result.AppendLine("\t\tprivate set");
            }

            result.AppendLine("\t\t{");
            if (this.Type != "bool" && !this.IsGeneric)
            {
                result.AppendLine("\t" + this.GenerateIfStatement());
                result.AppendLine();
            }

            result.AppendLine(string.Format("\t\t\tthis.{0} = value;", this.FieldName));
            result.AppendLine("\t\t}");

            return result.ToString().TrimEnd();
        }

        private string GenerateIfStatement()
        {
            var result = new StringBuilder();

            string statement = "value == null";
            switch (this.Type)
            {
                case "int":
                case "double":
                case "decimal":
                case "long":
                    statement = "value < 0";
                    break;
                case "string":
                    statement = "string.IsNullOrEmpty(value)";
                    break;
            }

            string statementFormat = "\t\tif ({0})";
            result.AppendLine(string.Format(statementFormat, statement));
            result.AppendLine("\t\t\t{");

            string exceptionFormat = "\t\t\t\tthrow new ArgumentException(\"{0}\");";
            string exceptionValue = "The " + this.FieldName + " cannot be ";
            switch (this.Type)
            {
                case "int":
                case "double":
                case "decimal":
                case "long":
                    exceptionValue += "negative";
                    break;
                case "string":
                    exceptionValue += "null or empty";
                    break;
                default:
                    exceptionValue += "null";
                    break;
            }

            result.AppendLine(string.Format(exceptionFormat, exceptionValue));
            result.AppendLine("\t\t\t}");

            return result.ToString().TrimEnd();
        }
    }
}
