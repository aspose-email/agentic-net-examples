using System;
using System.IO;
using Aspose.Email;
using Aspose.Email.Clients;
using Aspose.Email.Clients.Pop3;

class Program
{
    static int Main()
    {
        try
        {
            // POP3 server configuration (replace with real credentials)
            string host = "pop3.example.com";
            int port = 995;
            string username = "user@example.com";
            string password = "password";

            // Directory to store downloaded messages
            string outputDir = "DownloadedEmails";

            // Skip external calls when placeholder credentials are used
            if (host.Contains("example.com") || username.Contains("example.com") || password == "password")
            {
                Console.Error.WriteLine("Placeholder credentials detected. Skipping external calls.");
                return 0;
            }

            // Ensure the output directory exists
            if (!Directory.Exists(outputDir))
            {
                try
                {
                    Directory.CreateDirectory(outputDir);
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to create output directory '{outputDir}': {ex.Message}");
                    return 1;
                }
            }

            // Initialize POP3 client
            using (Pop3Client client = new Pop3Client())
            {
                client.Host = host;
                client.Port = port;
                client.Username = username;
                client.Password = password;
                client.SecurityOptions = SecurityOptions.Auto; // Adjust as needed (e.g., SSLImplicit)

                // Get total number of messages on the server
                int messageCount;
                try
                {
                    messageCount = client.GetMessageCount();
                }
                catch (Pop3Exception ex)
                {
                    Console.Error.WriteLine($"Error retrieving message count: {ex.Message}");
                    return 1;
                }

                Console.WriteLine($"Total messages on server: {messageCount}");

                // Iterate through each message
                for (int i = 1; i <= messageCount; i++)
                {
                    try
                    {
                        // Fetch the message (POP3 indexes start at 1)
                        using (MailMessage message = client.FetchMessage(i))
                        {
                            // Create a safe filename based on the subject
                            string safeSubject = string.IsNullOrEmpty(message.Subject) ? "NoSubject" : message.Subject;
                            foreach (char c in Path.GetInvalidFileNameChars())
                            {
                                safeSubject = safeSubject.Replace(c, '_');
                            }

                            string filePath = Path.Combine(outputDir, $"msg_{i}_{safeSubject}.eml");

                            // Save the message to disk
                            try
                            {
                                message.Save(filePath);
                                Console.WriteLine($"Saved message {i} to '{filePath}'.");
                                
                                // Delete the message from the server after successful download
                                client.DeleteMessage(i);
                                Console.WriteLine($"Deleted message {i} from server.");
                            }
                            catch (Exception ex)
                            {
                                Console.Error.WriteLine($"Failed to save or delete message {i}: {ex.Message}");
                            }
                        }
                    }
                    catch (Pop3Exception ex)
                    {
                        Console.Error.WriteLine($"Failed to fetch message {i}: {ex.Message}");
                    }
                }
            }

            return 0;
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
            return 1;
        }
    }
}
