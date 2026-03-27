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
            string host = "pop3.example.com";
            int port = 995;
            string username = "user@example.com";
            string password = "password";

            // Initialize POP3 client
            using (Pop3Client client = new Pop3Client(host, port, username, password, SecurityOptions.Auto))
            {
                try
                {
                    // Retrieve list of messages
                    Pop3MessageInfoCollection messages = client.ListMessages();

                    foreach (Pop3MessageInfo info in messages)
                    {
                        Console.WriteLine($"Message #{info.SequenceNumber} - Subject: {info.Subject}");

                        // Fetch full message
                        using (MailMessage message = client.FetchMessage(info.SequenceNumber))
                        {
                            // Ensure output directory exists
                            string outputDir = "SavedMessages";
                            if (!Directory.Exists(outputDir))
                            {
                                Directory.CreateDirectory(outputDir);
                            }

                            // Define file path
                            string filePath = Path.Combine(outputDir, $"Message_{info.SequenceNumber}.eml");

                            // Save message to file
                            try
                            {
                                using (FileStream fs = new FileStream(filePath, FileMode.Create, FileAccess.Write))
                                {
                                    message.Save(fs, SaveOptions.DefaultEml);
                                }
                            }
                            catch (Exception ex)
                            {
                                Console.Error.WriteLine($"Failed to save message #{info.SequenceNumber}: {ex.Message}");
                                continue;
                            }
                        }

                        // Mark message for deletion
                        client.DeleteMessage(info.SequenceNumber);
                    }

                    // Commit deletions on the server
                    client.CommitDeletes();
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"POP3 operation error: {ex.Message}");
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
