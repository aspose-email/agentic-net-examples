using System;
using System.IO;
using Aspose.Email;
using Aspose.Email.Clients.Imap;
using Aspose.Email.Clients;


class Program
{
    static void Main()
    {
        try
        {
            // Placeholder connection settings – replace with real values.
            string host = "imap.example.com";
            string username = "user@example.com";
            string password = "password";
            string outputCsvPath = "SentEmails.csv";

            // Skip execution when placeholder credentials are detected.
            if (host.Contains("example.com"))
            {
                Console.Error.WriteLine("Skipping execution due to placeholder host/credentials.");
                return;
            }

            // Ensure the output directory exists.
            try
            {
                string? directory = Path.GetDirectoryName(outputCsvPath);
                if (!string.IsNullOrEmpty(directory) && !Directory.Exists(directory))
                {
                    Directory.CreateDirectory(directory);
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Failed to prepare output directory: {ex.Message}");
                return;
            }

            // Create and use the IMAP client.
            try
            {
                using (ImapClient client = new ImapClient(host, username, password, SecurityOptions.Auto))
                {
                    // Select the Sent folder.
                    client.SelectFolder("Sent");

                    // Retrieve messages from the selected folder.
                    ImapMessageInfoCollection messages = client.ListMessages();

                    // Write messages to CSV.
                    try
                    {
                        using (StreamWriter writer = new StreamWriter(outputCsvPath))
                        {
                            // CSV header.
                            writer.WriteLine("Subject,From,To,Date");

                            foreach (ImapMessageInfo info in messages)
                            {
                                // Escape commas by enclosing fields in double quotes.
                                string subject = $"\"{info.Subject?.Replace("\"", "\"\"")}\"";
                                string from = $"\"{info.From?.ToString().Replace("\"", "\"\"")}\"";
                                string to = $"\"{info.To?.ToString().Replace("\"", "\"\"")}\"";
                                string date = info.Date.ToString("o"); // ISO 8601 format.

                                writer.WriteLine($"{subject},{from},{to},{date}");
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.Error.WriteLine($"Error writing CSV file: {ex.Message}");
                        return;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"IMAP client error: {ex.Message}");
                return;
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
