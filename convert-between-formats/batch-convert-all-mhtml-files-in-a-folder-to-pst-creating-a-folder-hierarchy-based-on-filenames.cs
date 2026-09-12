using System;
using System.IO;
using Aspose.Email;
using Aspose.Email.Mapi;
using Aspose.Email.Storage.Pst;

class Program
{
    static void Main()
    {
        try
        {
            // Input directory containing MHTML files
            string inputFolder = "InputMhtml";
            // Output PST file path
            string outputPstPath = "Output\\Converted.pst";

            // Verify input folder exists
            if (!Directory.Exists(inputFolder))
            {
                Console.Error.WriteLine($"Input folder does not exist: {inputFolder}");
                return;
            }

            // Ensure output directory exists
            string outputDirectory = Path.GetDirectoryName(outputPstPath);
            if (!string.IsNullOrEmpty(outputDirectory) && !Directory.Exists(outputDirectory))
            {
                Directory.CreateDirectory(outputDirectory);
            }

            // Create a new PST file (Unicode version)
            using (PersonalStorage pst = PersonalStorage.Create(outputPstPath, FileFormatVersion.Unicode))
            {
                // Enumerate all .mht (MHTML) files in the input folder
                foreach (string mhtmlFilePath in Directory.EnumerateFiles(inputFolder, "*.mht"))
                {
                    try
                    {
                        // Load the MHTML file into a MailMessage
                        MailMessage mailMessage = MailMessage.Load(mhtmlFilePath);

                        // Convert MailMessage to MapiMessage for PST storage
                        MapiMessage mapiMessage = MapiMessage.FromMailMessage(mailMessage);

                        // Determine folder name based on the file name (without extension)
                        string folderName = Path.GetFileNameWithoutExtension(mhtmlFilePath);

                        // Get existing subfolder or create a new one under the PST root
                        FolderInfo targetFolder = pst.RootFolder.GetSubFolder(folderName);
                        if (targetFolder == null)
                        {
                            targetFolder = pst.RootFolder.AddSubFolder(folderName);
                        }

                        // Add the message to the target folder
                        targetFolder.AddMessage(mapiMessage);
                    }
                    catch (Exception ex)
                    {
                        Console.Error.WriteLine($"Failed to process '{mhtmlFilePath}': {ex.Message}");
                        // Continue with next file
                    }
                }
            }

            Console.WriteLine("Conversion completed successfully.");
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
