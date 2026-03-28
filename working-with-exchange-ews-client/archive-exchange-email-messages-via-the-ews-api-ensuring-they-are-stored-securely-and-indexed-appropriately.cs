using Aspose.Email.Clients.Exchange;
using System;
using System.IO;
using Aspose.Email;
using Aspose.Email.Clients.Exchange.WebService;

class Program
{
    static void Main()
    {
        try
        {
            // Define the archive directory and ensure it exists
            string archiveDir = "Archive";
            if (!Directory.Exists(archiveDir))
            {
                Directory.CreateDirectory(archiveDir);
            }

            // Connect to the Exchange server using EWS
            try
            {
                using (IEWSClient client = EWSClient.GetEWSClient(
                    "https://exchange.example.com/EWS/Exchange.asmx",
                    "username",
                    "password"))
                {
                    // List messages in the Inbox folder
                    ExchangeMessageInfoCollection messageInfos = client.ListMessages(client.MailboxInfo.InboxUri);
                    foreach (ExchangeMessageInfo info in messageInfos)
                    {
                        try
                        {
                            // Fetch the message to read its subject for indexing
                            using (MailMessage message = client.FetchMessage(info.UniqueUri))
                            {
                                Console.WriteLine($"Archiving: {message.Subject}");
                            }

                            // Save the message to the archive directory
                            string filePath = Path.Combine(archiveDir, $"{Guid.NewGuid()}.eml");
                            client.SaveMessage(info.UniqueUri, filePath);
                        }
                        catch (Exception ex)
                        {
                            Console.Error.WriteLine($"Failed to archive message {info.UniqueUri}: {ex.Message}");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Failed to connect to Exchange server: {ex.Message}");
                return;
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
