namespace OOP_Interfaces_Creator
{
    using System;
    using OOP_Interfaces_Creator.Contracts;

    public abstract class Component : IComponent
    {
        private string type;
        private string name;

        public Component(string type, string name)
        {
            this.Type = type;
            this.Name = name;
        }

        public string Type
        {
            get
            {
                return this.type;
            }

            private set
            {
                if (string.IsNullOrEmpty(value))
                {
                    throw new ArgumentException("The type cannot be null or empty");
                }

                this.type = value;
            }
        }

        public string Name
        {
            get
            {
                return this.name;
            }

            private set
            {
                if (string.IsNullOrEmpty(value))
                {
                    throw new ArgumentException("The name cannot be null or empty");
                }

                this.name = value;
            }
        }
    }
}
