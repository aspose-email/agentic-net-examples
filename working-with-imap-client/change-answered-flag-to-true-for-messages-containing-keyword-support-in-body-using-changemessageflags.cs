using System;
using Aspose.Email;
using Aspose.Email.Clients;
using Aspose.Email.Clients.Imap;

class Program
{
    static void Main()
    {
        try
        {
            // Placeholder credentials detection
            string host = "imap.example.com";
            string username = "user@example.com";
            string password = "password";

            if (host.Contains("example.com") || username.Contains("example.com"))
            {
                Console.Error.WriteLine("Placeholder credentials detected. Skipping execution.");
                return;
            }

            // Create and connect the IMAP client
            try
            {
                using (ImapClient client = new ImapClient(host, username, password, SecurityOptions.Auto))
                {
                    // Select the INBOX folder
                    client.SelectFolder("INBOX");

                    // Retrieve all messages in the folder
                    ImapMessageInfoCollection messages = client.ListMessages();

                    foreach (ImapMessageInfo messageInfo in messages)
                    {
                        // Fetch the full message to inspect its body
                        MailMessage mailMessage = null;
                        try
                        {
                            mailMessage = client.FetchMessage(messageInfo.UniqueId);
                        }
                        catch (Exception fetchEx)
                        {
                            Console.Error.WriteLine($"Failed to fetch message {messageInfo.UniqueId}: {fetchEx.Message}");
                            continue;
                        }

                        if (mailMessage != null && mailMessage.Body != null && mailMessage.Body.Contains("support"))
                        {
                            try
                            {
                                // Set the Answered flag for this message
                                client.ChangeMessageFlags(messageInfo.UniqueId, ImapMessageFlags.Answered);
                                Console.WriteLine($"Answered flag set for message UID {messageInfo.UniqueId}");
                            }
                            catch (Exception flagEx)
                            {
                                Console.Error.WriteLine($"Failed to change flags for message {messageInfo.UniqueId}: {flagEx.Message}");
                            }
                        }

                        mailMessage?.Dispose();
                    }
                }
            }
            catch (Exception clientEx)
            {
                Console.Error.WriteLine($"IMAP client error: {clientEx.Message}");
                return;
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
