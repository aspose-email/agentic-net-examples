using System;
using Aspose.Email;
using Aspose.Email.Clients.Exchange.WebService;
using Aspose.Email.Clients.Exchange;

class Program
{
    static void Main()
    {
        try
        {
            // Exchange server connection details (replace with real values)
            string mailboxUri = "https://mail.example.com/EWS/Exchange.asmx";
            string username = "user@example.com";
            string password = "password";

            // Skip external calls when placeholder credentials are used
            if (mailboxUri.Contains("example.com") ||
                username.Contains("example.com") ||
                password == "password")
            {
                Console.Error.WriteLine("Placeholder credentials detected. Skipping external calls.");
                return;
            }

            // Create EWS client
            using (IEWSClient client = EWSClient.GetEWSClient(mailboxUri, username, password))
            {
                // Retrieve messages from the default Inbox folder
                ExchangeMessageInfoCollection messages = client.ListMessages();
                if (messages == null || messages.Count == 0)
                {
                    Console.WriteLine("No messages found in the mailbox.");
                    return;
                }

                // Use the first message's unique URI
                string messageUri = messages[0].UniqueUri;

                // Fetch the full MailMessage
                MailMessage mail = client.FetchMessage(messageUri);

                // Output all 'Received' headers to trace delivery path
                Console.WriteLine("Received headers:");
                foreach (string key in mail.Headers.AllKeys)
                {
                    if (key.Equals("Received", StringComparison.OrdinalIgnoreCase))
                    {
                        string[] values = mail.Headers.GetValues(key);
                        if (values != null)
                        {
                            foreach (string value in values)
                            {
                                Console.WriteLine(value);
                            }
                        }
                    }
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
