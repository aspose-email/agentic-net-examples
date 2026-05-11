using System;
using System.IO;
using System.Collections.Generic;
using Aspose.Email;
using Aspose.Email.Clients.Google;

public class Program
{
    public static void Main()
    {
        try
        {
            // Placeholder credentials – replace with real values for actual execution
            string accessToken = "YOUR_ACCESS_TOKEN";
            string defaultEmail = "user@example.com";
            string labelId = "YOUR_LABEL_ID";
            string outputDirectory = "Output";

            // Skip execution if placeholders are detected
            if (accessToken.StartsWith("YOUR_") || defaultEmail.StartsWith("user@") || labelId.StartsWith("YOUR_"))
            {
                Console.WriteLine("Placeholder credentials or label ID detected. Skipping Gmail retrieval.");
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
            catch (Exception dirEx)
            {
                Console.Error.WriteLine($"Failed to create output directory '{outputDirectory}': {dirEx.Message}");
                return;
            }

            // Create Gmail client
            try
            {
                using (IGmailClient gmailClient = GmailClient.GetInstance(accessToken, defaultEmail))
                {
                    // Retrieve all messages (filtering by label would require additional API calls)
                    List<GmailMessageInfo> messageInfos = gmailClient.ListMessages();

                    foreach (GmailMessageInfo messageInfo in messageInfos)
                    {
                        // Fetch the full message
                        using (MailMessage mailMessage = gmailClient.FetchMessage(messageInfo.Id))
                        {
                            // Placeholder for label filtering – assume messages belong to the desired label
                            string htmlBody = mailMessage.HtmlBody;
                            if (string.IsNullOrEmpty(htmlBody))
                            {
                                // Skip messages without HTML content
                                continue;
                            }

                            string filePath = Path.Combine(outputDirectory, $"{messageInfo.Id}.html");

                            try
                            {
                                using (FileStream fileStream = new FileStream(filePath, FileMode.Create, FileAccess.Write))
                                {
                                    using (StreamWriter writer = new StreamWriter(fileStream))
                                    {
                                        writer.Write(htmlBody);
                                    }
                                }
                            }
                            catch (Exception fileEx)
                            {
                                Console.Error.WriteLine($"Failed to save message '{messageInfo.Id}' to file: {fileEx.Message}");
                                // Continue with next message
                            }
                        }
                    }
                }
            }
            catch (Exception clientEx)
            {
                Console.Error.WriteLine($"Gmail client error: {clientEx.Message}");
                return;
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
