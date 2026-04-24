using System;
using System.IO;
using System.Collections.Generic;
using Aspose.Email;
using Aspose.Email.Clients.Google;

class Program
{
    static void Main()
    {
        try
        {
            // Placeholder credentials – replace with real values.
            string accessToken = "YOUR_ACCESS_TOKEN";
            string defaultEmail = "user@example.com";

            // Guard against running with placeholder credentials.
            if (accessToken.StartsWith("YOUR_") || defaultEmail.StartsWith("user@"))
            {
                Console.Error.WriteLine("Please provide a valid Gmail OAuth access token and email address.");
                return;
            }

            // Create Gmail client.
            IGmailClient gmailClient = null;
            try
            {
                gmailClient = GmailClient.GetInstance(accessToken, defaultEmail);
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Failed to create Gmail client: {ex.Message}");
                return;
            }

            using (gmailClient)
            {
                // Retrieve a list of messages.
                List<GmailMessageInfo> messageInfos = null;
                try
                {
                    messageInfos = gmailClient.ListMessages();
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to list messages: {ex.Message}");
                    return;
                }

                if (messageInfos == null || messageInfos.Count == 0)
                {
                    Console.Error.WriteLine("No messages found in the Gmail account.");
                    return;
                }

                // Use the first message's ID for demonstration.
                string messageId = messageInfos[0].Id;

                // Fetch the full message.
                MailMessage mailMessage = null;
                try
                {
                    mailMessage = gmailClient.FetchMessage(messageId);
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to fetch message with ID '{messageId}': {ex.Message}");
                    return;
                }

                using (mailMessage)
                {
                    string outputPath = "FetchedMessage.eml";

                    // Ensure the output directory exists.
                    string outputDirectory = Path.GetDirectoryName(Path.GetFullPath(outputPath));
                    if (!Directory.Exists(outputDirectory))
                    {
                        try
                        {
                            Directory.CreateDirectory(outputDirectory);
                        }
                        catch (Exception ex)
                        {
                            Console.Error.WriteLine($"Failed to create directory '{outputDirectory}': {ex.Message}");
                            return;
                        }
                    }

                    // Save the message as an EML file.
                    try
                    {
                        mailMessage.Save(outputPath);
                        Console.WriteLine($"Message saved successfully to '{outputPath}'.");
                    }
                    catch (Exception ex)
                    {
                        Console.Error.WriteLine($"Failed to save message: {ex.Message}");
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
