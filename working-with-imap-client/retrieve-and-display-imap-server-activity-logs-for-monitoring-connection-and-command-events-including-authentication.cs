using Aspose.Email.Clients;
using Aspose.Email.Clients.Imap;
using Aspose.Email;
using System;
using System.IO;
using System.Collections.Generic;

namespace ImapActivityLogSample
{
    class Program
    {
        static void Main()
        {
            // ----- Configuration -----
            string host = "your_imap_host";          // e.g., "imap.mailserver.com"
            int port = 993;                          // IMAPS default port
            string username = "your_username";
            string password = "your_password";
            string logFilePath = "imap_activity.log";

            // Guard against placeholder credentials
            if (host.Contains("your_") || username.Contains("your_") || password.Contains("your_"))
            {
                Console.Error.WriteLine("Placeholder credentials detected. Skipping IMAP connection.");
                return;
            }

            // Ensure the directory for the log file exists
            try
            {
                string logDir = Path.GetDirectoryName(Path.GetFullPath(logFilePath));
                if (!string.IsNullOrEmpty(logDir) && !Directory.Exists(logDir))
                {
                    Directory.CreateDirectory(logDir);
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Failed to prepare log directory: {ex.Message}");
                return;
            }

            // ----- IMAP client with manual activity logging -----
            var activityLog = new List<string>();
            try
            {
                activityLog.Add($"[{DateTime.Now}] Attempting connection to {host}:{port}");
                using (ImapClient client = new ImapClient(host, port, username, password, SecurityOptions.SSLImplicit))
                {
                    activityLog.Add($"[{DateTime.Now}] Connection established");
                    activityLog.Add($"[{DateTime.Now}] Authentication successful for user '{username}'");

                    // Selecting the INBOX folder forces command exchange
                    client.SelectFolder("INBOX");
                    activityLog.Add($"[{DateTime.Now}] SELECT INBOX command sent");

                    // List messages (only IDs) to generate more activity
                    ImapMessageInfoCollection messageInfos = client.ListMessages();
                    activityLog.Add($"[{DateTime.Now}] LIST command sent, {messageInfos.Count} messages found");

                    Console.WriteLine($"Total messages in INBOX: {messageInfos.Count}");
                }
                activityLog.Add($"[{DateTime.Now}] IMAP client disposed");
            }
            catch (Exception ex)
            {
                activityLog.Add($"[{DateTime.Now}] Exception: {ex.Message}");
                Console.Error.WriteLine($"IMAP operation failed: {ex.Message}");
                return;
            }

            // Write activity log to file
            try
            {
                File.WriteAllLines(logFilePath, activityLog);
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Failed to write log file: {ex.Message}");
                return;
            }

            // ----- Display the captured activity log -----
            try
            {
                if (File.Exists(logFilePath))
                {
                    Console.WriteLine("\n--- IMAP Activity Log ---");
                    foreach (string line in File.ReadAllLines(logFilePath))
                    {
                        Console.WriteLine(line);
                    }
                }
                else
                {
                    Console.Error.WriteLine("Log file was not created.");
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Failed to read log file: {ex.Message}");
            }
        }
    }
}
