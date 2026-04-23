using Aspose.Email;
using System;
using System.Collections.Generic;
using Aspose.Email.Clients.Imap;

class Program
{
    static void Main()
    {
        try
        {
            // Placeholder credentials check – skip execution if they are not replaced.
            string host = "imap.example.com";
            int port = 993;
            string username = "username";
            string password = "password";

            if (host.Contains("example.com") || username == "username")
            {
                Console.Error.WriteLine("Placeholder IMAP credentials detected. Skipping execution.");
                return;
            }

            // Create and connect the IMAP client.
            using (ImapClient client = new ImapClient(host, port, username, password))
            {
                try
                {
                    // Select the folder containing the messages.
                    client.SelectFolder("INBOX");

                    // UIDs of the messages to delete.
                    IEnumerable<string> uidsToDelete = new List<string> { "1001", "1002", "1003" };

                    // Delete the messages and commit the deletions immediately.
                    client.DeleteMessages(uidsToDelete, true);

                    Console.WriteLine("Specified messages have been deleted successfully.");
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"IMAP operation failed: {ex.Message}");
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
