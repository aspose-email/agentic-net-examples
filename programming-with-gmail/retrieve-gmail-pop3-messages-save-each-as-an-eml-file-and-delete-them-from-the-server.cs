using System;
using System.IO;
using Aspose.Email;
using Aspose.Email.Clients.Pop3;

namespace Sample
{
    class Program
    {
        static void Main()
        {
            try
            {
                // POP3 server settings (replace with real credentials)
                string host = "pop.gmail.com";
                int port = 995;
                string username = "your.email@gmail.com";
                string password = "yourpassword";
                string outputDir = "Emails";

                // Skip execution if placeholder credentials are detected
                if (host.Contains("example.com") || username.Contains("username") || password.Contains("password"))
                {
                    Console.Error.WriteLine("Placeholder credentials detected. Skipping POP3 operations.");
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
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to prepare output directory: {ex.Message}");
                    return;
                }

                // Connect to POP3 server
                using (Pop3Client client = new Pop3Client(host, port, username, password))
                {
                    try
                    {
                        // Validate credentials
                        client.ValidateCredentials();

                        // Get total number of messages
                        int messageCount = client.GetMessageCount();

                        // Process each message
                        for (int i = 1; i <= messageCount; i++)
                        {
                            string filePath = Path.Combine(outputDir, $"Message_{i}.eml");

                            // Save the message as .eml
                            try
                            {
                                client.SaveMessage(i, filePath);
                            }
                            catch (Exception ex)
                            {
                                Console.Error.WriteLine($"Failed to save message {i}: {ex.Message}");
                                continue;
                            }

                            // Delete the message from the server
                            try
                            {
                                client.DeleteMessage(i);
                            }
                            catch (Exception ex)
                            {
                                Console.Error.WriteLine($"Failed to delete message {i}: {ex.Message}");
                            }
                        }

                        // Commit deletions to finalize removal on the server
                    }
                    catch (Exception ex)
                    {
                        Console.Error.WriteLine($"POP3 operation failed: {ex.Message}");
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
