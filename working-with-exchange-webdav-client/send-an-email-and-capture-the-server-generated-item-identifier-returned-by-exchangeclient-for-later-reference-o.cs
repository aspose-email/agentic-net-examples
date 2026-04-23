using System;
using System.Net;
using Aspose.Email;
using Aspose.Email.Clients.Exchange.Dav;
using Aspose.Email.Clients.Exchange;

class Program
{
    static void Main()
    {
        try
        {
            // Placeholder connection details – replace with real values.
            string mailboxUri = "https://exchange.example.com/ews/exchange.asmx";
            string username = "user@example.com";
            string password = "password";

            // Skip execution when placeholder data is detected to avoid unwanted network calls.
            if (mailboxUri.Contains("example.com") || username.Contains("example.com"))
            {
                Console.Error.WriteLine("Placeholder credentials detected. Skipping execution.");
                return;
            }

            // Create and use the Exchange client.
            using (ExchangeClient client = new ExchangeClient(mailboxUri, username, password))
            {
                try
                {
                    // Build the email message.
                    using (MailMessage message = new MailMessage())
                    {
                        message.From = "user@example.com";
                        message.To.Add("recipient@example.com");
                        message.Subject = "Test Email";
                        message.Body = "Hello from Aspose.Email";

                        // Send the message.
                        client.Send(message);
                        Console.WriteLine("Email sent successfully.");
                    }

                    // After sending, list messages in the Inbox (or Sent Items if available) to obtain the server‑generated ID.
                    string folderUri = client.MailboxInfo.InboxUri; // using Inbox as a safe fallback
                    ExchangeMessageInfoCollection infos = client.ListMessages(folderUri);

                    // Capture the identifier of the most recent message.
                    foreach (ExchangeMessageInfo info in infos)
                    {
                        Console.WriteLine($"Captured Message ID: {info.MessageId}");
                        // Store or use the ID as needed, then exit the loop.
                        break;
                    }
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Exchange operation failed: {ex.Message}");
                    return;
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unhandled exception: {ex.Message}");
        }
    }
}
