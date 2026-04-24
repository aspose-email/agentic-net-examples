using Aspose.Email.Mime;
using System;
using Aspose.Email;
using Aspose.Email.Clients;
using Aspose.Email.Clients.Imap;

public class Program
{
    public static void Main()
    {
        try
        {
            // Connection parameters
            string host = "imap.example.com";
            int port = 993;
            string username = "user@example.com";
            string password = "password";
            SecurityOptions security = SecurityOptions.SSLImplicit;

            // Guard against placeholder credentials
            if (host.Contains("example.com") || username.Contains("example.com") || password == "password")
            {
                Console.Error.WriteLine("Placeholder IMAP credentials detected. Skipping execution.");
                return;
            }

            // Create and use ImapClient
            using (ImapClient client = new ImapClient(host, port, username, password, security))
            {
                try
                {
                    // Select the INBOX folder
                    client.SelectFolder("INBOX");

                    // Retrieve message infos (headers only)
                    ImapMessageInfoCollection messages = client.ListMessages();

                    foreach (ImapMessageInfo messageInfo in messages)
                    {
                        HeaderCollection headers = messageInfo.Headers;

                        Console.WriteLine($"Message UID: {messageInfo.UniqueId}");
                        foreach (string headerName in headers.AllKeys)
                        {
                            Console.WriteLine($"{headerName}: {headers[headerName]}");
                        }
                        Console.WriteLine(new string('-', 40));
                    }
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"IMAP operation failed: {ex.Message}");
                    return;
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
