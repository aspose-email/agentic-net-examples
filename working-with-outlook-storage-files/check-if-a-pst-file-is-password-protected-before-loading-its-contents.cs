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

            // Ensure the PST file exists; create a minimal placeholder if missing
            if (!File.Exists(pstPath))
            {
                try
                {
                    // Create an empty Unicode PST file
                    PersonalStorage.Create(pstPath, FileFormatVersion.Unicode);
                    Console.WriteLine("Placeholder PST file created at: " + pstPath);
                }
                catch (Exception createEx)
                {
                    Console.Error.WriteLine("Failed to create placeholder PST: " + createEx.Message);
                    return;
                }
            }

            // Open the PST file in read‑only mode
            using (PersonalStorage pst = PersonalStorage.FromFile(pstPath, false))
            {
                // Check if the PST storage is password protected
                bool isPasswordProtected = pst.Store.IsPasswordProtected;
                Console.WriteLine("Is PST password protected? " + isPasswordProtected);
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine("Error: " + ex.Message);
        }
    }
}
