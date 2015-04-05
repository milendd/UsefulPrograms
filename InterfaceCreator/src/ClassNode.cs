namespace OOP_Interfaces_Creator
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using OOP_Interfaces_Creator.Contracts;

    public class ClassNode : IClassNode
    {
        private string interfaceName;
        private string namespaceName;
        private string name;
        private IList<IProperty> properties;
        private IList<IProperty> currentProperties;
        private IList<IProperty> baseProperties;
        private IList<IMethod> methods;
        private IList<IClassNode> children;

        public ClassNode(string interfaceName)
        {
            this.IterfaceName = interfaceName;
            this.properties = new List<IProperty>();
            this.methods = new List<IMethod>();
            this.children = new List<IClassNode>();
        }

        public ClassNode(string interfaceName, string namespaceName)
            : this(interfaceName)
        {
            this.NamespaceName = namespaceName;
        }

        public string IterfaceName
        {
            get
            {
                return this.interfaceName;
            }

            private set
            {
                if (string.IsNullOrEmpty(value))
                {
                    throw new ArgumentException("The name cannot be null or empty");
                }

                this.interfaceName = value;
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

        public string Name
        {
            get
            {
                if (this.name == null)
                {
                    this.name = this.interfaceName.Substring(1);
                }

                return this.name;
            }
        }

        public bool HasParent { get; set; }

        public IClassNode Parent { get; set; }

        public IList<IProperty> Properties
        {
            get
            {
                return new List<IProperty>(this.properties);
            }
        }

        public virtual IList<IProperty> CurrentProperties
        {
            get
            {
                if (this.currentProperties == null)
                {
                    this.currentProperties = new List<IProperty>(this.properties);

                    if (this.HasParent)
                    {
                        this.currentProperties = this.currentProperties.Where(x => !this.Parent.Properties.Contains(x)).ToList();
                    }
                }

                return new List<IProperty>(this.currentProperties);
            }
        }

        public virtual IList<IProperty> BaseProperties
        {
            get
            {
                if (this.baseProperties == null)
                {
                    if (this.HasParent)
                    {
                        this.baseProperties = this.Parent.Properties;
                    }
                    else
                    {
                        return new List<IProperty>();
                    }
                }

                return new List<IProperty>(this.baseProperties);
            }
        }

        public void AddChild(IClassNode child)
        {
            if (child == null)
            {
                throw new ArgumentException("The child cannot be null");
            }

            this.children.Add(child);
            child.HasParent = true;
            child.Parent = this;

            foreach (var property in this.properties)
            {
                child.AddProperty(property);
            }
        }

        public void AddProperty(IProperty property)
        {
            if (property == null)
            {
                throw new ArgumentException("The property cannot be null");
            }

            this.properties.Add(property);
        }

        public void AddMethod(IMethod method)
        {
            if (method == null)
            {
                throw new ArgumentException("The method cannot be null");
            }

            if (method.Name != "ToString")
            {
                this.methods.Add(method);
            }
        }

        public void ClearProperties()
        {
            this.properties.Clear();
        }

        public override string ToString()
        {
            var result = new StringBuilder();
            result.AppendLine(this.CreateBegining());
            result.AppendLine("\t{");
            if (this.CurrentProperties.Count > 0)
            {
                result.AppendLine(this.CreateStringFormat());
                result.AppendLine();
                foreach (var property in this.CurrentProperties)
                {
                    result.AppendLine("\t\t" + property.CreateField());
                }

                result.AppendLine();
            }

            if (this.Properties.Count > 0)
            {
                result.AppendLine(this.CreateConstructor());
            }

            foreach (var property in this.CurrentProperties)
            {
                result.AppendLine("\t\t" + property.CreateProperty());
            }

            foreach (var method in this.methods)
            {
                result.AppendLine(method.ToString());
            }

            if (this.CurrentProperties.Count > 0)
            {
                result.AppendLine();
                result.AppendLine(this.CreateToString());
            }

            result.AppendLine("\t}");
            result.AppendLine("}");

            return result.ToString().TrimEnd();
        }

        private string CreateStringFormat()
        {
            var result = new StringBuilder();

            result.AppendFormat("\t\tprivate const string {0}Format = \"", this.Name);
            if (this.BaseProperties.Count > 0)
            {
                result.Append("{0}");
            }

            for (int i = 0; i < this.CurrentProperties.Count; i++)
            {
                var currentProperty = this.CurrentProperties[i];
                var index = i;
                if (this.BaseProperties.Count > 0)
                {
                    index = i + 1;
                }

                result.Append(currentProperty.Name + ":{" + index + "};");
            }

            result.Append("\";");
            return result.ToString().TrimEnd();
        }

        private string CreateBegining()
        {
            var result = new StringBuilder();
            result.Append("namespace ").AppendLine(this.NamespaceName);
            result.AppendLine("{");
            result.Append("\tpublic ");
            if (this.children.Count > 1)
            {
                result.Append("abstract ");
            }

            result.AppendFormat("class {0} : ", this.Name);
            if (this.HasParent)
            {
                result.AppendFormat("{0}, ", this.Parent.Name);
            }

            result.AppendLine(this.IterfaceName);

            return result.ToString().TrimEnd();
        }

        private string CreateConstructor()
        {
            var result = new StringBuilder();
            var allProperties = this.properties.Where(x => !x.IsGeneric);
            result.AppendFormat("\t\tpublic {0}({1})", this.Name, string.Join(", ", allProperties)).AppendLine();

            if (this.HasParent && this.BaseProperties.Count != 0)
            {
                var names = this.BaseProperties.Where(x => !x.IsGeneric).Select(x => x.FieldName);
                result.AppendFormat("\t\t\t: base({0})", string.Join(", ", names)).AppendLine();
            }

            result.AppendLine("\t\t{");
            foreach (var property in this.CurrentProperties)
            {
                if (property.IsGeneric)
                {
                    result.AppendFormat("\t\t\tthis.{0} = new {1}();", property.Name, property.StringTypeWhenGeneric()).AppendLine();
                }
                else
                {
                    result.AppendFormat("\t\t\tthis.{0} = {1};", property.Name, property.FieldName).AppendLine();
                }
            }

            result.AppendLine("\t\t}");
            return result.ToString().TrimEnd();
        }

        private string CreateToString()
        {
            var result = new StringBuilder();

            result.AppendLine("\t\tpublic override string ToString()");
            result.AppendLine("\t\t{");
            result.AppendFormat("\t\t\treturn string.Format({0}Format, ", this.Name);
            if (this.HasParent)
            {
                result.Append("base.ToString(), ");
            }

            var properties = this.CurrentProperties.Select(x => "this." + x.Name);
            result.Append(string.Join(", ", properties));
            result.AppendLine(");");
            result.AppendLine("\t\t}");

            return result.ToString().TrimEnd();
        }
    }
}