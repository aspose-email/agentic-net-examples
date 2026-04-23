using System;
using System.IO;
using Aspose.Email;
using Aspose.Email.Storage.Pst;
using Aspose.Email.Mapi;

class Program
{
    static void Main()
    {
        try
        {
            // Input folder containing MHTML files
            string inputFolderPath = "InputMhtml";
            // Output PST file path
            string outputPstPath = "Converted.pst";

            // Verify input folder exists
            if (!Directory.Exists(inputFolderPath))
            {
                Console.Error.WriteLine($"Input folder does not exist: {inputFolderPath}");
                return;
            }

            // Ensure the directory for the PST file exists
            string outputDirectory = Path.GetDirectoryName(outputPstPath);
            if (!string.IsNullOrEmpty(outputDirectory) && !Directory.Exists(outputDirectory))
            {
                try
                {
                    Directory.CreateDirectory(outputDirectory);
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to create output directory: {ex.Message}");
                    return;
                }
            }

            // Create a new PST file (Unicode format)
            using (PersonalStorage pst = PersonalStorage.Create(outputPstPath, FileFormatVersion.Unicode))
            {
                // Get all .mht and .mhtml files in the input folder
                string[] mhtmlFiles = Directory.GetFiles(inputFolderPath, "*.mht");
                string[] mhtmlAltFiles = Directory.GetFiles(inputFolderPath, "*.mhtml");
                string[] allFiles = new string[mhtmlFiles.Length + mhtmlAltFiles.Length];
                mhtmlFiles.CopyTo(allFiles, 0);
                mhtmlAltFiles.CopyTo(allFiles, mhtmlFiles.Length);

                foreach (string filePath in allFiles)
                {
                    try
                    {
                        if (!File.Exists(filePath))
                        {
                try
                {
                    using (MailMessage placeholder = new MailMessage(
                        "sender@example.com",
                        "recipient@example.com",
                        "Placeholder Subject",
                        "Placeholder body."))
                    {
                        placeholder.Save(filePath, SaveOptions.DefaultEml);
                    }
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Error creating placeholder message: {ex.Message}");
                    return;
                }

                            Console.Error.WriteLine($"File not found, skipping: {filePath}");
                            continue;
                        }

                        // Load the MHTML file into a MailMessage
                        using (MailMessage mailMessage = MailMessage.Load(filePath))
                        {
                            // Convert MailMessage to MapiMessage
                            using (MapiMessage mapiMessage = MapiMessage.FromMailMessage(mailMessage))
                            {
                                // Determine folder name based on file name (without extension)
                                string folderName = Path.GetFileNameWithoutExtension(filePath);
                                // Create subfolder under the PST root
                                FolderInfo targetFolder = pst.RootFolder.AddSubFolder(folderName);
                                // Add the message to the created folder
                                targetFolder.AddMessage(mapiMessage);
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.Error.WriteLine($"Error processing file '{filePath}': {ex.Message}");
                        // Continue with next file
                    }
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
