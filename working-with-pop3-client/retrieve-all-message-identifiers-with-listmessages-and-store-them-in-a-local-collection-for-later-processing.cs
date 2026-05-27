using Aspose.Email;
using System;
using System.Collections.Generic;
using Aspose.Email.Clients;
using Aspose.Email.Clients.Pop3;

class Program
{
    static void Main(string[] args)
    {
        try
        {
            // Guard against placeholder credentials to avoid real network calls during CI
            const string host = "pop3.example.com";
            const int port = 110;
            const string username = "username";
            const string password = "password";

            if (host.Contains("example.com") || username == "username")
            {
                Console.WriteLine("Placeholder credentials detected. Skipping POP3 operations.");
                return;
            }

            // Create POP3 client with correct constructor overload (host, port, username, password, security)
            Pop3Client client = new Pop3Client(host, port, username, password, SecurityOptions.Auto);
            using (client)
            {
                // Retrieve all messages information
                Pop3MessageInfoCollection messages = client.ListMessages();

                // Store identifiers (UniqueId if available, otherwise SequenceNumber) in a list
                List<string> messageIds = new List<string>();
                foreach (Pop3MessageInfo info in messages)
                {
                    string id = info.UniqueId ?? info.SequenceNumber.ToString();
                    messageIds.Add(id);
                }

                Console.WriteLine($"Retrieved {messageIds.Count} message identifier(s).");
                // messageIds can be used later for further processing
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
