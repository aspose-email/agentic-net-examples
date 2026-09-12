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
            // ----- POP3 server configuration -----
            string host = "your_pop3_host";          // e.g., "pop.mailserver.com"
            int port = 110;                          // default POP3 port
            string username = "your_username";       // e.g., "user@example.com"
            string password = "your_password";

            // Guard against placeholder credentials
            if (host.StartsWith("your_") || username.StartsWith("your_") || password.StartsWith("your_"))
            {
                Console.Error.WriteLine("Please replace placeholder POP3 credentials with real values.");
                return;
            }

            // Optional: configure a proxy if needed (commented out as example)
            // var proxy = new System.Net.WebProxy("http://proxy.example.com:8080");
            // client.Proxy = proxy;

            // ----- Create POP3 client -----
            using (Pop3Client client = new Pop3Client(host, port, username, password, SecurityOptions.Auto))
            {
                try
                {
                    // Connect implicitly by performing an operation
                    // ----- Identify the message to retrieve -----
                    int messageSequenceNumber = 1; // replace with the actual sequence number

                    // Guard against placeholder message identifier
                    if (messageSequenceNumber <= 0)
                    {
                        Console.Error.WriteLine("Please provide a valid message sequence number.");
                        return;
                    }

                    // ----- Retrieve the message -----
                    MailMessage message = client.FetchMessage(messageSequenceNumber);

                    // ----- Save the retrieved message to disk -----
                    string outputPath = "retrieved.eml";

                    // Ensure the output directory exists
                    string outputDir = Path.GetDirectoryName(Path.GetFullPath(outputPath));
                    if (!Directory.Exists(outputDir))
                    {
                        Directory.CreateDirectory(outputDir);
                    }

                    try
                    {
                        message.Save(outputPath);
                        Console.WriteLine($"Message saved to: {outputPath}");
                    }
                    catch (Exception ioEx)
                    {
                        Console.Error.WriteLine($"Failed to save message: {ioEx.Message}");
                    }
                }
                catch (Pop3Exception popEx)
                {
                    Console.Error.WriteLine($"POP3 error: {popEx.Message}");
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Error during POP3 operation: {ex.Message}");
                }
            }
        }
        catch (Exception e)
        {
            Console.Error.WriteLine($"Unexpected error: {e.Message}");
        }
    }
}
