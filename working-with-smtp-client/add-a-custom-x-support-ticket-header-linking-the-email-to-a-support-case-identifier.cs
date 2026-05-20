using System;
using System.IO;
using System.Net;
using Aspose.Email;
using Aspose.Email.Clients.Exchange.WebService;

class Program
{
    static void Main()
    {
        try
        {
            // Define email details
            string from = "support@example.com";
            string to = "customer@example.com";
            string subject = "Your Support Case";
            string body = "Dear Customer,\n\nPlease find your support case reference below.\n\nBest regards,\nSupport Team";

            // Custom support ticket identifier
            string supportTicketId = "CASE-12345";

            // Create the mail message and add the custom header
            using (MailMessage message = new MailMessage(from, to, subject, body))
            {
                message.Headers.Add("X-Support-Ticket", supportTicketId);

                // Save the message to a local .eml file
                string outputPath = "SupportTicketMessage.eml";
                try
                {
                    string directory = Path.GetDirectoryName(outputPath);
                    if (!string.IsNullOrEmpty(directory) && !Directory.Exists(directory))
                    {
                        Directory.CreateDirectory(directory);
                    }

                    using (FileStream fs = new FileStream(outputPath, FileMode.Create, FileAccess.Write))
                    {
                        message.Save(fs, SaveOptions.DefaultEml);
                    }

                    Console.WriteLine($"Message saved to '{outputPath}'.");
                }
                catch (Exception ioEx)
                {
                    Console.Error.WriteLine($"File I/O error: {ioEx.Message}");
                    return;
                }

                // Optional: send via Exchange if real credentials are provided
                string mailboxUri = "https://exchange.example.com/EWS/Exchange.asmx";
                string username = "user@example.com";
                string password = "password";

                // Guard against placeholder credentials
                if (mailboxUri.Contains("example.com") || username.Contains("example.com") || password == "password")
                {
                    Console.WriteLine("Placeholder credentials detected. Skipping send operation.");
                    return;
                }

                // Create and use the Exchange client
                IEWSClient client = null;
                try
                {
                    client = EWSClient.GetEWSClient(mailboxUri, username, password);
                    client.Send(message);
                    Console.WriteLine("Message sent successfully.");
                }
                catch (Exception clientEx)
                {
                    Console.Error.WriteLine($"Client error: {clientEx.Message}");
                }
                finally
                {
                    if (client is IDisposable disposableClient)
                    {
                        disposableClient.Dispose();
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
