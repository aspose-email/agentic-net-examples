using Aspose.Email.PersonalInfo;
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
            // Path to the PST file.
            string pstPath = "sample.pst";

            // Ensure the directory for the PST file exists.
            string pstDirectory = Path.GetDirectoryName(pstPath);
            if (!string.IsNullOrEmpty(pstDirectory) && !Directory.Exists(pstDirectory))
            {
                Directory.CreateDirectory(pstDirectory);
            }

            // If the PST file does not exist, create a new Unicode PST.
            if (!File.Exists(pstPath))
            {
                // Create a new PST with Unicode format (supports large PSTs).
                PersonalStorage.Create(pstPath, FileFormatVersion.Unicode);
                Console.WriteLine($"Created new PST file at '{pstPath}'.");
            }

            // Open the PST file for read/write operations.
            using (PersonalStorage pst = PersonalStorage.FromFile(pstPath, false))
            {
                // Get the root folder of the PST.
                FolderInfo rootFolder = pst.RootFolder;

                // Add a new subfolder (or retrieve it if it already exists).
                FolderInfo targetFolder;
                try
                {
                    targetFolder = rootFolder.AddSubFolder("MyFolder");
                    Console.WriteLine("Created subfolder 'MyFolder'.");
                }
                catch (InvalidOperationException)
                {
                    // Folder already exists; retrieve it.
                    targetFolder = rootFolder.GetSubFolder("MyFolder");
                    Console.WriteLine("Subfolder 'MyFolder' already exists; using existing folder.");
                }

                // Modify the container class of the folder.
                // Common container class values: "IPF.Note" (email), "IPF.Contact" (contacts), etc.
                targetFolder.ChangeContainerClass("IPF.Note");
                Console.WriteLine("Changed container class of 'MyFolder' to 'IPF.Note'.");

                // No explicit save call is required; changes are persisted when the PST is disposed.
            }

            Console.WriteLine("PST modification completed successfully.");
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
