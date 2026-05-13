using Aspose.Email.PersonalInfo;
using Aspose.Email.Clients.Exchange;
using System;
using System.Net;
using System.Collections.Generic;
using Aspose.Email;
using Aspose.Email.Clients.Exchange.WebService;

class Program
{
    static void Main()
    {
        try
        {
            // Configuration
            string ewsUrl = "https://mail.example.com/EWS/Exchange.asmx";
            string username = "user@example.com";
            string password = "password";

            // Skip external calls when placeholder credentials are used
            if (ewsUrl.Contains("example.com") || username.Contains("example.com") || password == "password")
            {
                Console.Error.WriteLine("Placeholder credentials detected. Skipping external calls.");
                return;
            }

            long sizeThresholdBytes = 100L * 1024 * 1024; // 100 MB

            // Create EWS client
            IEWSClient client = null;
            try
            {
                client = EWSClient.GetEWSClient(ewsUrl, username, password);
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Failed to create EWS client: {ex.Message}");
                return;
            }

            using (client)
            {
                // Retrieve all mailboxes (as contacts)
                Contact[] contacts;
                try
                {
                    contacts = client.GetMailboxes();
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to retrieve mailboxes: {ex.Message}");
                    return;
                }

                List<string> oversizedMailboxes = new List<string>();

                foreach (Contact contact in contacts)
                {
                    // Get primary SMTP address
                    string email = null;
                    if (contact.EmailAddresses != null && contact.EmailAddresses.Count > 0)
                    {
                        email = contact.EmailAddresses[0].Address;
                    }

                    if (string.IsNullOrEmpty(email))
                        continue;

                    // Get mailbox info for the specific email
                    ExchangeMailboxInfo mailboxInfo;
                    try
                    {
                        mailboxInfo = client.GetMailboxInfo(email);
                    }
                    catch (Exception ex)
                    {
                        Console.Error.WriteLine($"Failed to get info for {email}: {ex.Message}");
                        continue;
                    }

                    // Get total size of the mailbox using its root folder URI
                    long mailboxSize;
                    try
                    {
                        mailboxSize = client.GetMailboxSizeEx(mailboxInfo.RootUri);
                    }
                    catch (Exception ex)
                    {
                        Console.Error.WriteLine($"Failed to get size for {email}: {ex.Message}");
                        continue;
                    }

                    if (mailboxSize > sizeThresholdBytes)
                    {
                        oversizedMailboxes.Add($"{email} ({mailboxSize / (1024 * 1024)} MB)");
                    }
                }

                Console.WriteLine("Mailboxes exceeding the size threshold:");
                foreach (string entry in oversizedMailboxes)
                {
                    Console.WriteLine(entry);
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
