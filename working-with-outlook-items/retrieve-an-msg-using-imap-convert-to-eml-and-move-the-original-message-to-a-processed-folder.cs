using System;
using System.IO;
using Aspose.Email;
using Aspose.Email.Clients;
using Aspose.Email.Clients.Imap;

class Program
{
    static void Main()
    {
        try
        {
            // IMAP server connection settings
            string host = "imap.example.com";
            int port = 993;
            string username = "username";
            string password = "password";

            // Guard against placeholder credentials
            if (host.Contains("example.com") ||
                username.Equals("username", StringComparison.OrdinalIgnoreCase) ||
                password.Equals("password", StringComparison.OrdinalIgnoreCase))
            {
                Console.Error.WriteLine("Placeholder credentials detected. Skipping network operations.");
                return;
            }

            // Create and connect the IMAP client
            using (ImapClient client = new ImapClient(host, port, username, password, SecurityOptions.Auto))
            {
                try
                {
                    // Select the source folder
                    string sourceFolder = "INBOX";
                    client.SelectFolder(sourceFolder);

                    // Ensure the destination folder exists
                    string processedFolder = "Processed";
                    if (!client.ExistFolder(processedFolder))
                    {
                        client.CreateFolder(processedFolder);
                    }

                    // Retrieve the list of messages in the folder
                    ImapMessageInfoCollection messages = client.ListMessages();
                    if (messages == null || messages.Count == 0)
                    {
                        Console.WriteLine("No messages found in the folder.");
                        return;
                    }

                    // Process the first message (for demonstration)
                    ImapMessageInfo messageInfo = messages[0];
                    string messageUid = messageInfo.UniqueId;

                    // Fetch the message as a MailMessage
                    using (MailMessage mailMessage = client.FetchMessage(messageUid))
                    {
                        // Prepare the output directory
                        string outputDirectory = Path.Combine(Environment.CurrentDirectory, "Output");
                        if (!Directory.Exists(outputDirectory))
                        {
                            Directory.CreateDirectory(outputDirectory);
                        }

                        // Define the EML file path
                        string emlPath = Path.Combine(outputDirectory, $"{Guid.NewGuid()}.eml");

                        // Save the message as EML
                        try
                        {
                            mailMessage.Save(emlPath);
                            Console.WriteLine($"Message saved as EML to: {emlPath}");
                        }
                        catch (Exception ex)
                        {
                            Console.Error.WriteLine($"Failed to save EML file: {ex.Message}");
                            return;
                        }
                    }

                    // Move the original message to the processed folder
                    try
                    {
                        client.MoveMessage(messageUid, processedFolder);
                        Console.WriteLine("Message moved to the processed folder.");
                    }
                    catch (Exception ex)
                    {
                        Console.Error.WriteLine($"Failed to move message: {ex.Message}");
                    }
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"IMAP operation failed: {ex.Message}");
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
