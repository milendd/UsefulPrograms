namespace OOP_Interfaces_Creator.Contracts
{
    public interface IProperty : IComponent
    {
        string FieldName { get; }

        bool IsGeneric { get; }

        bool IsPublicSetter { get; set; }

        string CreateField();

        string CreateProperty();
    }
}
