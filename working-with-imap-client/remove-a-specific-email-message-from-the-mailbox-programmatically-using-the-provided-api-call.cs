using Aspose.Email;
using System;
using Aspose.Email.Clients;
using Aspose.Email.Clients.Imap;

namespace AsposeEmailImapDeleteExample
{
    // Author: Aspose.Email example for deleting a message via IMAP.
    class Program
    {
        static void Main()
        {
            // Placeholder credentials – replace with real values.
            string host = "your.imap.host.com";
            string username = "your_username";
            string password = "your_password";
            string messageUid = "your_message_uid";

            // Guard against executing network calls with placeholder data.
            if (host.Contains("your") || username.Contains("your") || password.Contains("your") || messageUid.Contains("your"))
            {
                Console.Error.WriteLine("Placeholder credentials or UID detected. Skipping IMAP operation.");
                return;
            }

            // Create and use the IMAP client.
            try
            {
                using (ImapClient client = new ImapClient(host, username, password, SecurityOptions.Auto))
                {
                    // Delete the message identified by its UID.
                    client.DeleteMessage(messageUid);
                    Console.WriteLine($"Message with UID '{messageUid}' has been marked for deletion.");
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"An error occurred while deleting the message: {ex.Message}");
            }
        }
    }
}
