namespace OOP_Interfaces_Creator.Contracts
{
    using System;

    public interface IInterfacesCreatorEngine
    {
        void Run(ReadMethod readMethod);

        TimeSpan ElapsedTime();
    }
}
