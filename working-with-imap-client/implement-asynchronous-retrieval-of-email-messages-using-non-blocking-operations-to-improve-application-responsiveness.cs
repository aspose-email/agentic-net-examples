using System;
using System.Threading;
using System.Threading.Tasks;
using Aspose.Email;
using Aspose.Email.Clients.Imap;

namespace AsposeEmailImapAsyncSample
{
    class Program
    {
        static async Task Main(string[] args)
        {
            try
            {
                // IMAP server connection settings (replace with real values)
                string host = "imap.example.com";
                string username = "user@example.com";
                string password = "password";

                // Create and connect the IMAP client inside a using block to ensure disposal
                using (ImapClient client = new ImapClient(host, username, password))
                {
                    try
                    {
                        // Asynchronously retrieve the list of message infos from the default folder
                        ImapMessageInfoCollection messageInfos = await client.ListMessagesAsync(CancellationToken.None);

                        foreach (Aspose.Email.Clients.Imap.ImapMessageInfo info in messageInfos)
                        {
                            // Asynchronously fetch the full message using its unique identifier
                            using (MailMessage message = await client.FetchMessageAsync(info.UniqueId, CancellationToken.None))
                            {
                                Console.WriteLine($"Subject: {message.Subject}");
                                Console.WriteLine($"From: {message.From}");
                                Console.WriteLine($"Date: {message.Date}");
                                Console.WriteLine(new string('-', 40));
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        // Handle errors related to IMAP operations
                        Console.Error.WriteLine($"IMAP operation failed: {ex.Message}");
                        return;
                    }
                }
            }
            catch (Exception ex)
            {
                // Handle errors related to client creation or other unexpected issues
                Console.Error.WriteLine($"Unexpected error: {ex.Message}");
                return;
            }
        }
    }
}
