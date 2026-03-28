using System;
using System.IO;
using Aspose.Email;
using Aspose.Email.Storage.Pst;
using Aspose.Email.Mapi;

class Program
{
    static void Main()
    {
        try
        {
            string pstPath = "sample.pst";
            string password = "myPassword";

            // Verify that the PST file exists before attempting to open it
            if (!File.Exists(pstPath))
            {
                Console.Error.WriteLine($"Error: File not found – {pstPath}");
                return;
            }

            // Load the PST file and validate the password
            try
            {
                using (PersonalStorage pst = PersonalStorage.FromFile(pstPath))
                {
                    MessageStore store = pst.Store;

                    // Determine if the PST is password protected
                    if (store.IsPasswordProtected)
                    {
                        bool isValid = store.IsPasswordValid(password);
                        Console.WriteLine(isValid ? "Password is valid." : "Password is invalid.");
                    }
                    else
                    {
                        Console.WriteLine("PST file is not password protected.");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Error loading PST: {ex.Message}");
                return;
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
