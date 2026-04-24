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
            // Placeholder credentials – skip execution if they are not replaced.
            string host = "imap.example.com";
            string username = "username";
            string password = "password";

            if (string.IsNullOrEmpty(host) || string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password) ||
                host.Contains("example.com"))
            {
                Console.WriteLine("Placeholder credentials detected. Skipping execution.");
                return;
            }

            // Ensure output directory exists.
            string outputDirectory = "SavedMessages";
            try
            {
                if (!Directory.Exists(outputDirectory))
                {
                    Directory.CreateDirectory(outputDirectory);
                }
            }
            catch (Exception ioEx)
            {
                Console.Error.WriteLine($"Failed to prepare output directory: {ioEx.Message}");
                return;
            }

            // Create and use the IMAP client.
            using (ImapClient client = new ImapClient(host, username, password))
            {
                try
                {
                    // Select the INBOX folder.
                    client.SelectFolder("INBOX");

                    // Retrieve the list of messages.
                    ImapMessageInfoCollection messageInfos = client.ListMessages();

                    int index = 0;
                    foreach (ImapMessageInfo messageInfo in messageInfos)
                    {
                        // Fetch each message.
                        using (MailMessage mailMessage = client.FetchMessage(messageInfo.UniqueId))
                        {
                            string fileName = $"Message_{index}_{messageInfo.UniqueId}.msg";
                            string filePath = Path.Combine(outputDirectory, fileName);

                            // Save the message in MSG format.
                            try
                            {
                                mailMessage.Save(filePath, new MsgSaveOptions(MailMessageSaveType.OutlookMessageFormat));
                            }
                            catch (Exception saveEx)
                            {
                                Console.Error.WriteLine($"Failed to save message {messageInfo.UniqueId}: {saveEx.Message}");
                            }
                        }

                        index++;
                    }
                }
                catch (Exception clientEx)
                {
                    Console.Error.WriteLine($"IMAP operation failed: {clientEx.Message}");
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
