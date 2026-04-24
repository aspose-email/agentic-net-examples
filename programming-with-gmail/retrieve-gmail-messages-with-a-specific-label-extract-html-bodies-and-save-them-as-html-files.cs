using System;
using System.IO;
using System.Collections.Generic;
using Aspose.Email;
using Aspose.Email.Clients.Google;

namespace GmailHtmlExport
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                // Placeholder credentials – replace with real values before running.
                string accessToken = "YOUR_ACCESS_TOKEN";
                string defaultEmail = "user@example.com";

                // Guard against placeholder credentials to avoid live network calls in CI.
                if (string.IsNullOrWhiteSpace(accessToken) || accessToken == "YOUR_ACCESS_TOKEN")
                {
                    Console.Error.WriteLine("Gmail credentials are not set. Skipping execution.");
                    return;
                }

                // Create Gmail client.
                using (IGmailClient gmailClient = GmailClient.GetInstance(accessToken, defaultEmail))
                {
                    // Retrieve list of messages.
                    List<GmailMessageInfo> messageInfos = gmailClient.ListMessages();

                    // Prepare output directory.
                    string outputDirectory = "GmailHtml";
                    if (!Directory.Exists(outputDirectory))
                    {
                        Directory.CreateDirectory(outputDirectory);
                    }

                    foreach (GmailMessageInfo info in messageInfos)
                    {
                        // Fetch the full message.
                        using (MailMessage message = gmailClient.FetchMessage(info.Id))
                        {
                            string htmlBody = message.HtmlBody;
                            if (string.IsNullOrEmpty(htmlBody))
                            {
                                // Skip messages without HTML content.
                                continue;
                            }

                            // Build a safe file name from the subject.
                            string subject = string.IsNullOrEmpty(message.Subject) ? "NoSubject" : message.Subject;
                            foreach (char invalidChar in Path.GetInvalidFileNameChars())
                            {
                                subject = subject.Replace(invalidChar, '_');
                            }

                            string filePath = Path.Combine(outputDirectory, subject + ".html");

                            // Save HTML body to file with error handling.
                            try
                            {
                                File.WriteAllText(filePath, htmlBody);
                            }
                            catch (Exception ioEx)
                            {
                                Console.Error.WriteLine($"Failed to write file '{filePath}': {ioEx.Message}");
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Error: {ex.Message}");
            }
        }
    }
}
