using Aspose.Email.Storage.Pst;
using Aspose.Email;
using System;
using Aspose.Email.Clients.Exchange.Dav;
using Aspose.Email.Clients.Exchange;

class Program
{
    static void Main()
    {
        try
        {
            string mailboxUri = "https://exchange.example.com/ews/exchange.asmx";
            string username = "user@example.com";
            string password = "password";
            string folderUri = "Inbox";

            // Skip execution when placeholder credentials are used
            if (mailboxUri.Contains("example.com"))
            {
                Console.Error.WriteLine("Placeholder credentials detected. Skipping network call.");
                return;
            }

            using (ExchangeClient client = new ExchangeClient(mailboxUri, username, password))
            {
                try
                {
                    ExchangeFolderInfo folderInfo = client.GetFolderInfo(folderUri);
                    Console.WriteLine($"Folder: {folderInfo.DisplayName}");
                    Console.WriteLine($"Total items: {folderInfo.TotalCount}");
                    Console.WriteLine($"Size (bytes): {folderInfo.Size}");
                    Console.WriteLine($"Unread items: {folderInfo.UnreadCount}");
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Error retrieving folder info: {ex.Message}");
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
