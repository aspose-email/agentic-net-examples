using System;
using System.IO;
using Aspose.Email;
using Aspose.Email.Clients.Exchange.Dav;

class Program
{
    static void Main()
    {
        try
        {
            // Placeholder connection details – replace with real values.
            string mailboxUri = "https://exchange.example.com/ews/Exchange.asmx";
            string username = "user@example.com";
            string password = "password";
            string messageUri = "/mail/inbox/12345";
            string outputDirectory = "Attachments";

            // Detect placeholder credentials and skip execution to avoid external calls.
            if (mailboxUri.Contains("example.com") ||
                username.Contains("example.com") ||
                password == "password")
            {
                Console.Error.WriteLine("Placeholder credentials detected. Skipping execution.");
                return;
            }

            // Ensure the output directory exists before saving any attachments.
            try
            {
                if (!Directory.Exists(outputDirectory))
                {
                    Directory.CreateDirectory(outputDirectory);
                }
            }
            catch (Exception dirEx)
            {
                Console.Error.WriteLine($"Failed to prepare output directory: {dirEx.Message}");
                return;
            }

            // Connect to Exchange server and fetch the message.
            try
            {
                using (ExchangeClient client = new ExchangeClient(mailboxUri, username, password))
                {
                    MailMessage mail;
                    try
                    {
                        mail = client.FetchMessage(messageUri);
                    }
                    catch (Exception fetchEx)
                    {
                        Console.Error.WriteLine($"Failed to fetch message: {fetchEx.Message}");
                        return;
                    }

                    using (mail)
                    {
                        Console.WriteLine($"Subject: {mail.Subject}");
                        Console.WriteLine($"Body: {mail.Body}");

                        // Process each attachment.
                        foreach (Attachment attachment in mail.Attachments)
                        {
                            string attachmentPath = Path.Combine(outputDirectory, attachment.Name ?? "unnamed");

                            try
                            {
                                using (FileStream fileStream = new FileStream(attachmentPath, FileMode.Create, FileAccess.Write))
                                {
                                    if (attachment.ContentStream != null)
                                    {
                                        attachment.ContentStream.CopyTo(fileStream);
                                    }
                                }
                                Console.WriteLine($"Saved attachment: {attachmentPath}");
                            }
                            catch (Exception attEx)
                            {
                                Console.Error.WriteLine($"Failed to save attachment '{attachment.Name}': {attEx.Message}");
                            }
                        }
                    }
                }
            }
            catch (Exception clientEx)
            {
                Console.Error.WriteLine($"Exchange client error: {clientEx.Message}");
                return;
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
