using Aspose.Email.Mapi;
using System;
using System.IO;
using Aspose.Email;
using Aspose.Email.Storage;
using Aspose.Email.Storage.Pst;

class Program
{
    static void Main()
    {
        try
        {
            // Define input and output paths
            string emlPath = "input.eml";
            string ostPath = "output.ost";

            // Ensure input EML exists; create a minimal placeholder if missing
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
                    string placeholder = "Subject: Placeholder\r\n\r\nThis is a placeholder email.";
                    File.WriteAllText(emlPath, placeholder);
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to create placeholder EML: {ex.Message}");
                    return;
                }
            }

            // Ensure output directory exists
            string outputDir = Path.GetDirectoryName(ostPath);
            if (!string.IsNullOrEmpty(outputDir) && !Directory.Exists(outputDir))
            {
                try
                {
                    Directory.CreateDirectory(outputDir);
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to create output directory: {ex.Message}");
                    return;
                }
            }

            // Load the EML message
            using (MailMessage mailMessage = MailMessage.Load(emlPath))
            {
                // Convert to MapiMessage
                using (MapiMessage mapiMessage = MapiMessage.FromMailMessage(mailMessage))
                {
                    // Create OST storage
                    using (PersonalStorage personalStorage = PersonalStorage.Create(ostPath, FileFormatVersion.Unicode))
                    {
                        // Get or create the Inbox folder
                        FolderInfo inboxFolder = personalStorage.RootFolder.GetSubFolder("Inbox");
                        if (inboxFolder == null)
                        {
                            inboxFolder = personalStorage.RootFolder.AddSubFolder("Inbox");
                        }

                        // Add the message to the Inbox
                        inboxFolder.AddMessage(mapiMessage);
                    }
                }
            }

            Console.WriteLine("EML successfully converted to OST.");
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
