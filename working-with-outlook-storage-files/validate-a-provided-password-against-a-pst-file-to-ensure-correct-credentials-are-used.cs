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
            // Path to the PST file
            string pstPath = "sample.pst";
            // Password to validate
            string password = "yourPassword";

            // Ensure a PST file exists; create a minimal placeholder if missing
            if (!File.Exists(pstPath))
            {
                // Create an empty PST file (Unicode format) as a placeholder
                PersonalStorage.Create(pstPath, FileFormatVersion.Unicode);
                Console.WriteLine($"Placeholder PST file created at: {pstPath}");
            }

            // Open the PST file within a using block to ensure proper disposal
            using (PersonalStorage pst = PersonalStorage.FromFile(pstPath))
            {
                // Access the message store of the PST
                MessageStore store = pst.Store;

                // Validate the provided password
                bool isValid = store.IsPasswordValid(password);
                Console.WriteLine(isValid ? "Password is valid." : "Password is invalid.");
            }
        }
        catch (Exception ex)
        {
            // Log any unexpected errors without crashing the application
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
