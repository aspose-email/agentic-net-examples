using Aspose.Email.Clients;
using System;
using System.Threading.Tasks;
using Aspose.Email;
using Aspose.Email.Clients.Imap;

class Program
{
    static async Task Main()
    {
        try
        {
            // Placeholder connection details – replace with real values for actual use
            string host = "imap.example.com";
            int port = 993;
            string username = "user@example.com";
            string password = "password";

            // Skip real network calls when placeholder credentials are detected
            if (host.Contains("example.com"))
            {
                Console.WriteLine("Placeholder host detected. Skipping IMAP operations.");
                return;
            }

            // Create and configure the IMAP client
            using (ImapClient client = new ImapClient(host, port, username, password, SecurityOptions.SSLImplicit))
            {
                // Enable detailed logging of IMAP commands and responses
                client.EnableLogger = true;
                client.LogFileName = "imap.log";

                // Validate credentials asynchronously
                bool credentialsValid = await client.ValidateCredentialsAsync();
                Console.WriteLine($"Credentials valid: {credentialsValid}");

                // List all folders asynchronously
                var folderInfoCollection = await client.ListFoldersAsync();
                Console.WriteLine("Folders:");
                foreach (var folderInfo in folderInfoCollection)
                {
                    Console.WriteLine($"- {folderInfo.Name}");
                }

                // Select the INBOX folder
                await client.SelectFolderAsync("INBOX");

                // List messages in the selected folder
                var messageInfoCollection = await client.ListMessagesAsync();
                Console.WriteLine($"Messages in INBOX: {messageInfoCollection.Count}");

                // Fetch the first message if any exist
                if (messageInfoCollection.Count > 0)
                {
                    var firstMessageInfo = messageInfoCollection[0];
                    var mailMessage = await client.FetchMessageAsync(firstMessageInfo.UniqueId);
                    Console.WriteLine($"Subject of first message: {mailMessage.Subject}");
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
