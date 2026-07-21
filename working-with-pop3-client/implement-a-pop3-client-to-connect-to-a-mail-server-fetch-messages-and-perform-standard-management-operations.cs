using System;
using System.IO;
using Aspose.Email;
using Aspose.Email.Clients;
using Aspose.Email.Clients.Pop3;

namespace Pop3Sample
{
    class Program
    {
        static void Main()
        {
            // POP3 server configuration (replace with real credentials)
            string host = "pop.example.com";
            int port = 110;
            bool enableSsl = false;
            string username = "user@example.com";
            string password = "password";

            // Directory to store downloaded messages
            string outputDirectory = "DownloadedEmails";


            // Skip external calls when placeholder credentials are used
            if (host.Contains("example.com") || username.Contains("example.com") || password == "password")
            {
                Console.Error.WriteLine("Placeholder credentials detected. Skipping external calls.");
                return;
            }

            // Ensure the output directory exists
            try
            {
                if (!Directory.Exists(outputDirectory))
                {
                    Directory.CreateDirectory(outputDirectory);
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
                using (Pop3Client pop3Client = new Pop3Client())
                {
                    pop3Client.Host = host;
                    pop3Client.Port = port;
                    pop3Client.Username = username;
                    pop3Client.Password = password;
                    pop3Client.SecurityOptions = enableSsl ? SecurityOptions.SSLImplicit : SecurityOptions.Auto;

                    // Get total message count
                    int messageCount = pop3Client.GetMessageCount();
                    Console.WriteLine($"Total messages on server: {messageCount}");

                    for (int sequenceNumber = 1; sequenceNumber <= messageCount; sequenceNumber++)
                    {
                        try
                        {
                            // Retrieve message info (used for filename)
                            Pop3MessageInfo info = pop3Client.GetMessageInfo(sequenceNumber);

                            // Build a safe filename from the subject
                            string subject = info.Subject ?? "NoSubject";
                            foreach (char c in Path.GetInvalidFileNameChars())
                            {
                                subject = subject.Replace(c.ToString(), string.Empty);
                            }
                            if (subject.Length > 50)
                            {
                                subject = subject.Substring(0, 50);
                            }

                            string fileName = $"{sequenceNumber}_{subject}.eml";
                            string filePath = Path.Combine(outputDirectory, fileName);

                            // Avoid overwriting existing files
                            if (File.Exists(filePath))
                            {
                                int duplicateCounter = 1;
                                while (File.Exists(filePath))
                                {
                                    filePath = Path.Combine(outputDirectory, $"{sequenceNumber}_{subject}_{duplicateCounter}.eml");
                                    duplicateCounter++;
                                }
                            }

                            // Fetch the full message
                            MailMessage message = pop3Client.FetchMessage(sequenceNumber);

                            // Save the message to disk
                            message.Save(filePath);
                            Console.WriteLine($"Saved message {sequenceNumber} to \"{filePath}\"");

                            // Optionally delete the message from the server after saving
                            pop3Client.DeleteMessage(sequenceNumber);
                        }
                        catch (Exception msgEx)
                        {
                            Console.Error.WriteLine($"Error processing message #{sequenceNumber}: {msgEx.Message}");
                        }
                    }

                    // Commit deletions (if any messages were marked for deletion)
                    try
                    {
                        Console.WriteLine("Deleted marked messages from the server.");
                    }
                    catch (Exception delEx)
                    {
                        Console.Error.WriteLine($"Failed to commit deletions: {delEx.Message}");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"POP3 operation failed: {ex.Message}");
            }
        }
    }
}
