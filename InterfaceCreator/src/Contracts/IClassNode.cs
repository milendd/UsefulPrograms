namespace OOP_Interfaces_Creator.Contracts
{
    using System.Collections.Generic;

    public interface IClassNode
    {
        string IterfaceName { get; }

        string Name { get; }

        bool HasParent { get; set; }

        IClassNode Parent { get; set; }

        IList<IProperty> Properties { get; }

        IList<IProperty> CurrentProperties { get; }

        IList<IProperty> BaseProperties { get; }

        void AddChild(IClassNode child);

        void AddProperty(IProperty property);

        void AddMethod(IMethod method);

        void ClearProperties();
    }
}
