using System;
using System.IO;
using Aspose.Email;
using Aspose.Email.Clients;
using Aspose.Email.Clients.Pop3;

class Program
{
    static void Main()
    {
        try
        {
            // POP3 server configuration
            string host = "pop3.example.com";
            int port = 995;
            string username = "user@example.com";
            string password = "password";
            SecurityOptions security = SecurityOptions.SSLImplicit;

            // Log file configuration
            string logDirectory = Path.Combine(Environment.CurrentDirectory, "Logs");
            string logPath = Path.Combine(logDirectory, "pop3_log.txt");

            // Ensure the log directory exists
            if (!Directory.Exists(logDirectory))
            {
                Directory.CreateDirectory(logDirectory);
            }

            // Initialize POP3 client with logging enabled
            using (Pop3Client client = new Pop3Client(host, port, username, password, security))
            {
                client.EnableLogger = true;
                client.LogFileName = logPath;
                client.UseDateInLogFileName = false;

                // Retrieve the list of messages
                Pop3MessageInfoCollection messages = client.ListMessages();

                Console.WriteLine($"Total messages: {messages.Count}");

                // Fetch and display the first message if available
                if (messages.Count > 0)
                {
                    Pop3MessageInfo firstInfo = messages[0];
                    MailMessage message = client.FetchMessage(firstInfo.UniqueId);
                    Console.WriteLine($"Subject: {message.Subject}");
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
