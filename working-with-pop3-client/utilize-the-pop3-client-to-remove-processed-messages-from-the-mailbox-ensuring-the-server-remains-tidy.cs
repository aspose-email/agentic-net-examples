using System;
using Aspose.Email.Clients;
using Aspose.Email.Clients.Pop3;

namespace AsposeEmailPop3DeleteExample
{
    class Program
    {
        static void Main()
        {
            try
            {
                // POP3 server credentials
                string host = "pop.example.com";
                string username = "user@example.com";
                string password = "password";

                // Create and dispose the POP3 client
                using (Pop3Client client = new Pop3Client(host, username, password))
                {
                    try
                    {
                        // Retrieve all messages from the mailbox
                        Pop3MessageInfoCollection messages = client.ListMessages();

                        // Process each message and mark it for deletion
                        foreach (Pop3MessageInfo info in messages)
                        {
                            // Example processing: output the subject
                            Console.WriteLine($"Processing message: {info.Subject}");

                            // Delete the message after processing
                            client.DeleteMessage(info.SequenceNumber);
                        }

                        // Commit the deletions so the server removes the marked messages
                        client.CommitDeletes();
                    }
                    catch (Exception ex)
                    {
                        // Handle errors that occur during POP3 operations
                        Console.Error.WriteLine($"POP3 operation error: {ex.Message}");
                    }
                }
            }
            catch (Exception ex)
            {
                // Handle unexpected errors (e.g., client creation failures)
                Console.Error.WriteLine($"Unexpected error: {ex.Message}");
            }
        }
    }
}
