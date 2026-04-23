using Aspose.Email.Clients;
using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Aspose.Email;
using Aspose.Email.Clients.Imap;

class Program
{
    static async Task Main(string[] args)
    {
        try
        {
            // Placeholder credentials – replace with real values.
            string host = "imap.example.com";
            string username = "user@example.com";
            string password = "password";

            // Skip network calls when placeholders are detected.
            if (host.Contains("example") || username.Contains("example") || password == "password")
            {
                Console.Error.WriteLine("Placeholder credentials detected. Skipping IMAP operations.");
                return;
            }

            // Ensure output directory exists.
            string outputDir = "Output";
            try
            {
                if (!Directory.Exists(outputDir))
                {
                    Directory.CreateDirectory(outputDir);
                }
            }
            catch (Exception ioEx)
            {
                Console.Error.WriteLine($"Failed to prepare output directory: {ioEx.Message}");
                return;
            }

            // Create and use the IMAP client.
            using (ImapClient client = new ImapClient(host, username, password, SecurityOptions.Auto))
            {
                try
                {
                    // Select the INBOX folder.
                    await client.SelectFolderAsync("INBOX");

                    // List messages in the folder.
                    ImapMessageInfoCollection messages = await client.ListMessagesAsync("INBOX");
                    if (messages == null || messages.Count == 0)
                    {
                        Console.Error.WriteLine("No messages found in INBOX.");
                        return;
                    }

                    // Fetch the first message asynchronously.
                    ImapMessageInfo firstInfo = messages[0];
                    MailMessage mailMessage = await client.FetchMessageAsync(firstInfo.UniqueId);
                    if (mailMessage == null)
                    {
                        Console.Error.WriteLine("Failed to fetch the message.");
                        return;
                    }

                    // Extract plain‑text and HTML bodies.
                    string plainTextBody = mailMessage.Body ?? string.Empty;
                    string htmlBody = mailMessage.HtmlBody ?? string.Empty;

                    // Write bodies to separate files.
                    string textPath = Path.Combine(outputDir, "MessageBody.txt");
                    string htmlPath = Path.Combine(outputDir, "MessageBody.html");

                    try
                    {
                        File.WriteAllText(textPath, plainTextBody);
                        File.WriteAllText(htmlPath, htmlBody);
                        Console.WriteLine($"Plain‑text saved to: {textPath}");
                        Console.WriteLine($"HTML saved to: {htmlPath}");
                    }
                    catch (Exception writeEx)
                    {
                        Console.Error.WriteLine($"Error writing output files: {writeEx.Message}");
                    }
                }
                catch (Exception clientEx)
                {
                    Console.Error.WriteLine($"IMAP client error: {clientEx.Message}");
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
