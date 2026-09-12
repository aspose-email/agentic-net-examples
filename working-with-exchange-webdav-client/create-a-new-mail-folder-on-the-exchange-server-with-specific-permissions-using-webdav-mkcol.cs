using Aspose.Email;
using System;
using System.Net;
using Aspose.Email.Clients.Exchange.Dav;

class Program
{
    static void Main()
    {
        try
        {
            // Exchange server URL (WebDAV endpoint)
            string exchangeUrl = "https://exchange.example.com/ews/exchangeusers/username";
            // Credentials for the mailbox
            string username = "user@example.com";
            string password = "password";

            // Parent folder URI (e.g., Inbox) under which the new folder will be created
            string parentFolderUri = exchangeUrl + "/Inbox";
            // Name of the new folder to create
            string newFolderName = "MyNewFolder";


            // Skip external calls when placeholder credentials are used
            if (exchangeUrl.Contains("example.com") || username.Contains("example.com") || password == "password")
            {
                Console.Error.WriteLine("Placeholder credentials detected. Skipping external calls.");
                return;
            }

            NetworkCredential credentials = new NetworkCredential(username, password);

            // ExchangeClient uses WebDAV MKCOL to create folders
            using (ExchangeClient client = new ExchangeClient(exchangeUrl, credentials))
            {
                client.CreateFolder(parentFolderUri, newFolderName);
                Console.WriteLine($"Folder '{newFolderName}' created successfully under '{parentFolderUri}'.");
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
