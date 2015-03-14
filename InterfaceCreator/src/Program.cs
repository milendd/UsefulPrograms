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
                Console.WriteLine(new string('#', 70));
                Console.WriteLine("The classes were created successfully.");
                Console.WriteLine("If some property is enum, remove the null condition from it's setter.");
                Console.WriteLine(new string('#', 70));

                // 0. Bug: Fix new line when no properties
                // 0. Bug: Consider for ISet and ICollection!!!
                // 0. Consider bool for automatic property.
                // 1. Fix the order in the input if possible (no order matters). Very difficult!!!
                // 2. Add namespace name if possible.
                // 3. Extract to methods processLines.
                // 4. Run StyleCop. Refactor everything for high quality code. Add public documentation to interface.
                // Double, long and int -> array. Add unit tests. Remove all string concatination.Use append instead.
                // 5. Try with the full interface, including the namespace. 
                // 6. If bool, consider to Toggle, Convert, and so on...
                // 7. Think for multiple interfaces: Human, ISmt, IOther, ...
                // 8. Think for using(should create them if not exists, or taken from the input).
                // 9. It is possible for virtual components(don't know if is possible).
                // 10. Implement ToString() + base calling and const string format.
                // 11. Add stopwatch for the user.
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
                // Thread.Sleep(5000);
            }
        }
    }
}
