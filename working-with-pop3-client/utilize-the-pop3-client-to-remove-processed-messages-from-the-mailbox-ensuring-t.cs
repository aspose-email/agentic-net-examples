using System;
using Aspose.Email;
using Aspose.Email.Clients;
using Aspose.Email.Clients.Pop3;

namespace AsposeEmailPop3DeleteSample
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                // POP3 server connection settings
                string host = "pop3.example.com";
                int port = 110;
                string username = "user@example.com";
                string password = "password";

                // Initialize and connect the POP3 client
                using (Pop3Client client = new Pop3Client(host, port, username, password, SecurityOptions.Auto))
                {
                    try
                    {
                        // Retrieve the list of messages in the mailbox
                        Pop3MessageInfoCollection messages = client.ListMessages();

                        // Process each message
                        foreach (Pop3MessageInfo messageInfo in messages)
                        {
                            // Fetch the full message
                            MailMessage mailMessage = client.FetchMessage(messageInfo.UniqueId);

                            // Example processing: display subject
                            Console.WriteLine("Subject: " + mailMessage.Subject);

                            // Mark the message for deletion after processing
                            client.DeleteMessage(messageInfo.UniqueId);
                        }

                        // Commit deletions to permanently remove marked messages from the server
                        client.CommitDeletes();
                    }
                    catch (Exception ex)
                    {
                        Console.Error.WriteLine("Error during POP3 operations: " + ex.Message);
                        // Ensure any pending deletions are not committed if an error occurs
                    }
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine("Failed to initialize POP3 client: " + ex.Message);
            }
        }
    }
}
