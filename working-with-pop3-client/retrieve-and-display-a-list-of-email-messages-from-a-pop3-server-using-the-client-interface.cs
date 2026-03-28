using System;
using Aspose.Email;
using Aspose.Email.Clients;
using Aspose.Email.Clients.Pop3;

namespace Pop3Sample
{
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

                // Initialize and use the POP3 client
                using (Pop3Client client = new Pop3Client(host, port, username, password))
                {
                    // Retrieve the list of message infos
                    Pop3MessageInfoCollection messages = client.ListMessages();

                    // Display basic information for each message
                    foreach (Pop3MessageInfo info in messages)
                    {
                        Console.WriteLine($"Subject: {info.Subject}");
                        Console.WriteLine($"From: {info.From}");
                        Console.WriteLine($"Date: {info.Date}");
                        Console.WriteLine($"Size: {info.Size} bytes");
                        Console.WriteLine(new string('-', 40));
                    }
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Error: {ex.Message}");
            }
        }
    }
}
