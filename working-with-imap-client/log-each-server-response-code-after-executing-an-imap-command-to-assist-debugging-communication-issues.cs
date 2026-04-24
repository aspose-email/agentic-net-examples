using System;
using Aspose.Email;
using Aspose.Email.Clients.Imap;
using Aspose.Email.Clients;

class Program
{
    static void Main()
    {
        try
        {
            // Placeholder credentials – skip real network calls in CI
            string host = "imap.example.com";
            string username = "user@example.com";
            string password = "password";

            if (host.Contains("example.com") || username.Contains("example.com"))
            {
                Console.Error.WriteLine("Placeholder IMAP credentials detected. Skipping network operations.");
                return;
            }

            // Create and use the IMAP client
            using (ImapClient client = new ImapClient(host, username, password, SecurityOptions.Auto))
            {
                try
                {
                    // Select the INBOX folder
                    client.SelectFolder("INBOX");
                    Console.WriteLine($"SelectFolder response: {ImapStatusCode.Ok}");

                    // List messages in the selected folder
                    ImapMessageInfoCollection messages = client.ListMessages();
                    Console.WriteLine($"ListMessages response: {ImapStatusCode.Ok}");

                    // Iterate through messages and fetch each one
                    foreach (ImapMessageInfo msgInfo in messages)
                    {
                        try
                        {
                            using (MailMessage message = client.FetchMessage(msgInfo.UniqueId))
                            {
                                Console.WriteLine($"FetchMessage UID {msgInfo.UniqueId} response: {ImapStatusCode.Ok}");
                                // Optionally process the message here
                            }
                        }
                        catch (ImapException fetchEx)
                        {
                            Console.Error.WriteLine($"FetchMessage UID {msgInfo.UniqueId} failed: {fetchEx.Message}");
                        }
                    }
                }
                catch (ImapException imapEx)
                {
                    Console.Error.WriteLine($"IMAP operation failed: {imapEx.Message}");
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
