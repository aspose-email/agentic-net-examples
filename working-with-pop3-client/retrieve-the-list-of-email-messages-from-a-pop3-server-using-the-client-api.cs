using System;
using Aspose.Email;
using Aspose.Email.Clients.Pop3;

namespace Pop3MessageListSample
{
    class Program
    {
        static void Main()
        {
            try
            {
                // Placeholder POP3 server credentials
                string host = "pop.example.com";
                int port = 110;
                string username = "username";
                string password = "password";

                // Skip real network call when placeholders are detected
                if (host.Contains("example.com") || username == "username" || password == "password")
                {
                    Console.Error.WriteLine("Placeholder POP3 credentials detected. Skipping server connection.");
                    return;
                }

                // Initialize POP3 client
                using (Pop3Client client = new Pop3Client(host, port, username, password))
                {
                    try
                    {
                        // Retrieve list of messages
                        Pop3MessageInfoCollection messages = client.ListMessages();

                        // Iterate through messages and display basic info
                        foreach (Pop3MessageInfo messageInfo in messages)
                        {
                            Console.WriteLine($"Subject: {messageInfo.Subject}");
                            Console.WriteLine($"From: {messageInfo.From}");
                            Console.WriteLine($"Date: {messageInfo.Date}");
                            Console.WriteLine();
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.Error.WriteLine($"Error retrieving messages: {ex.Message}");
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
