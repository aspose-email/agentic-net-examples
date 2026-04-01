using System;
using System.Collections.Generic;
using Aspose.Email;
using Aspose.Email.Clients.Pop3;

class Program
{
    static void Main()
    {
        try
        {
            // Placeholder connection settings
            string host = "pop3.example.com";
            int port = 110;
            string username = "user@example.com";
            string password = "password";

            // Skip actual network call when placeholders are used
            if (host.Contains("example.com"))
            {
                Console.Error.WriteLine("Placeholder POP3 server detected. Skipping connection.");
                return;
            }

            // Subject filter criteria
            string subjectFilter = "Important Report";

            // Connect to POP3 server
            using (Pop3Client client = new Pop3Client(host, port, username, password))
            {
                try
                {
                    // Retrieve list of message infos
                    Pop3MessageInfoCollection messagesInfo = client.ListMessages();

                    List<MailMessage> matchingMessages = new List<MailMessage>();

                    // Iterate through each message info and fetch the full message
                    foreach (Pop3MessageInfo info in messagesInfo)
                    {
                        MailMessage message = client.FetchMessage(info.UniqueId);
                        if (message != null && message.Subject != null && message.Subject.Contains(subjectFilter))
                        {
                            matchingMessages.Add(message);
                            Console.WriteLine($"Found matching message: Subject=\"{message.Subject}\"");
                        }
                    }

                    Console.WriteLine($"Total messages matching \"{subjectFilter}\": {matchingMessages.Count}");
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Error during POP3 operations: {ex.Message}");
                    return;
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unhandled exception: {ex.Message}");
        }
    }
}
