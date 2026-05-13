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
            // Paths
            string pstFilePath = "ImportedMessages.pst";
            string msgDirectoryPath = "MsgFiles";

            // Verify MSG directory exists
            if (!Directory.Exists(msgDirectoryPath))
            {
                Console.Error.WriteLine($"Message directory not found: {msgDirectoryPath}");
                return;
            }

            // Ensure PST file exists; create if missing
            if (!File.Exists(pstFilePath))
            {
                try
                {
                    PersonalStorage.Create(pstFilePath, FileFormatVersion.Unicode);
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to create PST file: {ex.Message}");
                    return;
                }
            }

            // Open PST for read/write
            using (PersonalStorage pst = PersonalStorage.FromFile(pstFilePath))
            {
                // Use the root folder as the destination
                FolderInfo destinationFolder = pst.RootFolder;

                // Process each .msg file
                string[] msgFiles;
                try
                {
                    msgFiles = Directory.GetFiles(msgDirectoryPath, "*.msg");
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to enumerate MSG files: {ex.Message}");
                    return;
                }

                foreach (string msgPath in msgFiles)
                {
                    if (!File.Exists(msgPath))
                    {
                try
                {
                    using (MailMessage placeholder = new MailMessage(
                        "sender@example.com",
                        "recipient@example.com",
                        "Placeholder Subject",
                        "Placeholder body."))
                    {
                        placeholder.Save(msgPath, SaveOptions.DefaultEml);
                    }
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Error creating placeholder message: {ex.Message}");
                    return;
                }

                        Console.Error.WriteLine($"MSG file not found, skipping: {msgPath}");
                        continue;
                    }

                    try
                    {
                        // Load the MSG as a MailMessage
                        using (MailMessage mailMessage = MailMessage.Load(msgPath))
                        {
                            // Convert to MapiMessage
                            MapiMessage mapiMessage = MapiMessage.FromMailMessage(mailMessage);

                            // Preserve original sent date
                            if (mailMessage.Date != DateTime.MinValue)
                            {
                                mapiMessage.ClientSubmitTime = mailMessage.Date;
                            }

                            // Preserve sender information
                            if (mailMessage.From != null)
                            {
                                mapiMessage.SentRepresentingEmailAddress = mailMessage.From.Address;
                                mapiMessage.SentRepresentingName = mailMessage.From.DisplayName;
                            }

                            // Add the message to the PST folder
                            string entryId = destinationFolder.AddMessage(mapiMessage);
                            Console.WriteLine($"Imported: {Path.GetFileName(msgPath)} (EntryId: {entryId})");
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.Error.WriteLine($"Failed to import '{msgPath}': {ex.Message}");
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
