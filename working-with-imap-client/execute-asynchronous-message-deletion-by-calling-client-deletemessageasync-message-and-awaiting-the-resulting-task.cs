using Aspose.Email.Clients;
using System;
using System.Threading.Tasks;
using Aspose.Email;
using Aspose.Email.Clients.Imap;
using Aspose.Email.Tools.Search;

namespace Sample
{
    class Program
    {
        static async Task Main(string[] args)
        {
            try
            {
                // Initialize the IMAP client with server details.
                using (ImapClient client = new ImapClient("imap.example.com", 993, "username", "password", SecurityOptions.SSLImplicit))
                {
                    // Select the INBOX folder.
                    await client.SelectFolderAsync("INBOX");

                    // Retrieve the list of messages in the selected folder.
                    ImapMessageInfoCollection messageInfos = await client.ListMessagesAsync();

                    if (messageInfos.Count > 0)
                    {
                        // Get the unique identifier of the first message.
                        string uid = messageInfos[0].UniqueId;

                        // Asynchronously delete the message and commit the deletion.
                        await client.DeleteMessageAsync(uid, true);

                        Console.WriteLine($"Message with UID {uid} has been deleted.");
                    }
                    else
                    {
                        Console.WriteLine("No messages found to delete.");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Error: {ex.Message}");
            }
        }
    }
}
