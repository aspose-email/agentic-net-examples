using System;
using Aspose.Email;
using Aspose.Email.Clients;
using Aspose.Email.Clients.Pop3;

class Program
{
    static void Main()
    {
        // Configuration placeholders – replace with real values when running against an actual server
        const string host = "pop3.example.com";
        const int port = 995;
        const string username = "user@example.com";
        const string password = "password";

        // Guard: skip network operations when placeholders are detected
        bool isPlaceholder = host.Contains("example.com") ||
                             username.Contains("example.com") ||
                             password == "password";

        if (isPlaceholder)
        {
            Console.WriteLine("POP3 client configuration contains placeholder values. Skipping network operations.");
            return;
        }

        try
        {
            // Initialize POP3 client with connection settings
            using (Pop3Client pop3Client = new Pop3Client())
            {
                pop3Client.Host = host;
                pop3Client.Port = port;
                pop3Client.Username = username;
                pop3Client.Password = password;
                pop3Client.SecurityOptions = SecurityOptions.Auto;

                // Retrieve the total number of messages in the mailbox
                int messageCount = pop3Client.GetMessageCount();
                Console.WriteLine($"Total messages: {messageCount}");

                // Process each message and delete it after processing
                for (int i = 1; i <= messageCount; i++)
                {
                    // Fetch the message by its index (POP3 indices start at 1)
                    MailMessage message = pop3Client.FetchMessage(i);
                    // Example processing: output the subject
                    Console.WriteLine($"Processing message {i}: {message.Subject}");

                    // Mark the message for deletion
                    pop3Client.DeleteMessage(i);
                }

                // Deletions are committed automatically when the client is disposed (QUIT command)
            }
        }
        catch (Exception ex)
        {
            // Output any errors without crashing the application
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
