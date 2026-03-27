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
            // POP3 server connection settings
            string host = "pop3.example.com";
            int port = 110;
            string username = "user@example.com";
            string password = "password";

            // Initialize POP3 client
            using (Pop3Client client = new Pop3Client(host, port, username, password))
            {
                try
                {
                    // Retrieve list of messages
                    Pop3MessageInfoCollection messages = client.ListMessages();

                    Console.WriteLine($"Total messages: {messages.Count}");

                    // Display basic information for each message
                    foreach (Pop3MessageInfo info in messages)
                    {
                        Console.WriteLine($"Message ID: {info.UniqueId}, Size: {info.Size} bytes");
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
