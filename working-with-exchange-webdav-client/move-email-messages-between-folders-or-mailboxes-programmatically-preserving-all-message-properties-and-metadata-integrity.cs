using Aspose.Email.Storage.Pst;
using Aspose.Email.Clients.Exchange;
using System;
using Aspose.Email;
using Aspose.Email.Clients.Exchange.Dav;

namespace AsposeEmailExample
{
    class Program
    {
        static void Main()
        {
            try
            {
                // Placeholder credentials – in real scenarios replace with actual values.
                string mailboxUri = "https://exchange.example.com/ews/exchange.asmx";
                string username = "username";
                string password = "password";

                // Guard against executing with placeholder credentials.
                if (mailboxUri.Contains("example.com") || username.Equals("username", StringComparison.OrdinalIgnoreCase))
                {
                    Console.WriteLine("Placeholder credentials detected. Skipping operation.");
                    return;
                }

                // Initialize the Exchange WebDAV client.
                using (ExchangeClient client = new ExchangeClient(mailboxUri, username, password))
                {
                    // Define source and destination folder names.
                    string sourceFolderName = "Inbox";
                    string destinationFolderName = "Processed";

                    // Retrieve folder information to obtain their URIs.
                    ExchangeFolderInfo sourceFolderInfo = client.GetFolderInfo(sourceFolderName);
                    ExchangeFolderInfo destinationFolderInfo = client.GetFolderInfo(destinationFolderName);

                    if (sourceFolderInfo == null || destinationFolderInfo == null)
                    {
                        Console.Error.WriteLine("Unable to locate one or both folders.");
                        return;
                    }

                    string sourceFolderUri = sourceFolderInfo.Uri;
                    string destinationFolderUri = destinationFolderInfo.Uri;

                    // List messages in the source folder.
                    var messages = client.ListMessages(sourceFolderUri);

                    foreach (ExchangeMessageInfo messageInfo in messages)
                    {
                        // Move each message to the destination folder.
                        client.MoveMessage(messageInfo, destinationFolderUri);
                        Console.WriteLine($"Moved message: Subject = \"{messageInfo.Subject}\"");
                    }

                    Console.WriteLine("Message move operation completed.");
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Error: {ex.Message}");
            }
        }
    }
}
