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

            // Ensure the PST file exists; create a minimal one if missing
            if (!File.Exists(pstPath))
            {
                try
                {
                    // Create a new Unicode PST file
                    PersonalStorage.Create(pstPath, FileFormatVersion.Unicode);
                    Console.WriteLine($"Created new PST file at '{pstPath}'.");
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to create PST file: {ex.Message}");
                    return;
                }
            }

            // Open the PST file with write access
            try
            {
                using (PersonalStorage pst = PersonalStorage.FromFile(pstPath, true))
                {
                    // Apply password protection if not already protected
                    if (!pst.Store.IsPasswordProtected)
                    {
                        pst.Store.ChangePassword("StrongPassword123!");
                        Console.WriteLine("Password protection applied to PST file.");
                    }
                    else
                    {
                        Console.WriteLine("PST file is already password protected.");
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
