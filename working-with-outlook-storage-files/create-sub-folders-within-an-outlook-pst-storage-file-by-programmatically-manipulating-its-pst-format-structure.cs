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

            // Ensure the PST file exists; create a new Unicode PST if missing
            if (!File.Exists(pstPath))
            {
                try
                {
                    PersonalStorage.Create(pstPath, FileFormatVersion.Unicode);
                    Console.WriteLine($"Created new PST file at '{pstPath}'.");
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to create PST file: {ex.Message}");
                    return;
                }
            }

            // Open the PST for read/write operations
            using (PersonalStorage pst = PersonalStorage.FromFile(pstPath))
            {
                // Access the root folder
                FolderInfo rootFolder = pst.RootFolder;

                // Create a first-level subfolder named "Invoices"
                FolderInfo invoicesFolder;
                try
                {
                    invoicesFolder = rootFolder.AddSubFolder("Invoices");
                    Console.WriteLine("Created subfolder: Invoices");
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Could not create 'Invoices' folder: {ex.Message}");
                    return;
                }

                // Create a nested subfolder "2023" under "Invoices"
                try
                {
                    FolderInfo yearFolder = invoicesFolder.AddSubFolder("2023");
                    Console.WriteLine("Created subfolder: Invoices\\2023");
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Could not create '2023' folder: {ex.Message}");
                }

                // Additional example: create another top‑level folder "Reports"
                try
                {
                    FolderInfo reportsFolder = rootFolder.AddSubFolder("Reports");
                    Console.WriteLine("Created subfolder: Reports");
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Could not create 'Reports' folder: {ex.Message}");
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
