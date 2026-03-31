using System;
using System.IO;
using Aspose.Email;
using Aspose.Email.Storage.Pst;

class Program
{
    static void Main(string[] args)
    {
        try
        {
            // Path to the PST file to inspect
            string pstPath = "sample.pst";

            // Ensure the PST file exists; create a minimal placeholder if missing
            if (!File.Exists(pstPath))
            {
                // Create an empty Unicode PST file as a placeholder
                using (PersonalStorage placeholder = PersonalStorage.Create(pstPath, FileFormatVersion.Unicode))
                {
                    // No additional setup required for the placeholder
                }

                Console.WriteLine($"Created placeholder PST at \"{pstPath}\".");
            }

            // Open the PST file
            using (PersonalStorage pst = PersonalStorage.FromFile(pstPath))
            {
                // Access the message store metadata
                MessageStore store = pst.Store;

                // Determine whether the PST is password protected
                bool isPasswordProtected = store.IsPasswordProtected;

                Console.WriteLine($"PST password protected: {isPasswordProtected}");
            }
        }
        catch (Exception ex)
        {
            // Friendly error output
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
