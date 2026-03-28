using System;
using System.Threading.Tasks;
using Aspose.Email.Clients.Imap;
using Aspose.Email.Clients;

class Program
{
    static async Task Main(string[] args)
    {
        try
        {
            // Initialize IMAP client with placeholder credentials
            using (ImapClient client = new ImapClient("imap.example.com", 993, "user@example.com", "password", SecurityOptions.Auto))
            {
                try
                {
                    // Connect to the IMAP server

                    // Unique identifier of the message to delete (replace with actual UID)
                    string messageUid = "12345";

                    // Permanently delete the message and commit the deletion immediately
                    await client.DeleteMessageAsync(messageUid, true);
                    Console.WriteLine("Message permanently deleted.");
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"IMAP operation failed: {ex.Message}");
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
