using Aspose.Email.Clients;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Aspose.Email;
using Aspose.Email.Clients.Imap;
using Aspose.Email.Clients.Imap.Models;

namespace AsposeEmailImapDomainStats
{
    class Program
    {
        static async Task Main(string[] args)
        {
            try
            {
                // Placeholder connection settings – replace with real values.
                string host = "imap.example.com";
                int port = 993;
                string username = "user@example.com";
                string password = "password";

                // Guard against running with placeholder credentials.
                if (host.Contains("example.com") || username.Contains("example.com"))
                {
                    Console.Error.WriteLine("Placeholder IMAP credentials detected. Skipping network operation.");
                    return;
                }

                // Prepare a dictionary to hold domain counts.
                Dictionary<string, int> domainCounts = new Dictionary<string, int>(StringComparer.OrdinalIgnoreCase);

                // Use ImapClient inside a using block to ensure proper disposal.
                using (ImapClient client = new ImapClient(host, port, username, password, SecurityOptions.Auto))
                {
                    // Validate credentials by selecting the INBOX folder.
                    try
                    {
                        await client.SelectFolderAsync("INBOX");
                    }
                    catch (Exception ex)
                    {
                        Console.Error.WriteLine($"Failed to connect or authenticate to IMAP server: {ex.Message}");
                        return;
                    }

                    // Retrieve the list of message infos from the INBOX.
                    ImapMessageInfoCollection messagesInfo = await client.ListMessagesAsync("INBOX");

                    // Fetch each message asynchronously and extract the sender domain.
                    List<Task> fetchTasks = new List<Task>();
                    foreach (ImapMessageInfo info in messagesInfo)
                    {
                        fetchTasks.Add(Task.Run(async () =>
                        {
                            try
                            {
                                MailMessage message = await client.FetchMessageAsync(info.UniqueId);
                                if (message?.From != null && message.From.Count > 0)
                                {
                                    string email = message.From[0].Address;
                                    string domain = email.Substring(email.IndexOf('@') + 1);
                                    lock (domainCounts)
                                    {
                                        if (domainCounts.ContainsKey(domain))
                                            domainCounts[domain]++;
                                        else
                                            domainCounts[domain] = 1;
                                    }
                                }
                            }
                            catch (Exception fetchEx)
                            {
                                Console.Error.WriteLine($"Error fetching message UID {info.UniqueId}: {fetchEx.Message}");
                            }
                        }));
                    }

                    // Wait for all fetch operations to complete.
                    await Task.WhenAll(fetchTasks);
                }

                // Output the distribution to the console.
                Console.WriteLine("Email domain distribution among senders:");
                foreach (KeyValuePair<string, int> kvp in domainCounts.OrderByDescending(k => k.Value))
                {
                    Console.WriteLine($"{kvp.Key}: {kvp.Value}");
                }

                // Write results to a file – ensure the directory exists and guard file I/O.
                string outputPath = "domain_distribution.txt";
                try
                {
                    string directory = Path.GetDirectoryName(Path.GetFullPath(outputPath));
                    if (!Directory.Exists(directory))
                        Directory.CreateDirectory(directory);

                    using (StreamWriter writer = new StreamWriter(outputPath, false))
                    {
                        writer.WriteLine("Email domain distribution among senders:");
                        foreach (KeyValuePair<string, int> kvp in domainCounts.OrderByDescending(k => k.Value))
                        {
                            writer.WriteLine($"{kvp.Key}: {kvp.Value}");
                        }
                    }
                }
                catch (Exception ioEx)
                {
                    Console.Error.WriteLine($"Failed to write output file: {ioEx.Message}");
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Unexpected error: {ex.Message}");
            }
        }
    }
}
