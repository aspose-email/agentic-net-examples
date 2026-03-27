using System;
using Aspose.Email;
using Aspose.Email.Clients;
using Aspose.Email.Clients.Imap;

namespace ImapLoggingExample
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                // Initialize the IMAP client with server details and credentials
                using (Aspose.Email.Clients.Imap.ImapClient imapClient = new Aspose.Email.Clients.Imap.ImapClient(
                    "imap.example.com",
                    993,
                    "username",
                    "password",
                    Aspose.Email.Clients.SecurityOptions.SSLImplicit))
                {
                    try
                    {
                        // Enable protocol logging and specify the log file name
                        imapClient.EnableLogger = true;
                        imapClient.LogFileName = "imap_log.txt";

                        // Perform an operation that triggers the connection (e.g., select a folder)
                        imapClient.SelectFolder("INBOX");

                        Console.WriteLine("IMAP client connected successfully. Logging is enabled.");
                    }
                    catch (Exception ex)
                    {
                        Console.Error.WriteLine($"IMAP operation failed: {ex.Message}");
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