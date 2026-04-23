using Aspose.Email.Clients;
using System;
using System.IO;
using Aspose.Email;
using Aspose.Email.Clients.Imap;
using Aspose.Email.Tools.Search;

class Program
{
    static void Main()
    {
        try
        {
            // Placeholder connection settings
            string host = "imap.example.com";
            int port = 993;
            string username = "username";
            string password = "password";

            // Skip network call if placeholders are detected
            if (host.Contains("example.com") || username.Equals("username", StringComparison.OrdinalIgnoreCase))
            {
                Console.WriteLine("Placeholder credentials detected. Skipping IMAP operations.");
                return;
            }

            // Create and connect the IMAP client inside a using block
            using (ImapClient client = new ImapClient(host, port, username, password, SecurityOptions.Auto))
            {
                try
                {
                    // Select the INBOX folder
                    client.SelectFolder("INBOX");

                    // Build a query to filter messages whose subject contains the keyword "Invoice"
                    MailQueryBuilder queryBuilder = new MailQueryBuilder();
                    queryBuilder.Subject.Contains("Invoice");
                    MailQuery query = queryBuilder.GetQuery();

                    // Retrieve messages matching the query
                    ImapMessageInfoCollection messages = client.ListMessages(query);

                    // Prepare output directory
                    string outputDir = Path.Combine(Environment.CurrentDirectory, "FilteredMessages");
                    if (!Directory.Exists(outputDir))
                    {
                        Directory.CreateDirectory(outputDir);
                    }

                    // Process each matching message
                    foreach (ImapMessageInfo info in messages)
                    {
                        try
                        {
                            // Fetch the full message
                            MailMessage message = client.FetchMessage(info.UniqueId);

                            // Create a safe file name based on the subject
                            string safeSubject = string.IsNullOrEmpty(message.Subject) ? "NoSubject" : message.Subject;
                            foreach (char c in Path.GetInvalidFileNameChars())
                            {
                                safeSubject = safeSubject.Replace(c, '_');
                            }
                            string filePath = Path.Combine(outputDir, safeSubject + ".msg");

                            // Save the message to a .msg file
                            message.Save(filePath);
                            Console.WriteLine($"Saved: {filePath}");
                        }
                        catch (Exception ex)
                        {
                            Console.Error.WriteLine($"Error processing message UID {info.UniqueId}: {ex.Message}");
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"IMAP operation failed: {ex.Message}");
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
