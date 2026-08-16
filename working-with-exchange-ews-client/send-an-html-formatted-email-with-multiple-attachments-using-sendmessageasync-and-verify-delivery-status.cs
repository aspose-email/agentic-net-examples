using System;
using System.IO;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Aspose.Email;
using Aspose.Email.Clients.Exchange.WebService;

class Program
{
    static async Task Main(string[] args)
    {
        try
        {
            // Mailbox URI and credentials (replace with real values)
            string mailboxUri = "https://mail.example.com/EWS/Exchange.asmx";

            // Skip external calls when placeholder credentials are used
            if (mailboxUri.Contains("example.com"))
            {
                Console.Error.WriteLine("Placeholder credentials detected. Skipping external calls.");
                return;
            }

            NetworkCredential credentials = new NetworkCredential("username", "password");

            // Initialize async EWS client
            using (IAsyncEwsClient client = await EWSClient.GetEwsClientAsync(mailboxUri, credentials))
            {
                // Build the email message
                MailMessage message = new MailMessage();
                message.From = "sender@example.com";
                message.To.Add("recipient@example.com");
                message.Subject = "Test HTML Email with Attachments";
                message.IsBodyHtml = true;
                message.HtmlBody = "<h1>Hello</h1><p>This is an HTML email with attachments.</p>";

                // Prepare first attachment (in-memory)
                byte[] data1 = Encoding.UTF8.GetBytes("Attachment content 1");
                MemoryStream stream1 = new MemoryStream(data1);
                Attachment attachment1 = new Attachment(stream1, "file1.txt");
                message.Attachments.Add(attachment1);

                // Prepare second attachment (in-memory)
                byte[] data2 = Encoding.UTF8.GetBytes("Attachment content 2");
                MemoryStream stream2 = new MemoryStream(data2);
                Attachment attachment2 = new Attachment(stream2, "file2.txt");
                message.Attachments.Add(attachment2);

                // Send the message asynchronously
                await client.SendAsync(message);

                // Clean up attachment resources
                attachment1.Dispose();
                stream1.Dispose();
                attachment2.Dispose();
                stream2.Dispose();

                Console.WriteLine("Email sent successfully.");
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
