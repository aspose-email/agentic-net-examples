using Aspose.Email.Mime;
using Aspose.Email;
using Aspose.Email.Clients;
using Aspose.Email.Clients.Pop3;
using System;

namespace AsposeEmailPop3Example
{
    class Program
    {
        static void Main()
        {
            try
            {
                // POP3 server credentials (replace with real values)
                string host = "pop3.example.com";
                string username = "username";
                string password = "password";

                // Skip execution when placeholder credentials are detected
                if (host.Contains("example") || username == "username")
                {
                    Console.Error.WriteLine("Placeholder POP3 credentials detected. Skipping network call.");
                    return;
                }

                // Create and dispose the POP3 client
                using (Pop3Client client = new Pop3Client(host, username, password, SecurityOptions.Auto))
                {
                    try
                    {
                        // Validate connection and credentials
                        client.ValidateCredentials();

                        // Retrieve headers of the third email (sequence number = 3)
                        HeaderCollection headers = client.GetMessageHeaders(3);

                        // Output the retrieved headers
                        foreach (string header in headers.Keys)
                        {
                            Console.WriteLine(header);
                        }
                    }
                    catch (Pop3Exception popEx)
                    {
                        Console.Error.WriteLine($"POP3 operation failed: {popEx.Message}");
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
}
