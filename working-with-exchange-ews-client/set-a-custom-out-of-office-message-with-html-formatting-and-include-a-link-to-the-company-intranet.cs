using System;
using System.Net;
using Aspose.Email;
using Aspose.Email.Clients.Exchange.WebService;

class Program
{
    static void Main()
    {
        try
        {
            // Define mailbox URI and credentials (replace with real values)
            string mailboxUri = "https://mail.example.com/EWS/Exchange.asmx";

            // Skip external calls when placeholder credentials are used
            if (mailboxUri.Contains("example.com"))
            {
                Console.Error.WriteLine("Placeholder credentials detected. Skipping external calls.");
                return;
            }

            ICredentials credentials = new NetworkCredential("username", "password");

            // Create EWS client
            using (IEWSClient client = EWSClient.GetEWSClient(mailboxUri, credentials))
            {
                // Compose the out‑of‑office (OOF) reply message
                MailMessage oofMessage = new MailMessage();
                oofMessage.From = "user@example.com";
                oofMessage.To = "sender@example.com";
                oofMessage.Subject = "Out of Office";

                // HTML formatted body with a link to the intranet
                oofMessage.HtmlBody = @"<html>
                    <body>
                        <p>Thank you for your email.</p>
                        <p>I am currently out of the office and will return on <b>June 1, 2026</b>.</p>
                        <p>For urgent matters, please visit the <a href=""https://intranet.example.com"">company intranet</a>.</p>
                        <p>Best regards,<br/>User Name</p>
                    </body>
                </html>";

                // Send the OOF reply (this example sends the message; actual OOF setup may require server‑side configuration)
                client.Send(oofMessage);
                Console.WriteLine("Out of office message sent successfully.");
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
