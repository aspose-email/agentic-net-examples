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
            string directory = Path.GetDirectoryName(pstPath);
            if (!string.IsNullOrEmpty(directory) && !Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }

            // Create a new PST file if it does not exist
            if (!File.Exists(pstPath))
            {
                try
                {
                    PersonalStorage.Create(pstPath, FileFormatVersion.Unicode);
                    Console.WriteLine($"Created new PST file at {pstPath}");
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Error creating PST file: {ex.Message}");
                    return;
                }
            }

            // Open the PST file
            using (PersonalStorage pst = PersonalStorage.FromFile(pstPath))
            {
                // Verify that the PST is writable
                if (!pst.CanWrite)
                {
                    Console.Error.WriteLine("PST file is read‑only. Cannot add sub‑folders.");
                    return;
                }

                // Get the root folder of the PST
                FolderInfo rootFolder = pst.RootFolder;

                // Create a top‑level folder named "Projects"
                FolderInfo projectsFolder = rootFolder.AddSubFolder("Projects");

                // Create a sub‑folder named "2024" under "Projects"
                FolderInfo yearFolder = projectsFolder.AddSubFolder("2024");

                // Create a nested hierarchy in one call using FolderCreationOptions
                FolderCreationOptions options = new FolderCreationOptions
                {
                    CreateHierarchy = true
                };
                // This creates "Archives\2023" under the root folder
                FolderInfo archiveFolder = rootFolder.AddSubFolder(@"Archives\2023", options);

                Console.WriteLine("Sub‑folders created successfully.");
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
