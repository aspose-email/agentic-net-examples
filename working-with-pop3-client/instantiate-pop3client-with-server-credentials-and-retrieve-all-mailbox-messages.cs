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
            // Placeholder credentials – skip actual network call if they are not replaced.
            string host = "pop3.example.com";
            int port = 110;
            string username = "username";
            string password = "password";

            if (host.Contains("example.com") || username == "username" || password == "password")
            {
                Console.WriteLine("Placeholder POP3 credentials detected. Skipping POP3 operations.");
                return;
            }

            // Create and connect the POP3 client.
            using (Pop3Client client = new Pop3Client(host, port, username, password, SecurityOptions.Auto))
            {
                try
                {
                    // Validate credentials before proceeding.
                    client.ValidateCredentials();
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to validate POP3 credentials: {ex.Message}");
                    return;
                }

                // Retrieve the list of messages.
                Pop3MessageInfoCollection messages;
                try
                {
                    messages = client.ListMessages();
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Error listing POP3 messages: {ex.Message}");
                    return;
                }

                // Iterate through each message info and display basic details.
                foreach (Pop3MessageInfo info in messages)
                {
                    Console.WriteLine($"Subject: {info.Subject}");
                    Console.WriteLine($"From: {info.From}");
                    Console.WriteLine($"Date: {info.Date}");
                    Console.WriteLine(new string('-', 40));
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
