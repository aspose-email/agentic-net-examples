using System;
using System.IO;
using Aspose.Email;
using Aspose.Email.Clients.Exchange;
using Aspose.Email.Clients.Exchange.Dav;
 // for ExchangeMessageInfoCollection namespace if needed

class Program
{
    static void Main()
    {
        try
        {
            // Placeholder credentials – replace with real values or skip execution in CI.
            string mailboxUri = "https://exchange.example.com/EWS/Exchange.asmx";
            string username = "user@example.com";
            string password = "password";

            if (mailboxUri.Contains("example") || username.Contains("example") || password == "password")
            {
                Console.Error.WriteLine("Placeholder credentials detected. Skipping Exchange operations.");
                return;
            }

            // Create and use the Exchange client.
            using (ExchangeClient client = new ExchangeClient(mailboxUri, username, password))
            {
                try
                {
                    // List messages in the Inbox folder.
                    ExchangeMessageInfoCollection messages = client.ListMessages(client.MailboxInfo.InboxUri);
                    if (messages == null || messages.Count == 0)
                    {
                        Console.WriteLine("No messages found in the inbox.");
                        return;
                    }

                    // Process the first message (or iterate as needed).
                    foreach (ExchangeMessageInfo messageInfo in messages)
                    {
                        // Fetch the full mail message.
                        MailMessage mail = client.FetchMessage(messageInfo.UniqueUri);
                        if (mail == null)
                        {
                            Console.WriteLine($"Failed to fetch message with URI: {messageInfo.UniqueUri}");
                            continue;
                        }

                        // Prepare output file path.
                        string outputPath = Path.Combine(Environment.CurrentDirectory, "email_body.txt");
                        string outputDir = Path.GetDirectoryName(outputPath);
                        if (!Directory.Exists(outputDir))
                        {
                            Directory.CreateDirectory(outputDir);
                        }

                        // Write the plain‑text body to the file.
                        try
                        {
                            using (StreamWriter writer = new StreamWriter(outputPath))
                            {
                                writer.Write(mail.Body);
                            }
                            Console.WriteLine($"Message body saved to: {outputPath}");
                        }
                        catch (Exception ioEx)
                        {
                            Console.Error.WriteLine($"File I/O error: {ioEx.Message}");
                        }

                        // Only process the first message for this example.
                        break;
                    }
                }
                catch (Exception clientEx)
                {
                    Console.Error.WriteLine($"Exchange client error: {clientEx.Message}");
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
