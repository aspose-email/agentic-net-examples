using System;
using System.IO;
using System.Text;
using System.Collections.Generic;
using Aspose.Email;
using Aspose.Email.Clients.Google;

class Program
{
    static void Main()
    {
        try
        {
            // Placeholder Gmail OAuth credentials
            string clientId = "your_client_id";
            string clientSecret = "your_client_secret";
            string refreshToken = "your_refresh_token";
            string defaultEmail = "your_email@example.com";

            // Skip execution if placeholder credentials are detected
            if (clientId.StartsWith("your_") || clientSecret.StartsWith("your_") ||
                refreshToken.StartsWith("your_") || defaultEmail.StartsWith("your_"))
            {
                Console.Error.WriteLine("Gmail credentials are not set. Skipping execution.");
                return;
            }

            // Create Gmail client instance
            using (IGmailClient gmailClient = GmailClient.GetInstance(clientId, clientSecret, refreshToken, defaultEmail))
            {
                List<GmailMessageInfo> messagesInfo;
                try
                {
                    messagesInfo = gmailClient.ListMessages();
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to list Gmail messages: {ex.Message}");
                    return;
                }

                // Output directory for saved messages
                string outputDir = Path.Combine(Environment.CurrentDirectory, "GmailMessages");
                try
                {
                    if (!Directory.Exists(outputDir))
                    {
                        Directory.CreateDirectory(outputDir);
                    }
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to create output directory: {ex.Message}");
                    return;
                }

                foreach (GmailMessageInfo info in messagesInfo)
                {
                    MailMessage message = null;
                    try
                    {
                        // Fetch the full message
                        message = gmailClient.FetchMessage(info.Id);
                    }
                    catch (Exception ex)
                    {
                        Console.Error.WriteLine($"Failed to fetch message ID {info.Id}: {ex.Message}");
                        continue;
                    }

                    using (message)
                    {
                        // Ensure the body is encoded in UTF-8
                        message.BodyEncoding = Encoding.UTF8;

                        // Prepare file path (use message ID as filename)
                        string safeFileName = $"{info.Id}.eml";
                        string filePath = Path.Combine(outputDir, safeFileName);

                        try
                        {
                            // Save the message as EML
                            message.Save(filePath);
                            Console.WriteLine($"Saved message ID {info.Id} to {filePath}");
                        }
                        catch (Exception ex)
                        {
                            Console.Error.WriteLine($"Failed to save message ID {info.Id}: {ex.Message}");
                        }
                    }
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
