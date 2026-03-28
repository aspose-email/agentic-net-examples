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
            // Paths – adjust as needed
            string pstFilePath = "ExistingStorage.pst";
            string sourceFolderPath = "EmailFiles";

            // Verify PST file exists
            if (!File.Exists(pstFilePath))
            {
                Console.Error.WriteLine($"Error: PST file not found – {pstFilePath}");
                return;
            }

            // Verify source folder exists
            if (!Directory.Exists(sourceFolderPath))
            {
                Console.Error.WriteLine($"Error: Source folder not found – {sourceFolderPath}");
                return;
            }

            // Open the PST file for read/write
            using (PersonalStorage pst = PersonalStorage.FromFile(pstFilePath))
            {
                // Ensure the PST is writable
                if (!pst.CanWrite)
                {
                    Console.Error.WriteLine("Error: PST file is read‑only.");
                    return;
                }

                // Use the root folder (or change to a specific subfolder if required)
                FolderInfo targetFolder = pst.RootFolder;

                // Get all .eml files in the source folder
                string[] emlFiles;
                try
                {
                    emlFiles = Directory.GetFiles(sourceFolderPath, "*.eml");
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Error accessing files in folder: {ex.Message}");
                    return;
                }

                foreach (string emlPath in emlFiles)
                {
                    // Load the email message
                    MailMessage mailMessage;
                    try
                    {
                        mailMessage = MailMessage.Load(emlPath);
                    }
                    catch (Exception ex)
                    {
                        Console.Error.WriteLine($"Failed to load message '{emlPath}': {ex.Message}");
                        continue;
                    }

                    // Convert to MAPI message
                    MapiMessage mapiMessage;
                    try
                    {
                        mapiMessage = MapiMessage.FromMailMessage(mailMessage);
                    }
                    catch (Exception ex)
                    {
                        Console.Error.WriteLine($"Conversion to MAPI failed for '{emlPath}': {ex.Message}");
                        continue;
                    }

                    // Add the message to the PST folder
                    try
                    {
                        string entryId = targetFolder.AddMessage(mapiMessage);
                        Console.WriteLine($"Added message '{Path.GetFileName(emlPath)}' – EntryId: {entryId}");
                    }
                    catch (Exception ex)
                    {
                        Console.Error.WriteLine($"Failed to add message '{emlPath}' to PST: {ex.Message}");
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
