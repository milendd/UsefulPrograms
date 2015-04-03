namespace OOP_Interfaces_Creator
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Threading;

    public class Program
    {
        public static void Main()
        {
            try
            {
                InterfacesCreatorEngine.Instance.Run();
                Console.WriteLine(new string('#', 40));
                Console.WriteLine("The classes were created successfully.");
                Console.WriteLine(new string('#', 40));

                // 0. Bug: Fix new line when no properties
                // 0. Bug: Consider for ISet and ICollection!!!
                // 0. Consider bool for automatic property.
                // 0. Don't know if it is bug, but think for abstract on node with 1 child(shouldn't have abstract)
                // 1. Fix the order in the input if possible (no order matters). Very difficult!!!
                // 2. Add namespace name if possible.
                // 3. Run StyleCop. Refactor everything for high quality code. Add public documentation to interface.
                // Double, long and int -> array. Add unit tests. Remove all string concatination.Use append instead.
                // Think for removing the second constructor from the class node(it can stay this way)
                // 4. Try with the full interface, including the namespace. 
                // 5. If bool, consider to Toggle, Convert, and so on...
                // 6. Think for multiple interfaces: Human, ISmt, IOther, ...
                // 7. Think for using(should create them if not exists, or taken from the input).
                // 8. It is possible for virtual components(don't know if is possible).
                // 9. Implement ToString() + base calling and const string format.
                // 10. Add stopwatch for the user.
                // 11. Add instructions in the github page.
                // 12. Rename project, folders, etc... to Class creator, not interface creator
                // 13. Remove repeating code in Process class and interface.
                // 14. Think for repeating elements(for example two classes Person)
            }
            catch (FileNotFoundException)
            {
                Console.WriteLine("Could not find file \"input.txt\".");
            }
            catch (DirectoryNotFoundException)
            {
                Console.WriteLine("Could not find folder Models.");
            }
            catch (KeyNotFoundException)
            {
                Console.WriteLine("Please reorder the interfaces from the base class to the childs.");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                Thread.Sleep(4000);
            }
        }
    }
}
