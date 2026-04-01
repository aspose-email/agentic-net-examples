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
            // Path to the PST file and the password to validate
            string pstPath = "sample.pst";
            string password = "password123";

            // Ensure the PST file exists; create a minimal placeholder if it does not
            if (!File.Exists(pstPath))
            {
                try
                {
                    // Create an empty Unicode PST file (no password)
                    PersonalStorage.Create(pstPath, FileFormatVersion.Unicode);
                    Console.WriteLine($"Placeholder PST created at '{pstPath}'.");
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Error creating placeholder PST: {ex.Message}");
                    return;
                }
            }

            // Open the PST file (read‑only)
            try
            {
                using (PersonalStorage pst = PersonalStorage.FromFile(pstPath, false))
                {
                    MessageStore store = pst.Store;

                    Console.WriteLine($"Is password protected: {store.IsPasswordProtected}");

                    // Validate the supplied password
                    bool isValid = store.IsPasswordValid(password);
                    Console.WriteLine($"Password valid: {isValid}");
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Error accessing PST file: {ex.Message}");
                return;
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
