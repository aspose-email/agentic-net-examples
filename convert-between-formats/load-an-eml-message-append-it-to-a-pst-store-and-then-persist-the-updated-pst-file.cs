using Aspose.Email;
using Aspose.Email.Mapi;
using Aspose.Email.Storage.Pst;
using System;
using System.IO;

class Program
{
    static void Main()
    {
        try
        {
            // Define file paths
            string emlPath = "source.eml";
            string pstPath = "output.pst";

            // Verify source EML exists
            if (!File.Exists(emlPath))
            {
                try
                {
                    using (MailMessage placeholder = new MailMessage(
                        "sender@example.com",
                        "recipient@example.com",
                        "Placeholder Subject",
                        "Placeholder body."))
                    {
                        placeholder.Save(emlPath, SaveOptions.DefaultEml);
                    }
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Error creating placeholder message: {ex.Message}");
                    return;
                }

                try
                {
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Error creating placeholder message: {ex.Message}");
                    return;
                }

                Console.Error.WriteLine($"EML file not found: {emlPath}");
                return;
            }

            // Ensure directory for PST exists
            string pstDirectory = Path.GetDirectoryName(pstPath);
            if (!string.IsNullOrEmpty(pstDirectory) && !Directory.Exists(pstDirectory))
            {
                try
                {
                    Directory.CreateDirectory(pstDirectory);
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to create PST directory: {ex.Message}");
                    return;
                }
            }

            // Load or create PST storage
            PersonalStorage pstStorage;
            if (File.Exists(pstPath))
            {
                try
                {
                    pstStorage = PersonalStorage.FromFile(pstPath);
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to load PST file: {ex.Message}");
                    return;
                }
            }
            else
            {
                try
                {
                    pstStorage = PersonalStorage.Create(pstPath, FileFormatVersion.Unicode);
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to create PST file: {ex.Message}");
                    return;
                }
            }

            // Use PST within a using block to ensure proper disposal
            using (pstStorage)
            {
                // Load the EML message
                MailMessage mailMessage;
                try
                {
                    mailMessage = MailMessage.Load(emlPath);
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to load EML message: {ex.Message}");
                    return;
                }

                // Get (or create) the Inbox folder in the PST
                FolderInfo inboxFolder;
                try
                {
                    inboxFolder = pstStorage.GetPredefinedFolder(StandardIpmFolder.Inbox);
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to get Inbox folder: {ex.Message}");
                    return;
                }

                // Append the message to the folder
                try
                {
                    inboxFolder.AddMessage(MapiMessage.FromMailMessage(mailMessage));
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to add message to PST: {ex.Message}");
                    return;
                }

                // No explicit SaveAs needed; disposing the storage persists changes.
            }

            Console.WriteLine("EML message successfully appended to PST.");
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
