using Aspose.Email;
using Aspose.Email.Storage.Pst;
using System;
using System.IO;
using System.Text.RegularExpressions;

class Program
{
    static void Main()
    {
        try
        {
            string pstPath = "sample.pst";

            // Ensure PST file exists; create a minimal placeholder if missing
            if (!File.Exists(pstPath))
            {
                // Create a new PST file with Unicode format
                PersonalStorage.Create(pstPath, FileFormatVersion.Unicode);
                Console.WriteLine($"Placeholder PST created at: {pstPath}");
            }

            // Open PST file
            using (PersonalStorage pst = PersonalStorage.FromFile(pstPath))
            {
                // Retrieve display name (read‑only)
                MessageStore store = pst.Store;
                string displayName = store.DisplayName ?? string.Empty;

                // Define expected naming convention (example: letters followed by underscore and 'Archive')
                string pattern = @"^[A-Za-z]+_Archive$";
                bool isValid = Regex.IsMatch(displayName, pattern, RegexOptions.IgnoreCase);

                if (isValid)
                {
                    Console.WriteLine($"Display name \"{displayName}\" matches the expected convention.");
                }
                else
                {
                    Console.WriteLine($"Display name \"{displayName}\" does NOT match the expected convention.");
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
