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
            // Placeholder connection settings – replace with real values.
            string host = "pop3.example.com";
            int port = 110;
            string username = "username";
            string password = "password";
            string uniqueId = "12345";

            // Skip execution when placeholder credentials are detected.
            if (host.Contains("example.com") || username == "username")
            {
                Console.Error.WriteLine("Placeholder credentials detected. Skipping execution.");
                return;
            }

            // Create and use the POP3 client.
            using (Pop3Client client = new Pop3Client(host, port, username, password))
            {
                try
                {
                    // Validate the connection and credentials.
                    client.ValidateCredentials();

                    // Retrieve a concise summary of the message by its unique identifier.
                    Pop3MessageInfo messageInfo = client.GetMessageInfo(uniqueId);

                    if (messageInfo != null)
                    {
                        Console.WriteLine($"Subject: {messageInfo.Subject}");
                        Console.WriteLine($"From: {messageInfo.From}");
                        Console.WriteLine($"Date: {messageInfo.Date}");
                        Console.WriteLine($"Size: {messageInfo.Size} bytes");
                    }
                    else
                    {
                        Console.WriteLine("Message not found.");
                    }
                }
                catch (Pop3Exception ex)
                {
                    Console.Error.WriteLine($"POP3 error: {ex.Message}");
                    return;
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Error: {ex.Message}");
                    return;
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unhandled exception: {ex.Message}");
        }
    }
}
