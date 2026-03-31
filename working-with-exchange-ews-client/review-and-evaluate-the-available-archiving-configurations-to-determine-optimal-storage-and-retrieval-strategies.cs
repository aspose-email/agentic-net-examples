using Aspose.Email.Storage.Pst;
using Aspose.Email.Clients.Exchange;
using System;
using Aspose.Email;
using Aspose.Email.Clients.Exchange.WebService;

class Program
{
    static void Main(string[] args)
    {
        try
        {
            // Placeholder connection parameters
            string mailboxUri = "https://exchange.example.com/EWS/Exchange.asmx";
            string username = "username";
            string password = "password";

            // Skip real network calls when placeholders are used
            if (mailboxUri.Contains("example.com"))
            {
                Console.WriteLine("Placeholder credentials detected. Skipping actual Exchange connection.");
                return;
            }

            // Create the EWS client
            using (IEWSClient client = EWSClient.GetEWSClient(mailboxUri, username, password))
            {
                // Retrieve basic mailbox information
                ExchangeMailboxInfo mailboxInfo = client.GetMailboxInfo();
                Console.WriteLine($"Mailbox URI: {mailboxInfo.MailboxUri}");

                // Get total mailbox size
                long mailboxSize = client.GetMailboxSize();
                Console.WriteLine($"Mailbox size: {mailboxSize} bytes");

                // Attempt to retrieve the Archive folder information
                try
                {
                    // The Archive folder is typically a top‑level folder named "Archive"
                    ExchangeFolderInfo archiveFolder = client.GetFolderInfo("Archive");
                    Console.WriteLine($"Archive folder found: {archiveFolder.DisplayName}");
                    Console.WriteLine($"Archive folder URI: {archiveFolder.Uri}");
                }
                catch (Exception)
                {
                    Console.WriteLine("Archive folder not found or cannot be accessed.");
                }

                // List sub‑folders under the Archive folder (if it exists)
                try
                {
                    ExchangeFolderInfoCollection subFolders = client.ListSubFolders("Archive");
                    Console.WriteLine("Sub‑folders within Archive:");
                    foreach (ExchangeFolderInfo subFolder in subFolders)
                    {
                        Console.WriteLine($"- {subFolder.DisplayName}");
                    }
                }
                catch (Exception)
                {
                    // If the Archive folder does not exist, this block will be reached
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
