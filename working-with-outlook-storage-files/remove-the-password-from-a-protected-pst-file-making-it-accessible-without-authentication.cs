using Aspose.Email;
using System;
using System.IO;
using Aspose.Email.Storage.Pst;

class Program
{
    static void Main(string[] args)
    {
        try
        {
            string pstPath = "protected.pst";
            string password = "secret";

            // Ensure the PST file exists; create a minimal placeholder if it does not.
            try
            {
                if (!File.Exists(pstPath))
                {
                    PersonalStorage.Create(pstPath, FileFormatVersion.Unicode);
                    Console.WriteLine("Placeholder PST file created.");
                }
            }
            catch (Exception ioEx)
            {
                Console.Error.WriteLine($"File I/O error: {ioEx.Message}");
                return;
            }

            // Open the PST file with write access and remove its password if set.
            try
            {
                using (PersonalStorage pst = PersonalStorage.FromFile(pstPath, true))
                {
                    MessageStore store = pst.Store;
                    if (store.IsPasswordProtected)
                    {
                        if (!store.IsPasswordValid(password))
                        {
                            Console.Error.WriteLine("Invalid password provided.");
                            return;
                        }
                        store.ChangePassword(string.Empty);
                        Console.WriteLine("Password removed successfully.");
                    }
                    else
                    {
                        Console.WriteLine("PST is not password protected.");
                    }
                }
            }
            catch (Exception pstEx)
            {
                Console.Error.WriteLine($"PST processing error: {pstEx.Message}");
                return;
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
