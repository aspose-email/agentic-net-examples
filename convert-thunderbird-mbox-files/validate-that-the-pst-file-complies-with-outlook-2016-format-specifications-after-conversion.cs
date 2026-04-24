using System;
using System.IO;
using Aspose.Email;
using Aspose.Email.Storage.Pst;

class Program
{
    static void Main()
    {
        try
        {
            string pstPath = "sample.pst";

            // Ensure the directory for the PST file exists
            try
            {
                string pstDirectory = Path.GetDirectoryName(pstPath);
                if (!string.IsNullOrEmpty(pstDirectory) && !Directory.Exists(pstDirectory))
                {
                    Directory.CreateDirectory(pstDirectory);
                }
            }
            catch (Exception dirEx)
            {
                Console.Error.WriteLine($"Directory preparation failed: {dirEx.Message}");
                return;
            }

            // Guard file existence; create a minimal placeholder PST if missing
            if (!File.Exists(pstPath))
            {
                try
                {
                    // Create an empty Unicode PST file
                    PersonalStorage.Create(pstPath, FileFormatVersion.Unicode).Dispose();
                    Console.WriteLine($"Placeholder PST created at '{pstPath}'.");
                }
                catch (Exception createEx)
                {
                    Console.Error.WriteLine($"Failed to create placeholder PST: {createEx.Message}");
                    return;
                }
            }

            // Load the PST and perform basic validation
            try
            {
                using (PersonalStorage pst = PersonalStorage.FromFile(pstPath))
                {
                    // Validate format (Outlook 2016 uses Unicode PST)
                    FileFormat pstFormat = pst.Format;
                    Console.WriteLine($"PST format: {pstFormat}");

                    // Simple structural checks
                    if (pst.RootFolder != null)
                    {
                        Console.WriteLine("Root folder is present.");
                        Console.WriteLine($"Root folder display name: {pst.RootFolder.DisplayName}");
                    }
                    else
                    {
                        Console.Error.WriteLine("Root folder is missing.");
                    }

                    // Verify total items count (should be non‑negative)
                    int totalItems = pst.Store.GetTotalItemsCount();
                    Console.WriteLine($"Total items in PST: {totalItems}");
                }
            }
            catch (Exception loadEx)
            {
                Console.Error.WriteLine($"Failed to load or validate PST: {loadEx.Message}");
                return;
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
