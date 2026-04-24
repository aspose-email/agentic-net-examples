using System;
using System.IO;
using System.Text.RegularExpressions;
using Aspose.Email;
using Aspose.Email.Clients;
using Aspose.Email.Clients.Imap;

namespace ImapSyncSample
{
    class Program
    {
        static void Main()
        {
            try
            {
                // Placeholder credentials – skip execution if they are not replaced.
                string host = "imap.example.com";
                int port = 993;
                string username = "user@example.com";
                string password = "password";

                if (host.Contains("example.com"))
                {
                    Console.Error.WriteLine("Placeholder IMAP server details detected. Skipping execution.");
                    return;
                }

                string folderToMonitor = "INBOX";
                string localCacheDir = "LocalCache";

                // Ensure the local cache directory exists.
                try
                {
                    if (!Directory.Exists(localCacheDir))
                    {
                        Directory.CreateDirectory(localCacheDir);
                    }
                }
                catch (Exception dirEx)
                {
                    Console.Error.WriteLine($"Failed to create cache directory: {dirEx.Message}");
                    return;
                }

                // Create and configure the IMAP client.
                using (ImapClient client = new ImapClient(host, port, SecurityOptions.SSLImplicit))
                {
                    try
                    {
                        client.Username = username;
                        client.Password = password;
                        client.SelectFolder(folderToMonitor);
                    }
                    catch (Exception connEx)
                    {
                        Console.Error.WriteLine($"Failed to connect or authenticate to IMAP server: {connEx.Message}");
                        return;
                    }

                    // Callback for new/deleted messages.
                    void OnMessageChanged(object sender, ImapMonitoringEventArgs e)
                    {
                        // Process newly arrived messages.
                        foreach (ImapMessageInfo info in e.NewMessages)
                        {
                            try
                            {
                                using (MailMessage message = client.FetchMessage(info.UniqueId))
                                {
                                    string subject = string.IsNullOrEmpty(message.Subject) ? "NoSubject" : message.Subject;
                                    string safeSubject = SanitizeFileName(subject);
                                    string fileName = $"{info.UniqueId}_{safeSubject}.eml";
                                    string filePath = Path.Combine(localCacheDir, fileName);

                                    try
                                    {
                                        message.Save(filePath);
                                        Console.WriteLine($"Saved new message to {filePath}");
                                    }
                                    catch (Exception saveEx)
                                    {
                                        Console.Error.WriteLine($"Failed to save message {info.UniqueId}: {saveEx.Message}");
                                    }
                                }
                            }
                            catch (Exception fetchEx)
                            {
                                Console.Error.WriteLine($"Failed to fetch message {info.UniqueId}: {fetchEx.Message}");
                            }
                        }

                        // Process deleted messages.
                        foreach (ImapMessageInfo info in e.DeletedMessages)
                        {
                            try
                            {
                                string pattern = $"{info.UniqueId}_*.eml";
                                foreach (string file in Directory.GetFiles(localCacheDir, pattern))
                                {
                                    File.Delete(file);
                                    Console.WriteLine($"Deleted local cache file {file}");
                                }
                            }
                            catch (Exception delEx)
                            {
                                Console.Error.WriteLine($"Failed to delete local cache for message {info.UniqueId}: {delEx.Message}");
                            }
                        }
                    }

                    // Callback for monitoring errors.
                    void OnMonitoringError(object sender, ImapMonitoringErrorEventArgs err)
                    {
                        Console.Error.WriteLine($"Monitoring error: {err.Error?.Message}");
                    }

                    // Start monitoring the folder.
                    try
                    {
                        client.StartMonitoring(OnMessageChanged, OnMonitoringError, folderToMonitor);
                        Console.WriteLine("Monitoring started. Press Enter to stop...");
                        Console.ReadLine();
                        client.StopMonitoring(folderToMonitor);
                    }
                    catch (Exception monEx)
                    {
                        Console.Error.WriteLine($"Failed to start monitoring: {monEx.Message}");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Unexpected error: {ex.Message}");
            }
        }

        // Helper to make a filename safe for the file system.
        private static string SanitizeFileName(string name)
        {
            string invalidChars = new string(Path.GetInvalidFileNameChars()) + new string(Path.GetInvalidPathChars());
            string escaped = Regex.Replace(name, $"[{Regex.Escape(invalidChars)}]+", "_");
            return escaped;
        }
    }
}
