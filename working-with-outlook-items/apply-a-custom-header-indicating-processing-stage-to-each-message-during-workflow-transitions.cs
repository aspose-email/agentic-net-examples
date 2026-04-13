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
            // Placeholder POP3 server details
            string host = "pop3.example.com";
            int port = 110;
            string username = "username";
            string password = "password";

            // Skip actual network call when placeholders are used
            if (host.Contains("example.com"))
            {
                Console.WriteLine("Skipping POP3 operations due to placeholder credentials.");
                return;
            }

            // Ensure the output directory exists
            string outputDir = "ProcessedMessages";
            try
            {
                if (!Directory.Exists(outputDir))
                {
                    Directory.CreateDirectory(outputDir);
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Failed to prepare output directory: {ex.Message}");
                return;
            }

            // Connect to POP3 server and process messages
            try
            {
                using (Pop3Client pop3Client = new Pop3Client(host, port, username, password))
                {
                    // Validate credentials safely
                    try
                    {
                        pop3Client.ValidateCredentials();
                    }
                    catch (Exception credEx)
                    {
                        Console.Error.WriteLine($"Authentication failed: {credEx.Message}");
                        return;
                    }

                    int messageCount = pop3Client.GetMessageCount();
                    Console.WriteLine($"Total messages: {messageCount}");

                    for (int i = 1; i <= messageCount; i++)
                    {
                        try
                        {
                            using (MailMessage message = pop3Client.FetchMessage(i))
                            {
                                // Add a custom header to indicate processing stage
                                message.Headers.Add("X-Processing-Stage", "Processed");

                                // Save the modified message to a local .eml file
                                string filePath = Path.Combine(outputDir, $"msg_{i}.eml");
                                try
                                {
                                    message.Save(filePath);
                                    Console.WriteLine($"Saved processed message #{i} to {filePath}");
                                }
                                catch (Exception saveEx)
                                {
                                    Console.Error.WriteLine($"Failed to save message #{i}: {saveEx.Message}");
                                }
                            }
                        }
                        catch (Exception fetchEx)
                        {
                            Console.Error.WriteLine($"Failed to fetch message #{i}: {fetchEx.Message}");
                        }
                    }
                }
            }
            catch (Exception clientEx)
            {
                Console.Error.WriteLine($"POP3 client error: {clientEx.Message}");
                return;
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
