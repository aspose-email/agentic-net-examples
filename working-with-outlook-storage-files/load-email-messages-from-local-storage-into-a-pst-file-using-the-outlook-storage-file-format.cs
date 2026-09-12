using System;
using System.IO;
using Aspose.Email;
using Aspose.Email.Mapi;
using Aspose.Email.Storage.Pst;

namespace AsposeEmailPstImport
{
    class Program
    {
        static void Main(string[] args)
        {
            // Define source directory containing .msg files and target PST file path
            string sourceDirectory = "Emails";
            string pstFilePath = "output.pst";

            // Verify source directory exists
            if (!Directory.Exists(sourceDirectory))
            {
                Console.Error.WriteLine($"Source directory \"{sourceDirectory}\" does not exist.");
                return;
            }

            // If a PST file already exists, delete it to start fresh
            try
            {
                if (File.Exists(pstFilePath))
                {
                    File.Delete(pstFilePath);
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Failed to delete existing PST file: {ex.Message}");
                return;
            }

            // Create the PST file and import messages
            try
            {
                // Create a new PST with Unicode format
                using (PersonalStorage pst = PersonalStorage.Create(pstFilePath, FileFormatVersion.Unicode))
                {
                    // Create a predefined folder named "Imported" under the Inbox IPM folder
                    FolderInfo importFolder = pst.CreatePredefinedFolder("Imported", StandardIpmFolder.Inbox);

                    // Get all .msg files from the source directory
                    string[] msgFiles = Directory.GetFiles(sourceDirectory, "*.msg");
                    foreach (string msgFilePath in msgFiles)
                    {
                        // Load the .msg file as a MapiMessage
                        using (MapiMessage mapMsg = MapiMessage.Load(msgFilePath))
                        {
                            // Add the message to the PST folder
                            importFolder.AddMessage(mapMsg);
                        }
                    }

                    // Optionally, display folder statistics
                    Console.WriteLine($"Folder \"{importFolder.DisplayName}\" contains {importFolder.ContentCount} messages.");
                }

                Console.WriteLine($"PST file created successfully at \"{pstFilePath}\".");
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"An error occurred while creating the PST: {ex.Message}");
            }
        }
    }
}
