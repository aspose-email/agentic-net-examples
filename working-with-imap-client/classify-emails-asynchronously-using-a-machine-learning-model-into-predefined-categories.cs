using System;
using System.IO;
using System.Collections.Generic;
using System.Threading.Tasks;
using Aspose.Email;
using Aspose.Email.Clients;
using Aspose.Email.Clients.Imap;

class Program
{
    static async Task Main(string[] args)
    {
        try
        {
            // Placeholder credentials – skip real network calls if they are not set.
            string host = "imap.example.com";
            string username = "user@example.com";
            string password = "password";

            if (host.Contains("example.com"))
            {
                Console.Error.WriteLine("Placeholder IMAP host detected. Skipping network operations.");
                return;
            }

            // Create and dispose the IMAP client safely.
            using (ImapClient client = new ImapClient(host, username, password, SecurityOptions.Auto))
            {
                try
                {
                    // Select the INBOX folder.
                    await client.SelectFolderAsync("INBOX");

                    // Retrieve information about all messages in the folder.
                    ImapMessageInfoCollection messageInfos = await client.ListMessagesAsync();

                    // Store classification results.
                    List<(string Subject, string Category)> results = new List<(string Subject, string Category)>();

                    // Process each message asynchronously.
                    foreach (ImapMessageInfo info in messageInfos)
                    {
                        // Fetch the full message and ensure it is disposed after use.
                        using (MailMessage message = await client.FetchMessageAsync(info.UniqueId))
                        {
                            string category = ClassifyMessage(message);
                            string subject = message.Subject ?? string.Empty;
                            results.Add((subject, category));
                        }
                    }

                    // Prepare output file path.
                    string outputPath = "classification_results.csv";

                    // Ensure the directory exists.
                    string outputDirectory = Path.GetDirectoryName(outputPath);
                    if (!string.IsNullOrEmpty(outputDirectory) && !Directory.Exists(outputDirectory))
                    {
                        Directory.CreateDirectory(outputDirectory);
                    }

                    // Write results to CSV inside a guarded file I/O block.
                    try
                    {
                        using (StreamWriter writer = new StreamWriter(outputPath, false))
                        {
                            writer.WriteLine("Subject,Category");
                            foreach ((string Subject, string Category) result in results)
                            {
                                // Escape double quotes in the subject.
                                string escapedSubject = result.Subject.Replace("\"", "\"\"");
                                writer.WriteLine($"\"{escapedSubject}\",\"{result.Category}\"");
                            }
                        }

                        Console.WriteLine($"Classification results saved to {outputPath}");
                    }
                    catch (Exception ioEx)
                    {
                        Console.Error.WriteLine($"File I/O error: {ioEx.Message}");
                    }
                }
                catch (Exception clientEx)
                {
                    Console.Error.WriteLine($"IMAP operation failed: {clientEx.Message}");
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }

    // Simple keyword‑based stub classifier.
    static string ClassifyMessage(MailMessage message)
    {
        string subject = message.Subject?.ToLowerInvariant() ?? string.Empty;

        if (subject.Contains("invoice") || subject.Contains("billing"))
            return "Finance";

        if (subject.Contains("meeting") || subject.Contains("schedule"))
            return "Meeting";

        if (subject.Contains("support") || subject.Contains("help"))
            return "Support";

        return "Other";
    }
}
