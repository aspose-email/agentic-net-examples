using System;
using Aspose.Email;
using Aspose.Email.Clients.Imap;
using Aspose.Email.Clients;
using Aspose.Email.Clients.Imap.Models;

namespace ImapHeaderExample
{
    class Program
    {
        static void Main()
        {
            try
            {
                // Placeholder IMAP server credentials
                string host = "imap.example.com";
                string username = "user@example.com";
                string password = "password";

                // Skip execution when placeholder credentials are detected
                if (host.Contains("example.com"))
                {
                    Console.WriteLine("Skipping IMAP operations due to placeholder credentials.");
                    return;
                }

                // Connect to the IMAP server and process messages
                using (ImapClient client = new ImapClient(host, username, password, SecurityOptions.Auto))
                {
                    try
                    {
                        // Select the INBOX folder
                        client.SelectFolder("INBOX");

                        // Retrieve the list of messages in the folder
                        ImapMessageInfoCollection messages = client.ListMessages();

                        foreach (ImapMessageInfo info in messages)
                        {
                            // Fetch the full message
                            MailMessage message = client.FetchMessage(info.UniqueId);

                            // Add a custom header to indicate processing
                            message.Headers.Add("X-Processed", "true");

                            // Append the modified message back to the same folder
                            client.AppendMessageAsync("INBOX", message).Wait();

                            // Delete the original message (commit the deletion)
                            client.DeleteMessage(info.UniqueId, true);
                        }
                    }
                    catch (ImapException imapEx)
                    {
                        Console.Error.WriteLine($"IMAP error: {imapEx.Message}");
                        return;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Unexpected error: {ex.Message}");
                return;
            }
        }
    }
}
