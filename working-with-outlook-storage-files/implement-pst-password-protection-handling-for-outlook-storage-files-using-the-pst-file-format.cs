using Aspose.Email;
using System;
using System.IO;
using Aspose.Email.Storage.Pst;

class Program
{
    static void Main()
    {
        try
        {
            string pstPath = "sample.pst";

            // Ensure the directory for the PST file exists
            string pstDirectory = Path.GetDirectoryName(pstPath);
            if (!string.IsNullOrEmpty(pstDirectory) && !Directory.Exists(pstDirectory))
            {
                Directory.CreateDirectory(pstDirectory);
            }

            // Guard file existence; create a minimal PST if missing
            if (!File.Exists(pstPath))
            {
                try
                {
                    using (PersonalStorage createdPst = PersonalStorage.Create(pstPath, FileFormatVersion.Unicode))
                    {
                        // No additional setup required for an empty PST
                    }
                    Console.WriteLine($"Created placeholder PST at '{pstPath}'.");
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Error creating PST file: {ex.Message}");
                    return;
                }
            }

            // Open the PST file with write access
            try
            {
                using (PersonalStorage pst = PersonalStorage.FromFile(pstPath, true))
                {
                    bool isProtected = pst.Store.IsPasswordProtected;
                    Console.WriteLine($"PST password protected: {isProtected}");

                    string password = "Secret123";

                    if (!isProtected)
                    {
                        // Set a password
                        pst.Store.ChangePassword(password);
                        Console.WriteLine("Password has been set on the PST file.");
                    }
                    else
                    {
                        // Validate the existing password
                        bool isValid = pst.Store.IsPasswordValid(password);
                        Console.WriteLine($"Provided password is {(isValid ? "valid" : "invalid")}.");
                    }
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
