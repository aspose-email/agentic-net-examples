using System;
using Aspose.Email;
using Aspose.Email.Clients;
using Aspose.Email.Clients.Pop3;

namespace Pop3LogAnalyzer
{
    class Program
    {
        static void Main()
        {
            try
            {
                // POP3 server connection settings
                string host = "pop3.example.com";
                int port = 995;
                string username = "user@example.com";
                string password = "password";


                // Skip external calls when placeholder credentials are used
                if (host.Contains("example.com") || username.Contains("example.com") || password == "password")
                {
                    Console.Error.WriteLine("Placeholder credentials detected. Skipping external calls.");
                    return;
                }

                // Create and configure the POP3 client
                using (Pop3Client client = new Pop3Client())
                {
                    client.Host = host;
                    client.Port = port;
                    client.Username = username;
                    client.Password = password;
                    client.SecurityOptions = SecurityOptions.Auto; // TLS/SSL negotiation

                    try
                    {
                        // Retrieve total number of messages on the server
                        int messageCount = client.GetMessageCount();
                        Console.WriteLine($"Total messages on server: {messageCount}");

                        // Iterate through each message to analyze its headers and content
                        for (int i = 1; i <= messageCount; i++)
                        {
                            // Get basic info for the message (size, UID, etc.)
                            Pop3MessageInfo info = client.GetMessageInfo(i);

                            // Fetch the full message
                            MailMessage message = client.FetchMessage(i);

                            // Output key details for troubleshooting
                            Console.WriteLine($"Message {i}/{messageCount}");
                            Console.WriteLine($"  Subject : {message.Subject}");
                            Console.WriteLine($"  From    : {message.From}");
                            Console.WriteLine($"  Size    : {info.Size} bytes");
                            Console.WriteLine($"  UID     : {info.UniqueId}");
                            Console.WriteLine();
                        }
                    }
                    catch (Pop3Exception popEx)
                    {
                        Console.Error.WriteLine($"POP3 operation failed: {popEx.Message}");
                        return;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Unexpected error: {ex.Message}");
            }
        }
    }
}
