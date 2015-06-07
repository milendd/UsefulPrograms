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
                InterfacesCreatorEngine.Instance.Run(ReadMethod.TextFile);
                var milliseconds = InterfacesCreatorEngine.Instance.ElapsedTime().Milliseconds;
                Console.Clear();
                Console.WriteLine(new string('#', 50));
                Console.WriteLine("The classes were created for {0} milliseconds.", milliseconds);
                Console.WriteLine(new string('#', 50));

                // 0. Bug: fix new line when only methods
                // 0. Bug: sb = new StringBuilder();
                // 0. Bug: Fix new line when no properties
                // 0. Consider bool for automatic property.
                // 0. Don't know if it is bug, but think for abstract on node with 1 child(shouldn't have abstract)
                // 3. Run StyleCop. Refactor everything for high quality code. Add public documentation to interface.
                // Double, long and int -> array. Add unit tests. Remove all string concatination.Use append instead.
                // Think for removing the second constructor from the class node(it can stay this way)
                // 5. If bool, consider to Toggle, Convert, and so on...
                // 8. It is possible for virtual components(don't know if is possible).
                // 11. Add instructions in the github page.
                // 12. Rename project, folders, etc... to Class creator, not interface creator
                // 14. Think for repeating elements(for example two classes Person)
                // 15. Var everything
                // 16. LinesReorder -> should be high quality!!!! + interfaces for all
                // 17. Remove hack for dictionary line 186 in Engine. Use while and string.Join!
                // 18. Usings should be HQC! That means with arrays with the data.
                // 19. TODO: implement directory files
                // 20. Create file with settings (constants) Important!
                // 21. Create directory Models with the result classes
                // 22. Remove repeating code in read from directory and read from text file -> extract reading as method or class
                // 23. Abstract class Engine with virtual(abstract) methods and different implementation for C#, Java, JavaScript, PHP...
                // 24. throw new IndexOutOfRangeException("The overtime rate must be non-negative."); Implement a new class with the exceptions
                // See the exam!
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
                Console.WriteLine("You could have classes which does not appear in the input!");
                Console.WriteLine("Example: ITestClass: INotFound");
                Thread.Sleep(2000);
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
