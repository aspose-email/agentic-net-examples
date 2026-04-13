using System;
using Aspose.Email;
using Aspose.Email.Clients.Exchange.WebService;

class Program
{
    static void Main()
    {
        try
        {
            // Placeholder credentials – replace with real values or skip execution.
            string mailboxUri = "https://exchange.example.com/EWS/Exchange.asmx";
            string username = "username";
            string password = "password";

            // Guard against placeholder credentials.
            if (string.Equals(username, "username", StringComparison.OrdinalIgnoreCase) ||
                string.Equals(password, "password", StringComparison.OrdinalIgnoreCase) ||
                string.IsNullOrWhiteSpace(mailboxUri))
            {
                Console.Error.WriteLine("Placeholder credentials detected. Skipping EWS operations.");
                return;
            }

            // Create the EWS client.
            using (IEWSClient client = EWSClient.GetEWSClient(mailboxUri, username, password))
            {
                try
                {
                    // Source folder where the messages currently reside (e.g., Inbox).
                    // Adjust the property if your version uses a different name.
                    string sourceFolderUri = client.MailboxInfo.InboxUri;

                    // Collection of UniqueIds of the messages to be archived.
                    string[] uniqueIds = new string[]
                    {
                        "AAMkADk3.../AQABAAADZ...=", // Example UniqueId 1
                        "AAMkADk3.../AQABAAADZ...=", // Example UniqueId 2
                        "AAMkADk3.../AQABAAADZ...="  // Example UniqueId 3
                    };

                    // Archive each message.
                    foreach (string id in uniqueIds)
                    {
                        client.ArchiveItem(sourceFolderUri, id);
                        Console.WriteLine($"Archived item with UniqueId: {id}");
                    }
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"EWS operation failed: {ex.Message}");
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
