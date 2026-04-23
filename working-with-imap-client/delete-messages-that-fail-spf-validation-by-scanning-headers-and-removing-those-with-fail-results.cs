using System;
using System.Collections.Generic;
using Aspose.Email;
using Aspose.Email.Clients.Imap;
using Aspose.Email.Clients;
using Aspose.Email.Clients.Imap.Models;

class Program
{
    static void Main()
    {
        try
        {
            // Placeholder connection settings
            string host = "imap.example.com";
            int port = 993;
            string username = "user@example.com";
            string password = "password";

            // Skip execution when placeholder credentials are used
            if (host.Contains("example.com"))
            {
                Console.Error.WriteLine("Placeholder IMAP server detected. Skipping execution.");
                return;
            }

            // Create and connect the IMAP client
            using (ImapClient client = new ImapClient(host, port, SecurityOptions.SSLImplicit))
            {
                try
                {
                    client.Username = username;
                    client.Password = password;
                    client.ValidateCredentials();
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to connect or authenticate: {ex.Message}");
                    return;
                }

                // Select the INBOX folder
                try
                {
                    client.SelectFolder("INBOX");
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to select folder: {ex.Message}");
                    return;
                }

                // Retrieve list of messages in the folder
                ImapMessageInfoCollection messageInfos;
                try
                {
                    messageInfos = client.ListMessages();
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to list messages: {ex.Message}");
                    return;
                }

                List<ImapMessageInfo> messagesToDelete = new List<ImapMessageInfo>();

                // Scan each message for SPF failure
                foreach (ImapMessageInfo info in messageInfos)
                {
                    MailMessage message = null;
                    try
                    {
                        message = client.FetchMessage(info.UniqueId);
                    }
                    catch (Exception ex)
                    {
                        Console.Error.WriteLine($"Failed to fetch message UID {info.UniqueId}: {ex.Message}");
                        continue;
                    }

                    // Check relevant headers for "fail"
                    bool spfFailed = false;
                    foreach (string headerKey in message.Headers.Keys)
                    {
                        string headerValue = message.Headers[headerKey];
                        if (headerValue != null && headerValue.IndexOf("fail", StringComparison.OrdinalIgnoreCase) >= 0)
                        {
                            // Look for typical SPF headers
                            if (headerKey.Equals("Received-SPF", StringComparison.OrdinalIgnoreCase) ||
                                headerKey.Equals("Authentication-Results", StringComparison.OrdinalIgnoreCase))
                            {
                                spfFailed = true;
                                break;
                            }
                        }
                    }

                    if (spfFailed)
                    {
                        messagesToDelete.Add(info);
                    }

                    // Dispose the fetched message
                    if (message != null)
                    {
                        message.Dispose();
                    }
                }

                // Delete messages that failed SPF validation
                if (messagesToDelete.Count > 0)
                {
                    try
                    {
                        client.DeleteMessages(messagesToDelete, true);
                        Console.WriteLine($"{messagesToDelete.Count} message(s) deleted due to SPF failure.");
                    }
                    catch (Exception ex)
                    {
                        Console.Error.WriteLine($"Failed to delete messages: {ex.Message}");
                    }
                }
                else
                {
                    Console.WriteLine("No messages with SPF failure found.");
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
