using System;
using System.IO;
using System.Text;
using Aspose.Email;
using Aspose.Email.Clients.Exchange.Dav;

class Program
{
    static void Main()
    {
        try
        {
            // Placeholder connection details – replace with real values when needed.
            string mailboxUri = "https://exchange.example.com/exchange/username";
            string username = "username";
            string password = "password";

            // Skip real network calls when placeholders are detected.
            if (mailboxUri.Contains("example.com"))
            {
                Console.Error.WriteLine("Placeholder credentials detected. Skipping network operation.");
                return;
            }

            using (ExchangeClient client = new ExchangeClient(mailboxUri, username, password))
            {
                // Verify connection by accessing the inbox URI.
                try
                {
                    string inboxUri = client.MailboxInfo.InboxUri;
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to connect to Exchange: {ex.Message}");
                    return;
                }

                // Target folder under the inbox.
                string parentFolderUri = client.MailboxInfo.InboxUri;
                string folderName = "CustomFolder";
                string targetFolder = parentFolderUri + "/" + folderName;

                // Ensure the target folder exists.
                try
                {
                    if (!client.FolderExists(parentFolderUri, folderName, out var _))
                    {
                        client.CreateFolder(parentFolderUri, folderName);
                    }
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Folder check/creation failed: {ex.Message}");
                    return;
                }

                // Build a mail message with an embedded image.
                MailMessage message = new MailMessage();
                message.From = "sender@example.com";
                message.To.Add("recipient@example.com");
                message.Subject = "Email with embedded image";

                string htmlBody = "<html><body><h1>Hello</h1><img src=\"cid:myImage\"/></body></html>";
                AlternateView htmlView = AlternateView.CreateAlternateViewFromString(htmlBody, Encoding.UTF8, "text/html");

                // Minimal PNG image (1x1 pixel) as a byte array.
                byte[] imageBytes = Convert.FromBase64String(
                    "iVBORw0KGgoAAAANSUhEUgAAAAEAAAABCAQAAAC1HAwCAAAAC0lEQVR42mP8Xw8AAusB9Y6Z9WQAAAAASUVORK5CYII=");
                var imageStream = new MemoryStream(imageBytes);
                LinkedResource linkedResource = new LinkedResource(imageStream, "image/png")
                {
                    ContentId = "myImage"
                };
                htmlView.LinkedResources.Add(linkedResource);
                message.AlternateViews.Add(htmlView);

                // Upload the message to the specified folder.
                try
                {
                    string messageUri = client.AppendMessage(targetFolder, message);
                    Console.WriteLine($"Message uploaded successfully. URI: {messageUri}");
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Upload failed: {ex.Message}");
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
