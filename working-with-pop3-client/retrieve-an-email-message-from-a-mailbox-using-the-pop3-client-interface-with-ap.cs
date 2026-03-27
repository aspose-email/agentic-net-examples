using System;
using Aspose.Email;
using Aspose.Email.Clients;
using Aspose.Email.Clients.Pop3;

namespace Pop3RetrieveExample
{
    class Program
    {
        static void Main()
        {
            try
            {
                // POP3 server connection parameters
                string host = "pop.example.com";
                int port = 995;
                string username = "user@example.com";
                string password = "password";

                // Initialize POP3 client with SSL/TLS
                using (Pop3Client client = new Pop3Client(host, port, username, password, SecurityOptions.Auto))
                {
                    try
                    {
                        // Retrieve list of messages
                        Pop3MessageInfoCollection messages = client.ListMessages();

                        if (messages.Count > 0)
                        {
                            // Get information about the first message
                            Pop3MessageInfo firstInfo = messages[0];

                            // Fetch the full message using its UniqueId
                            using (MailMessage mailMessage = client.FetchMessage(firstInfo.UniqueId))
                            {
                                Console.WriteLine("Subject: " + mailMessage.Subject);
                            }
                        }
                        else
                        {
                            Console.WriteLine("No messages found in the mailbox.");
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.Error.WriteLine("Error during POP3 operations: " + ex.Message);
                        return;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine("Unexpected error: " + ex.Message);
            }
        }
    }
}
