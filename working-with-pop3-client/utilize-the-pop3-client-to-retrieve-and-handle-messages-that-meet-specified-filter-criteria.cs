using System;
using System.IO;
using Aspose.Email;
using Aspose.Email.Clients.Pop3;

class Program
{
    static void Main()
    {
        try
        {
            // POP3 server connection settings
            string host = "pop.example.com";
            int port = 110; // change to 995 for SSL
            string username = "user@example.com";
            string password = "password";

            // Output directory for retrieved messages
            string outputDir = "RetrievedMessages";


            // Skip external calls when placeholder credentials are used
            if (host.Contains("example.com") || username.Contains("example.com") || password == "password")
            {
                Console.Error.WriteLine("Placeholder credentials detected. Skipping external calls.");
                return;
            }

            // Ensure the output directory exists
            try
            {
                if (!Directory.Exists(outputDir))
                {
                    Directory.CreateDirectory(outputDir);
                }
            }
            catch (Exception dirEx)
            {
                Console.Error.WriteLine($"Failed to prepare output directory: {dirEx.Message}");
                return;
            }

            // Connect to POP3 server and process messages
            using (Pop3Client pop3Client = new Pop3Client(host, port, username, password))
            {
                try
                {
                    int messageCount = pop3Client.GetMessageCount();

                    for (int i = 1; i <= messageCount; i++)
                    {
                        // Fetch the full message
                        MailMessage message = pop3Client.FetchMessage(i);

                        // Example filter: only process messages whose subject contains "Invoice"
                        if (message.Subject != null && message.Subject.IndexOf("Invoice", StringComparison.OrdinalIgnoreCase) >= 0)
                        {
                            string filePath = Path.Combine(outputDir, $"Message_{i}.eml");

                            try
                            {
                                // Save the message to the file system
                                message.Save(filePath);
                                Console.WriteLine($"Saved filtered message #{i} to '{filePath}'.");
                            }
                            catch (Exception saveEx)
                            {
                                Console.Error.WriteLine($"Failed to save message #{i}: {saveEx.Message}");
                            }
                        }

                        // Dispose the message after use
                        message.Dispose();
                    }
                }
                catch (Pop3Exception popEx)
                {
                    Console.Error.WriteLine($"POP3 operation failed: {popEx.Message}");
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Unexpected error during POP3 processing: {ex.Message}");
                }
            }
        }
        catch (Exception e)
        {
            Console.Error.WriteLine($"Unhandled exception: {e.Message}");
        }
    }
}
