using System;
using Aspose.Email;
using Aspose.Email.Clients.Exchange.WebService;

class Program
{
    static void Main(string[] args)
    {
        try
        {
            // Initialize the EWS client (replace with real server URL and credentials)
            using (IEWSClient client = EWSClient.GetEWSClient("https://exchange.example.com/EWS/Exchange.asmx", "username", "password"))
            {
                try
                {
                    // Get the URI of the parent folder (Inbox in this example)
                    string parentFolderUri = client.MailboxInfo.InboxUri;

                    // Create a new folder under the parent folder
                    client.CreateFolder(parentFolderUri, "NewFolder");

                    Console.WriteLine("Folder 'NewFolder' created successfully.");
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine("Error while creating folder: " + ex.Message);
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine("Unexpected error: " + ex.Message);
        }
    }
}
