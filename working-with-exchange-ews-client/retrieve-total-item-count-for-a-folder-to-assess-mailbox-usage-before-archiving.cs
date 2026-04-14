using Aspose.Email.Storage.Pst;
using Aspose.Email.Clients.Exchange;
using Aspose.Email;
using System;
using Aspose.Email.Clients.Exchange.WebService;

class Program
{
    static void Main()
    {
        try
        {
            // Define connection parameters
            string mailboxUri = "https://exchange.example.com/EWS/Exchange.asmx";
            string username = "user@example.com";
            string password = "password";


            // Skip external calls when placeholder credentials are used
            if (mailboxUri.Contains("example.com") || username.Contains("example.com") || password == "password")
            {
                Console.Error.WriteLine("Placeholder credentials detected. Skipping external calls.");
                return;
            }

            // Create and use the EWS client
            using (IEWSClient client = EWSClient.GetEWSClient(mailboxUri, username, password))
            {
                // Retrieve folder information (e.g., Inbox)
                ExchangeFolderInfo folderInfo = client.GetFolderInfo("Inbox");

                // Get total item count in the folder
                int totalCount = folderInfo.TotalCount;

                Console.WriteLine($"Total items in Inbox: {totalCount}");
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
