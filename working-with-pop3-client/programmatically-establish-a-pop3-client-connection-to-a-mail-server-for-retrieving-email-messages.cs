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
            // POP3 server configuration
            string host = "pop.example.com";
            int port = 110;
            string username = "user@example.com";
            string password = "password";

            // Directory to save retrieved messages
            string outputDir = "SavedMessages";

            // Ensure the output directory exists
            if (!Directory.Exists(outputDir))
            {
                Directory.CreateDirectory(outputDir);
            }

            // Create and use the POP3 client
            using (Pop3Client client = new Pop3Client(host, port, username, password, SecurityOptions.Auto))
            {
                try
                {
                    // Retrieve the list of messages
                    Pop3MessageInfoCollection messages = client.ListMessages();

                    foreach (Pop3MessageInfo info in messages)
                    {
                        // Fetch the full message
                        using (MailMessage message = client.FetchMessage(info.UniqueId))
                        {
                            Console.WriteLine($"Subject: {message.Subject}");

                            // Save the message to a file
                            string filePath = Path.Combine(outputDir, $"{info.UniqueId}.eml");
                            try
                            {
                                message.Save(filePath, SaveOptions.DefaultEml);
                            }
                            catch (Exception ex)
                            {
                                Console.Error.WriteLine($"Failed to save message {info.UniqueId}: {ex.Message}");
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"POP3 operation failed: {ex.Message}");
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
