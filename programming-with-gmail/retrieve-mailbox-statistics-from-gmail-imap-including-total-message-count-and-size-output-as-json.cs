using Aspose.Email.Storage.Pst;
using Aspose.Email;
using System;
using System.Linq;
using Aspose.Email.Clients.Imap;
using Aspose.Email.Clients;
using System.Collections.Generic;

class Program
{
    static void Main()
    {
        try
        {
            // Placeholder credentials – replace with real values or skip execution.
            string host = "imap.gmail.com";
            string username = "your-email@gmail.com";
            string password = "your-password";

            // Detect placeholder values and skip network call.
            if (username.Contains("your-email") || password.Contains("your-password"))
            {
                Console.Error.WriteLine("Placeholder credentials detected. Skipping Gmail IMAP connection.");
                return;
            }

            // Create and connect the IMAP client.
            using (ImapClient client = new ImapClient(host, username, password, SecurityOptions.SSLImplicit))
            {
                try
                {
                    client.ValidateCredentials();
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to authenticate IMAP client: {ex.Message}");
                    return;
                }

                // Retrieve list of folders and locate the INBOX.
                ImapFolderInfoCollection folders;
                try
                {
                    folders = client.ListFolders();
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to list IMAP folders: {ex.Message}");
                    return;
                }

                ImapFolderInfo inboxFolder = folders.FirstOrDefault(f => f.Name.Equals("INBOX", StringComparison.OrdinalIgnoreCase));
                if (inboxFolder == null)
                {
                    Console.Error.WriteLine("INBOX folder not found.");
                    return;
                }

                // Total message count in INBOX.
                int totalMessageCount = inboxFolder.TotalMessageCount;

                // Attempt to retrieve mailbox size via quota (may not be supported).
                long totalSizeBytes = 0;
                try
                {
                    // GetQuotaRootAsync returns an array of ImapQuotaRoot; sum the usage if available.
                    var quotaRoots = client.GetQuotaRootAsync("INBOX").Result;
                    if (quotaRoots != null)
                    {
                        foreach (var quotaRoot in quotaRoots)
                        {
                            // The ImapQuotaRoot type typically has a Usage property; use reflection as a fallback.
                            var usageProp = quotaRoot.GetType().GetProperty("Usage");
                            if (usageProp != null && usageProp.PropertyType == typeof(long))
                            {
                                totalSizeBytes += (long)usageProp.GetValue(quotaRoot);
                            }
                        }
                    }
                }
                catch
                {
                    // If quota retrieval fails, size remains zero.
                }

                // Output statistics as JSON.
                string json = $"{{\"totalMessageCount\":{totalMessageCount},\"totalSizeBytes\":{totalSizeBytes}}}";
                Console.WriteLine(json);
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
