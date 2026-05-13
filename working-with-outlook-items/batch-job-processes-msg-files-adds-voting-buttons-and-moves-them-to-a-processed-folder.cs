using Aspose.Email.Clients;
using System;
using System.IO;
using Aspose.Email;
using Aspose.Email.Clients.Imap;
using Aspose.Email.Mapi;

class Program
{
    static void Main()
    {
        try
        {
            // Placeholder IMAP connection settings
            string imapHost = "imap.example.com";
            string imapUsername = "user@example.com";
            string imapPassword = "password";

            // Skip execution when placeholder credentials are detected
            if (imapHost.Contains("example.com"))
            {
                Console.WriteLine("Placeholder IMAP settings detected. Skipping execution.");
                return;
            }

            // Local folder containing MSG files
            string sourceFolderPath = @"C:\Messages";
            // Destination folder on the IMAP server
            string processedFolderName = "Processed";

            // Verify source folder existence
            if (!Directory.Exists(sourceFolderPath))
            {
                Console.Error.WriteLine($"Source folder does not exist: {sourceFolderPath}");
                return;
            }

            // Get all MSG files in the source folder
            string[] msgFiles;
            try
            {
                msgFiles = Directory.GetFiles(sourceFolderPath, "*.msg");
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Failed to enumerate MSG files: {ex.Message}");
                return;
            }

            // Connect to the IMAP server
            using (ImapClient client = new ImapClient(imapHost, imapUsername, imapPassword, SecurityOptions.Auto))
            {
                try
                {
                    client.ValidateCredentials();
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"IMAP authentication failed: {ex.Message}");
                    return;
                }

                // Ensure the destination folder exists
                try
                {
                    if (!client.ExistFolder(processedFolderName))
                    {
                        client.CreateFolder(processedFolderName);
                    }
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to ensure processed folder: {ex.Message}");
                    return;
                }

                // Process each MSG file
                foreach (string msgFilePath in msgFiles)
                {
                    // Guard against missing file (should not happen after GetFiles, but safe)
                    if (!File.Exists(msgFilePath))
                    {
                try
                {
                    using (MapiMessage placeholder = new MapiMessage(
                        "from@example.com",
                        "to@example.com",
                        "Placeholder Subject",
                        "Placeholder body."))
                    {
                        placeholder.Save(msgFilePath);
                    }
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Error creating placeholder MSG: {ex.Message}");
                    return;
                }

                        Console.Error.WriteLine($"File not found: {msgFilePath}");
                        continue;
                    }

                    try
                    {
                        // Load the MSG file as a MAPI message
                        using (MapiMessage mapiMessage = MapiMessage.Load(msgFilePath))
                        {
                            // Add a voting button to the message
                            FollowUpManager.AddVotingButton(mapiMessage, "Approve");

                            // Convert MAPI message to MailMessage for upload
                            MailMessage mailMessage = mapiMessage.ToMailMessage(new MailConversionOptions());

                            // Upload the message to the "Processed" folder
                            client.AppendMessage(processedFolderName, mailMessage);
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.Error.WriteLine($"Error processing file '{msgFilePath}': {ex.Message}");
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
