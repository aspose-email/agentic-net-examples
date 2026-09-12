using System;
using Aspose.Email;
using Aspose.Email.Clients;
using Aspose.Email.Clients.Pop3;

// Author: Generated example for POP3 validation processing
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

            // Initialize POP3 client
            using (Pop3Client client = new Pop3Client())
            {
                client.Host = host;
                client.Port = port;
                client.Username = username;
                client.Password = password;
                client.SecurityOptions = SecurityOptions.Auto; // Enable SSL/TLS automatically

                try
                {
                    // Retrieve total number of messages on the server
                    int messageCount = client.GetMessageCount();
                    Console.WriteLine($"Total messages on server: {messageCount}");

                    // Process each message individually
                    for (int index = 1; index <= messageCount; index++)
                    {
                        try
                        {
                            // Fetch the message by its index (1‑based)
                            MailMessage message = client.FetchMessage(index);
                            Console.WriteLine($"Message {index}: Subject = \"{message.Subject}\"");

                            // Example: delete the message after processing (optional)
                            // client.DeleteMessage(index);
                        }
                        catch (Pop3Exception popEx)
                        {
                            // Handle errors specific to fetching a single message
                            Console.Error.WriteLine($"Error fetching message {index}: {popEx.Message}");
                        }
                    }
                }
                catch (Pop3Exception popEx)
                {
                    // Handle errors that occur during overall POP3 operations
                    Console.Error.WriteLine($"POP3 operation failed: {popEx.Message}");
                }
            }
        }
        catch (Exception ex)
        {
            // Catch any unexpected exceptions
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
