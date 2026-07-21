using System;
using System.IO;
using Aspose.Email;
using Aspose.Email.Clients;
using Aspose.Email.Clients.Pop3;

namespace Pop3RetrieveExample
{
    class Program
    {
        static void Main()
        {
            try
            {
                // POP3 server connection settings
                string host = "pop.example.com";
                int port = 110; // use 995 for SSL
                string username = "user@example.com";
                string password = "password";

                // Create and use the POP3 client inside a using block to ensure proper disposal
                using (Pop3Client client = new Pop3Client(host, port, username, password, SecurityOptions.Auto))
                {
                    // Get the total number of messages in the mailbox
                    int messageCount = client.GetMessageCount();
                    Console.WriteLine($"Message count: {messageCount}");

                    if (messageCount > 0)
                    {
                        // Retrieve the first message (index starts at 1)
                        MailMessage message = client.FetchMessage(1);

                        // Prepare output directory and file path
                        string outputDirectory = "output";

                        // Skip external calls when placeholder credentials are used
                        if (host.Contains("example.com") || username.Contains("example.com") || password == "password")
                        {
                            Console.Error.WriteLine("Placeholder credentials detected. Skipping external calls.");
                            return;
                        }

                        string outputPath = Path.Combine(outputDirectory, "message.eml");

                        // Ensure the output directory exists before saving
                        if (!Directory.Exists(outputDirectory))
                        {
                            Directory.CreateDirectory(outputDirectory);
                        }

                        // Save the retrieved message to an .eml file
                        message.Save(outputPath);
                        Console.WriteLine($"Message saved to: {outputPath}");
                    }
                    else
                    {
                        Console.WriteLine("No messages available to retrieve.");
                    }
                }
            }
            catch (Exception ex)
            {
                // Log any unexpected errors without crashing the application
                Console.Error.WriteLine($"Error: {ex.Message}");
            }
        }
    }
}
