using System;
using System.IO;
using System.Text;
using Aspose.Email;
using Aspose.Email.Clients;
using Aspose.Email.Clients.Imap;

class Program
{
    static void Main(string[] args)
    {
        try
        {
            // Configuration – replace with real values when available
            string host = "imap.example.com";
            int port = 993;
            string username = "user@example.com";
            string password = "password";
            string outputCsv = "metadata.csv";

            // Skip execution when placeholder credentials are detected
            if (host.Contains("example.com") || username.Contains("example.com") || password == "password")
            {
                Console.WriteLine("Placeholder credentials detected – execution skipped.");
                return;
            }

            // Ensure the output directory exists
            try
            {
                string outputDirectory = Path.GetDirectoryName(outputCsv);
                if (!string.IsNullOrEmpty(outputDirectory) && !Directory.Exists(outputDirectory))
                {
                    Directory.CreateDirectory(outputDirectory);
                }
            }
            catch (Exception dirEx)
            {
                Console.Error.WriteLine($"Failed to prepare output directory: {dirEx.Message}");
                return;
            }

            // Connect to the IMAP server and retrieve messages
            try
            {
                using (ImapClient client = new ImapClient(host, port, SecurityOptions.SSLImplicit))
                {
                    client.Username = username;
                    client.Password = password;

                    // Attempt to select the INBOX folder to validate the connection
                    client.SelectFolder("INBOX");

                    // Retrieve message information from the selected folder
                    ImapMessageInfoCollection messageInfos = client.ListMessages();

                    // Write metadata to CSV
                    try
                    {
                        using (StreamWriter writer = new StreamWriter(outputCsv, false, Encoding.UTF8))
                        {
                            writer.WriteLine("Subject,From,Date");
                            foreach (ImapMessageInfo info in messageInfos)
                            {
                                string subject = info.Subject ?? string.Empty;
                                string from = info.From != null ? info.From.ToString() : string.Empty;
                                string date = info.Date.ToString("o"); // ISO 8601 format

                                // Escape double quotes in CSV fields
                                subject = subject.Replace("\"", "\"\"");
                                from = from.Replace("\"", "\"\"");

                                writer.WriteLine($"\"{subject}\",\"{from}\",\"{date}\"");
                            }
                        }
                    }
                    catch (Exception ioEx)
                    {
                        Console.Error.WriteLine($"File I/O error while writing CSV: {ioEx.Message}");
                        return;
                    }
                }
            }
            catch (Exception imapEx)
            {
                Console.Error.WriteLine($"IMAP operation failed: {imapEx.Message}");
                return;
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
