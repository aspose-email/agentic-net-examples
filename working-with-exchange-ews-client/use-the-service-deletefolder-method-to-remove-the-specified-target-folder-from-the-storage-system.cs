using System;
using Aspose.Email;
using Aspose.Email.Clients.Exchange.WebService;

class Program
{
    static void Main()
    {
        try
        {
            // Initialize the EWS client with mailbox URI and credentials.
            string mailboxUri = "https://exchange.example.com/EWS/Exchange.asmx";
            string username = "user@example.com";
            string password = "password";

            using (IEWSClient client = EWSClient.GetEWSClient(mailboxUri, username, password))
            {
                // Construct the target folder URI. Adjust the path as needed.
                string targetFolderUri = client.MailboxInfo.InboxUri + "/TargetFolder";

                // Delete the specified folder.
                client.DeleteFolder(targetFolderUri);

                Console.WriteLine("Folder deleted successfully.");
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine(ex.Message);
        }
    }
}
