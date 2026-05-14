using System;
using Aspose.Email;
using Aspose.Email.Clients;
using Aspose.Email.Clients.Pop3;

class Program
{
    static void Main()
    {
        try
        {
            // Placeholder connection settings – real values should be provided by the user.
            string host = "pop3.example.com";
            int port = 110;
            string username = "username";
            string password = "password";

            // Skip execution when placeholder credentials are detected to avoid unwanted network calls.
            if (host.Contains("example.com") || username == "username" || password == "password")
            {
                Console.Error.WriteLine("Skipping POP3 operations because placeholder credentials are used.");
                return;
            }

            // Create and use the POP3 client inside a using block to ensure proper disposal.
            using (Pop3Client client = new Pop3Client(host, port, username, password, SecurityOptions.Auto))
            {
                try
                {
                    // Retrieve the list of messages from the server.
                    Pop3MessageInfoCollection messages = client.ListMessages();

                    // Iterate over each message and delete those that match the filter criteria.
                    for (int i = 0; i < messages.Count; i++)
                    {
                        Pop3MessageInfo info = messages[i];

                        // Example filter: delete messages whose subject contains the word "Spam".
                        if (!string.IsNullOrEmpty(info.Subject) && info.Subject.IndexOf("Spam", StringComparison.OrdinalIgnoreCase) >= 0)
                        {
                            // Delete the message by its sequence number.
                            client.DeleteMessage(info.SequenceNumber);
                            Console.WriteLine($"Deleted message #{info.SequenceNumber}: {info.Subject}");
                        }
                    }

                    // Commit the deletions so the server finalizes the removal.
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"POP3 operation failed: {ex.Message}");
                    // The client will be disposed automatically by the using statement.
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
