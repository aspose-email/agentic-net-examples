using System;
using System.IO;
using System.Collections.Generic;
using Aspose.Email;
using Aspose.Email.Clients.Google;

namespace AsposeEmailGmailExport
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                // Placeholder credentials – replace with real values.
                string accessToken = "YOUR_ACCESS_TOKEN";
                string defaultEmail = "user@example.com";

                // Skip execution if placeholders are detected.
                if (accessToken.StartsWith("YOUR_") || defaultEmail.StartsWith("user@"))
                {
                    Console.WriteLine("Placeholder credentials detected. Skipping Gmail operations.");
                    return;
                }

                // Create Gmail client instance.
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
                    // Retrieve all messages (could be filtered for unread if needed).
                    List<GmailMessageInfo> messages = null;
                    try
                    {
                        messages = gmailClient.ListMessages();
                    }
                    catch (Exception ex)
                    {
                        Console.Error.WriteLine($"Error listing messages: {ex.Message}");
                        return;
                    }

                    // Prepare CSV output.
                    string outputPath = "unread_messages.csv";
                    string directory = Path.GetDirectoryName(outputPath);
                    if (!string.IsNullOrEmpty(directory) && !Directory.Exists(directory))
                    {
                        try
                        {
                            Directory.CreateDirectory(directory);
                        }
                        catch (Exception ex)
                        {
                            Console.Error.WriteLine($"Failed to create directory '{directory}': {ex.Message}");
                            return;
                        }
                    }

                    try
                    {
                        using (StreamWriter writer = new StreamWriter(outputPath))
                        {
                            // CSV header.
                            writer.WriteLine("Id,Subject,From,Date");

                            foreach (GmailMessageInfo info in messages)
                            {
                                // Fetch full message to obtain subject, from, and date.
                                MailMessage msg = null;
                                try
                                {
                                    msg = gmailClient.FetchMessage(info.Id);
                                }
                                catch (Exception ex)
                                {
                                    Console.Error.WriteLine($"Failed to fetch message {info.Id}: {ex.Message}");
                                    continue;
                                }

                                using (msg)
                                {
                                    string subject = (msg.Subject ?? string.Empty).Replace("\"", "\"\"");
                                    string from = (msg.From?.Address ?? string.Empty).Replace("\"", "\"\"");
                                    string date = msg.Date.ToString("o"); // ISO 8601 format.

                                    writer.WriteLine($"{info.Id},\"{subject}\",\"{from}\",{date}");
                                }
                            }
                        }

                        Console.WriteLine($"Exported {messages?.Count ?? 0} messages to '{outputPath}'.");
                    }
                    catch (Exception ex)
                    {
                        Console.Error.WriteLine($"Error writing CSV file: {ex.Message}");
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
