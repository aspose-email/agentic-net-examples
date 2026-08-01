using System;
using Aspose.Email;
using Aspose.Email.Clients.Pop3;

// Author: Example code demonstrating POP3 message deletion using Aspose.Email

class Program
{
    static void Main()
    {
        try
        {
            // POP3 server connection parameters
            string host = "pop3.example.com";
            int port = 110;
            string username = "user@example.com";
            string password = "password";

            // Create and use the POP3 client (client will be disposed automatically)
            using (Pop3Client pop3Client = new Pop3Client(host, port, username, password))
            {
                // Retrieve the list of messages in the mailbox
                Pop3MessageInfoCollection messageInfos = pop3Client.ListMessages();

                // Define the criteria for the message to delete (e.g., subject)
                string targetSubject = "Target Email Subject";


                // Skip external calls when placeholder credentials are used
                if (host.Contains("example.com") || username.Contains("example.com") || password == "password")
                {
                    Console.Error.WriteLine("Placeholder credentials detected. Skipping external calls.");
                    return;
                }

                // Locate the unique identifier of the target message
                string targetUniqueId = null;
                foreach (Pop3MessageInfo info in messageInfos)
                {
                    if (info.Subject != null && info.Subject.Equals(targetSubject, StringComparison.OrdinalIgnoreCase))
                    {
                        targetUniqueId = info.UniqueId;
                        break;
                    }
                }

                if (targetUniqueId != null)
                {
                    // Mark the message for deletion; actual removal occurs when the session ends (QUIT)
                    pop3Client.DeleteMessage(targetUniqueId);
                    Console.WriteLine("Message marked for deletion.");
                }
                else
                {
                    Console.WriteLine("Target message not found.");
                }
                // Exiting the using block sends QUIT, permanently deleting marked messages
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
