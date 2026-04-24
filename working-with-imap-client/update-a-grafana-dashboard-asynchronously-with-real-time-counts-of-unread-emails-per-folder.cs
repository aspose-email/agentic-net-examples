using Aspose.Email.Storage.Pst;
using System;
using System.Collections.Generic;
using System.Threading;
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
            // Placeholder credentials – replace with real values when running in production.
            string host = "imap.example.com";
            string username = "user@example.com";
            string password = "password";

            // Guard against placeholder credentials to avoid unwanted network calls during CI.
            if (host.Contains("example.com") || username.Contains("example.com") || password == "password")
            {
                Console.WriteLine("Placeholder credentials detected. Skipping real IMAP connection.");
                return;
            }

            // Create and connect the IMAP client inside a using block to ensure disposal.
            using (ImapClient client = new ImapClient(host, username, password, SecurityOptions.Auto))
            {
                // Validate credentials by selecting the INBOX folder (lightweight operation).
                try
                {
                    await client.SelectFolderAsync("INBOX");
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to validate IMAP credentials: {ex.Message}");
                    return;
                }

                // Define the monitoring callback.
                void OnMessageChanged(object sender, ImapMonitoringEventArgs e)
                {
                    // For each folder, retrieve its info and output the new message count.
                    // This simulates sending the data to a Grafana dashboard.
                    Task.Run(async () =>
                    {
                        try
                        {
                            // Get list of all folders.
                            ImapFolderInfoCollection folders = await client.ListFoldersAsync();

                            foreach (ImapFolderInfo folder in folders)
                            {
                                // Retrieve fresh folder info to get the latest counts.
                                ImapFolderInfo info = await client.GetFolderInfoAsync(folder.Name);
                                int newCount = info.NewMessageCount;
                                Console.WriteLine($"Folder: {folder.Name}, New Messages: {newCount}");
                                // Here you would send the count to Grafana via its HTTP API.
                                // Skipped per rules – placeholder for actual Grafana update.
                            }
                        }
                        catch (Exception ex)
                        {
                            Console.Error.WriteLine($"Error while processing folder counts: {ex.Message}");
                        }
                    });
                }

                // Define the error callback.
                void OnMonitoringError(object sender, ImapMonitoringErrorEventArgs e)
                {
                    // Use the Error property (not a non‑existent Exception property).
                    Console.Error.WriteLine($"IMAP monitoring error: {e.Error?.Message ?? "Unknown error"}");
                }

                // Start monitoring the INBOX folder asynchronously.
                try
                {
                    await client.StartMonitoringAsync(OnMessageChanged, OnMonitoringError, "INBOX");
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to start IMAP monitoring: {ex.Message}");
                    return;
                }

                // Keep the monitoring alive for a demo period (e.g., 30 seconds).
                Console.WriteLine("Monitoring started. Press Ctrl+C to exit.");
                await Task.Delay(TimeSpan.FromSeconds(30));

                // Stop monitoring before exiting.
                try
                {
                    await client.StopMonitoringAsync("INBOX");
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to stop IMAP monitoring: {ex.Message}");
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unhandled exception: {ex.Message}");
        }
    }
}
