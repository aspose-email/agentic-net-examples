using Aspose.Email.Clients;
using System;
using System.IO;
using Aspose.Email;
using Aspose.Email.Clients.Imap;

class Program
{
    static void Main()
    {
        try
        {
            // Placeholder IMAP connection settings
            string host = "imap.example.com";
            int port = 993;
            string username = "username";
            string password = "password";

            // Detect placeholder credentials and skip actual network call
            if (host.Contains("example.com") || username.Equals("username", StringComparison.OrdinalIgnoreCase))
            {
                Console.Error.WriteLine("Placeholder IMAP credentials detected. Skipping network operations.");
                return;
            }

            // Output directory for EML files
            string outputDir = "ExportedEml";
            try
            {
                if (!Directory.Exists(outputDir))
                {
                    Directory.CreateDirectory(outputDir);
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Failed to create output directory: {ex.Message}");
                return;
            }

            // Connect to IMAP server and process messages
            using (ImapClient client = new ImapClient())
            {
                try
                {
                    client.Host = host;
                    client.Port = port;
                    client.Username = username;
                    client.Password = password;
                    client.SecurityOptions = SecurityOptions.Auto;
                    client.ValidateCredentials();
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"IMAP connection/authentication failed: {ex.Message}");
                    return;
                }

                // Select the INBOX folder
                try
                {
                    client.SelectFolder("INBOX");
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to select folder: {ex.Message}");
                    return;
                }

                // Retrieve list of message UIDs
                ImapMessageInfoCollection messages;
                try
                {
                    messages = client.ListMessages();
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to list messages: {ex.Message}");
                    return;
                }

                // Prepare save options to preserve embedded content
                EmlSaveOptions saveOptions = new EmlSaveOptions(MailMessageSaveType.EmlFormat)
                {
                    PreserveEmbeddedMessageFormat = true
                };

                // Process each message
                foreach (ImapMessageInfo info in messages)
                {
                    try
                    {
                        // Fetch the message as MailMessage
                        MailMessage mail = client.FetchMessage(info.UniqueId);
                        if (mail == null)
                        {
                            Console.Error.WriteLine($"Message UID {info.UniqueId} could not be fetched.");
                            continue;
                        }

                        // Build a safe file name
                        string safeSubject = string.IsNullOrWhiteSpace(mail.Subject) ? "NoSubject" : mail.Subject;
                        foreach (char c in Path.GetInvalidFileNameChars())
                        {
                            safeSubject = safeSubject.Replace(c, '_');
                        }
                        string emlPath = Path.Combine(outputDir, $"{safeSubject}_{info.UniqueId}.eml");

                        // Save the message as EML with the specified options
                        mail.Save(emlPath, saveOptions);
                    }
                    catch (Exception ex)
                    {
                        Console.Error.WriteLine($"Error processing message UID {info.UniqueId}: {ex.Message}");
                        // Continue with next message
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
