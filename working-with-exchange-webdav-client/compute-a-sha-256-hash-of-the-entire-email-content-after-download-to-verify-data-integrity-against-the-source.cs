using System;
using System.IO;
using System.Security.Cryptography;
using Aspose.Email;
using Aspose.Email.Clients.Exchange.Dav;
using Aspose.Email.Clients.Exchange;

class Program
{
    static void Main()
    {
        try
        {
            // Placeholder connection details.
            string mailboxUri = "https://exchange.example.com/EWS/Exchange.asmx";
            string username = "user@example.com";
            string password = "password";

            // Skip execution when placeholders are detected.
            if (mailboxUri.Contains("example.com"))
            {
                Console.Error.WriteLine("Placeholder credentials detected. Skipping execution.");
                return;
            }

            // Create and use the Exchange client.
            using (ExchangeClient client = new ExchangeClient(mailboxUri, username, password))
            {
                try
                {
                    // List messages in the Inbox folder.
                    ExchangeMessageInfoCollection messages = client.ListMessages(client.MailboxInfo.InboxUri);

                    // Prepare SHA256 algorithm (can be reused for all messages).
                    using (SHA256 sha256 = SHA256.Create())
                    {
                        foreach (ExchangeMessageInfo messageInfo in messages)
                        {
                            // Fetch the full message.
                            using (MailMessage message = client.FetchMessage(messageInfo.UniqueUri))
                            {
                                // Save the message to a memory stream.
                                using (MemoryStream ms = new MemoryStream())
                                {
                                    message.Save(ms);
                                    byte[] rawData = ms.ToArray();

                                    // Compute the SHA‑256 hash.
                                    byte[] hashBytes = sha256.ComputeHash(rawData);
                                    string hashString = BitConverter.ToString(hashBytes).Replace("-", string.Empty);

                                    Console.WriteLine($"Message URI: {messageInfo.UniqueUri}");
                                    Console.WriteLine($"SHA‑256: {hashString}");
                                    Console.WriteLine();
                                }
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Error during Exchange operations: {ex.Message}");
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unhandled exception: {ex.Message}");
        }
    }
}
