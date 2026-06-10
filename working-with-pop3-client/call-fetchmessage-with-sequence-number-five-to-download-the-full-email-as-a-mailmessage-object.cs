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
            // POP3 server connection details (replace with real values)
            string host = "pop3.example.com";
            int port = 110;
            string username = "user@example.com";
            string password = "password";

            // Skip execution when placeholder credentials are detected
            if (host.Contains("example.com") || username.Contains("example.com"))
            {
                Console.Error.WriteLine("Placeholder POP3 credentials detected. Skipping network call.");
                return;
            }

            // Create and use the POP3 client
            using (Pop3Client client = new Pop3Client(host, port, username, password, SecurityOptions.Auto))
            {
                try
                {
                    // Validate credentials (establishes connection)
                    client.ValidateCredentials();

                    // Fetch the message with sequence number 5
                    using (MailMessage message = client.FetchMessage(5))
                    {
                        Console.WriteLine("Subject: " + message.Subject);
                    }
                }
                catch (Pop3Exception ex)
                {
                    Console.Error.WriteLine("POP3 error: " + ex.Message);
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine("Error: " + ex.Message);
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine("Unhandled error: " + ex.Message);
        }
    }
}
