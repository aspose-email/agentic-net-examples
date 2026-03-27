using System;
using System.IO;
using Aspose.Email;
using Aspose.Email.Clients;
using Aspose.Email.Clients.Pop3;

namespace Sample
{
    class Program
    {
        static void Main()
        {
            try
            {
                // POP3 server settings
                string host = "pop3.example.com";
                int port = 995;
                string username = "user@example.com";
                string password = "password";

                // Ensure the output directory exists
                string outputDir = "output";
                try
                {
                    if (!Directory.Exists(outputDir))
                    {
                        Directory.CreateDirectory(outputDir);
                    }
                }
                catch (Exception dirEx)
                {
                    Console.Error.WriteLine($"Failed to create output directory: {dirEx.Message}");
                    return;
                }

                // Create and use the POP3 client
                using (Pop3Client client = new Pop3Client(host, port, username, password, SecurityOptions.Auto))
                {
                    try
                    {
                        // Retrieve the list of messages
                        Pop3MessageInfoCollection messages = client.ListMessages();

                        int index = 1;
                        foreach (Pop3MessageInfo info in messages)
                        {
                            // Fetch the full message
                            using (MailMessage message = client.FetchMessage(info.UniqueId))
                            {
                                string filePath = Path.Combine(outputDir, $"Message_{index}.eml");
                                try
                                {
                                    // Save the message to a file
                                    message.Save(filePath, SaveOptions.DefaultEml);
                                    Console.WriteLine($"Saved message {index}: {info.Subject}");
                                }
                                catch (Exception saveEx)
                                {
                                    Console.Error.WriteLine($"Failed to save message {index}: {saveEx.Message}");
                                }
                            }
                            index++;
                        }
                    }
                    catch (Exception clientEx)
                    {
                        Console.Error.WriteLine($"POP3 operation failed: {clientEx.Message}");
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
