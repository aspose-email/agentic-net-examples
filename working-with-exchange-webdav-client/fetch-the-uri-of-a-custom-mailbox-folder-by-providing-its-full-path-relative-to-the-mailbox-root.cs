using Aspose.Email.Storage.Pst;
using Aspose.Email.Clients.Exchange;
using System;
using System.Net;
using Aspose.Email;
using Aspose.Email.Clients.Exchange.Dav;

class Program
{
    static void Main()
    {
        try
        {
            // Exchange server connection details (replace with real values)
            string mailboxUri = "https://exchange.example.com/ews/Exchange.asmx";
            string username = "user@example.com";
            string password = "password";

            // Full folder path relative to the mailbox root (e.g., "Inbox/CustomFolder")
            string folderPath = "Inbox/CustomFolder";

            // Guard against placeholder credentials to avoid unwanted network calls
            if (mailboxUri.Contains("example.com") || username.Contains("example.com"))
            {
                Console.Error.WriteLine("Placeholder credentials detected. Skipping Exchange operation.");
                return;
            }

            // Create and dispose the Exchange client safely
            using (ExchangeClient client = new ExchangeClient(mailboxUri, username, password))
            {
                try
                {
                    // Retrieve folder information using the full path
                    ExchangeFolderInfo folderInfo = client.GetFolderInfo(folderPath);

                    // Output the folder's URI
                    Console.WriteLine("Folder URI: " + folderInfo.Uri);
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine("Error retrieving folder info: " + ex.Message);
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine("Unexpected error: " + ex.Message);
        }
    }
}
