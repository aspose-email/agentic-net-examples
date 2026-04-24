using System;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Aspose.Email;
using Aspose.Email.Clients.Imap;
using Aspose.Email.Clients;

class Program
{
    static async Task Main(string[] args)
    {
        try
        {
            // Placeholder credentials – skip real network call if they are not replaced.
            string host = "imap.example.com";
            string username = "username";
            string password = "password";

            if (host.Contains("example.com") || username == "username" || password == "password")
            {
                Console.Error.WriteLine("Skipping IMAP connection because placeholder credentials are used.");
                return;
            }

            // Output CSV file path.
            string outputPath = "email_report.csv";

            // Ensure the directory exists.
            try
            {
                string? directory = Path.GetDirectoryName(outputPath);
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

            // Open the CSV file for writing.
            try
            {
                using (FileStream fileStream = new FileStream(outputPath, FileMode.Create, FileAccess.Write, FileShare.None))
                using (StreamWriter writer = new StreamWriter(fileStream, Encoding.UTF8))
                {
                    // Write CSV header.
                    await writer.WriteLineAsync("Sender,Subject,ReceivedDate").ConfigureAwait(false);

                    // Connect to the IMAP server.
                    using (ImapClient client = new ImapClient(host, username, password, SecurityOptions.Auto))
                    {
                        try
                        {
                            // Select the INBOX folder (default if not selected).
                            await client.SelectFolderAsync("INBOX", CancellationToken.None).ConfigureAwait(false);

                            // Retrieve the list of messages in the folder.
                            ImapMessageInfoCollection messagesInfo = await client.ListMessagesAsync(CancellationToken.None).ConfigureAwait(false);

                            foreach (ImapMessageInfo info in messagesInfo)
                            {
                                // Fetch the full message to obtain the sender address.
                                MailMessage message = await client.FetchMessageAsync(info.UniqueId, CancellationToken.None).ConfigureAwait(false);

                                // Sender may be null; fallback to the Sender property from the info object.
                                string sender = message.From?.ToString() ?? info.Sender?.ToString() ?? string.Empty;
                                string subject = info.Subject ?? string.Empty;
                                string receivedDate = info.Date.ToString("o"); // ISO 8601 format.

                                // Escape commas in fields.
                                sender = $"\"{sender.Replace("\"", "\"\"")}\"";
                                subject = $"\"{subject.Replace("\"", "\"\"")}\"";

                                await writer.WriteLineAsync($"{sender},{subject},{receivedDate}").ConfigureAwait(false);
                            }
                        }
                        catch (Exception ex)
                        {
                            Console.Error.WriteLine($"IMAP operation failed: {ex.Message}");
                            return;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"File I/O error: {ex.Message}");
                return;
            }

            Console.WriteLine($"CSV report generated at: {Path.GetFullPath(outputPath)}");
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
