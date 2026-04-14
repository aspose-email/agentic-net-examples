using System;
using Aspose.Email;
using Aspose.Email.Clients.Exchange.WebService;

class Program
{
    static void Main()
    {
        try
        {
            // Define connection and item parameters
            string mailboxUri = "https://exchange.example.com/EWS/Exchange.asmx";
            string username = "user@example.com";
            string password = "password";
            string sourceFolderUri = "inbox";
            string destinationFolderUri = "customfolder";
            string itemUri = "unique-item-id";


            // Skip external calls when placeholder credentials are used
            if (mailboxUri.Contains("example.com") || username.Contains("example.com") || password == "password")
            {
                Console.Error.WriteLine("Placeholder credentials detected. Skipping external calls.");
                return;
            }

            // Create EWS client
            using (IEWSClient client = EWSClient.GetEWSClient(mailboxUri, username, password))
            {
                try
                {
                    // Move the selected email to the custom folder
                    string movedItemUri = client.MoveItem(itemUri, destinationFolderUri);
                    Console.WriteLine($"Message moved to folder '{destinationFolderUri}'. New URI: {movedItemUri}");

                    // Archive the moved email
                    client.ArchiveItem(sourceFolderUri, movedItemUri);
                    Console.WriteLine("Message archived successfully.");
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"EWS operation failed: {ex.Message}");
                    return;
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
