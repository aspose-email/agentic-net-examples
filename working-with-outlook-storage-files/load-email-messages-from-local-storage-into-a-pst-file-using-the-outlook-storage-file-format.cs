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
            // Paths (adjust as needed)
            string inputFolderPath = "InputEmails";
            string outputPstPath = "OutputMessages.pst";

            // Verify input folder exists
            if (!Directory.Exists(inputFolderPath))
            {
                Console.Error.WriteLine($"Error: Input folder not found – {inputFolderPath}");
                return;
            }

            // Ensure output directory exists
            string outputDirectory = Path.GetDirectoryName(outputPstPath);
            if (!string.IsNullOrEmpty(outputDirectory) && !Directory.Exists(outputDirectory))
            {
                try
                {
                    Directory.CreateDirectory(outputDirectory);
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Error: Unable to create output directory – {ex.Message}");
                    return;
                }
            }

            // Create a new PST file (Unicode version)
            using (PersonalStorage pst = PersonalStorage.Create(outputPstPath, FileFormatVersion.Unicode))
            {
                // Get the predefined Inbox folder
                FolderInfo inboxFolder = pst.GetPredefinedFolder(StandardIpmFolder.Inbox);

                // Process each .eml file in the input folder
                string[] emlFiles;
                try
                {
                    emlFiles = Directory.GetFiles(inputFolderPath, "*.eml");
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Error: Unable to enumerate .eml files – {ex.Message}");
                    return;
                }

                foreach (string emlFilePath in emlFiles)
                {
                    if (!File.Exists(emlFilePath))
                    {
                try
                {
                    using (MailMessage placeholder = new MailMessage(
                        "sender@example.com",
                        "recipient@example.com",
                        "Placeholder Subject",
                        "Placeholder body."))
                    {
                        placeholder.Save(emlFilePath, SaveOptions.DefaultEml);
                    }
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Error creating placeholder message: {ex.Message}");
                    return;
                }

                        Console.Error.WriteLine($"Warning: File not found – {emlFilePath}");
                        continue;
                    }

                    try
                    {
                        // Load the email message from the .eml file
                        using (MailMessage mailMessage = MailMessage.Load(emlFilePath))
                        {
                            // Convert to MAPI message
                            MapiMessage mapiMessage = MapiMessage.FromMailMessage(mailMessage);

                            // Add the message to the PST Inbox folder
                            inboxFolder.AddMessage(mapiMessage);

                            Console.WriteLine($"Added: {mailMessage.Subject}");
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.Error.WriteLine($"Error processing file '{emlFilePath}': {ex.Message}");
                    }
                }
            }

            Console.WriteLine("PST creation completed successfully.");
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
