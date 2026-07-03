using Aspose.Email.Mime;
using System;
using Aspose.Email;
using Aspose.Email.Clients;
using Aspose.Email.Clients.Pop3;

class Program
{
    static void Main(string[] args)
    {
        try
        {
            // Placeholder credentials – skip real network call if they are not replaced.
            string host = "pop.example.com";
            string username = "username";
            string password = "password";

            if (host.Contains("example.com") || username == "username" || password == "password")
            {
                Console.WriteLine("Skipping POP3 operations due to placeholder credentials.");
                return;
            }

            // Create and connect the POP3 client.
            using (Pop3Client client = new Pop3Client(host, username, password, SecurityOptions.Auto))
            {
                try
                {
                    client.ValidateCredentials();
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to connect/validate POP3 client: {ex.Message}");
                    return;
                }

                // Loop through sequence numbers 10 to 20 and fetch headers.
                for (int sequenceNumber = 10; sequenceNumber <= 20; sequenceNumber++)
                {
                    try
                    {
                        HeaderCollection headers = client.GetMessageHeaders(sequenceNumber);
                        string subject = headers["Subject"] ?? "(no subject)";
                        Console.WriteLine($"Message {sequenceNumber} Subject: {subject}");
                    }
                    catch (Exception ex)
                    {
                        Console.Error.WriteLine($"Error retrieving headers for message {sequenceNumber}: {ex.Message}");
                    }
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
