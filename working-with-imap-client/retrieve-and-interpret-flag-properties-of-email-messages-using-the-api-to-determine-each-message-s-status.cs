using Aspose.Email.Clients;
using System;
using Aspose.Email.Clients.Imap;
using Aspose.Email;
using Aspose.Email.Tools.Search;

namespace RetrieveMessageFlags
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                // Placeholder connection settings
                string host = "imap.example.com";
                int port = 993;
                string username = "user@example.com";
                string password = "password";

                // Detect placeholder credentials and skip real network call
                if (host.Contains("example.com") || username.Contains("example.com") || password == "password")
                {
                    Console.WriteLine("Placeholder credentials detected. Skipping connection to the mail server.");
                    return;
                }

                // Create and connect the IMAP client
                using (ImapClient client = new ImapClient(host, port, username, password, SecurityOptions.Auto))
                {
                    try
                    {
                        // Select the INBOX folder
                        client.SelectFolder("INBOX");

                        // Retrieve message infos from the selected folder
                        ImapMessageInfo[] messages = client.ListMessages();

                        foreach (ImapMessageInfo messageInfo in messages)
                        {
                            // Interpret flag properties
                            Console.WriteLine("Message UID: " + messageInfo.UniqueId);
                            Console.WriteLine("Subject: " + messageInfo.Subject);
                            Console.WriteLine("Is Read: " + messageInfo.IsRead);
                            Console.WriteLine("Answered: " + messageInfo.Answered);
                            Console.WriteLine("Deleted: " + messageInfo.Deleted);
                            Console.WriteLine("Draft: " + messageInfo.Draft);
                            Console.WriteLine("Flagged: " + messageInfo.Flagged);
                            Console.WriteLine("Recent: " + messageInfo.Recent);
                            Console.WriteLine(new string('-', 40));
                        }
                    }
                    catch (Exception ex)
                    {
                        // Handle connection or operation errors gracefully
                        Console.Error.WriteLine("Error during IMAP operations: " + ex.Message);
                        return;
                    }
                }
            }
            catch (Exception ex)
            {
                // Top-level exception guard
                Console.Error.WriteLine("Unexpected error: " + ex.Message);
                return;
            }
        }
    }
}
