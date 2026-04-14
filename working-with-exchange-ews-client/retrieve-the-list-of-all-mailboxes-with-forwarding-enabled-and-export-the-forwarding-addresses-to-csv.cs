using Aspose.Email.PersonalInfo;
using System;
using System.IO;
using System.Net;
using System.Reflection;
using Aspose.Email;
using Aspose.Email.Clients.Exchange.WebService;

class Program
{
    static void Main()
    {
        try
        {
            // Connection parameters – replace with actual values
            string mailboxUri = "https://exchange.example.com/EWS/Exchange.asmx";
            string username = "user@example.com";
            string password = "password";

            // Create EWS client with safety guard
            using (IEWSClient client = EWSClient.GetEWSClient(mailboxUri, username, password))
            {
                // Retrieve all mailboxes (contacts) from the GAL
                Contact[] contacts = client.GetMailboxes();

                // Prepare CSV output path
                string csvPath = "forwarding_addresses.csv";

                // Skip external calls when placeholder credentials are used
                if (mailboxUri.Contains("example.com") || username.Contains("example.com") || password == "password")
                {
                    Console.Error.WriteLine("Placeholder credentials detected. Skipping external calls.");
                    return;
                }

                string directory = Path.GetDirectoryName(csvPath);
                if (!string.IsNullOrEmpty(directory) && !Directory.Exists(directory))
                {
                    Directory.CreateDirectory(directory);
                }

                // Write forwarding information to CSV
                using (StreamWriter writer = new StreamWriter(csvPath, false))
                {
                    writer.WriteLine("Mailbox,ForwardingAddress");

                    foreach (Contact contact in contacts)
                    {
                        // Get primary SMTP address of the mailbox
                        string mailboxAddress = string.Empty;
                        if (contact.EmailAddresses != null && contact.EmailAddresses.Count > 0)
                        {
                            mailboxAddress = contact.EmailAddresses[0].Address;
                        }

                        // Attempt to read the forwarding address via reflection
                        // (the exact member name may vary between SDK versions)
                        string forwardingAddress = string.Empty;
                        PropertyInfo forwardingProp = contact.GetType().GetProperty("ForwardingAddress");
                        if (forwardingProp != null)
                        {
                            forwardingAddress = forwardingProp.GetValue(contact) as string;
                        }

                        // Export only mailboxes that have forwarding configured
                        if (!string.IsNullOrEmpty(forwardingAddress))
                        {
                            writer.WriteLine($"{mailboxAddress},{forwardingAddress}");
                        }
                    }
                }

                Console.WriteLine("Forwarding addresses exported to " + csvPath);
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine("Error: " + ex.Message);
        }
    }
}
