using System;
using System.Net;
using Aspose.Email;
using Aspose.Email.Clients.Exchange.Dav;
using Aspose.Email.Mapi;

class Program
{
    static void Main()
    {
        try
        {
            // Placeholder connection details – replace with real values.
            string exchangeUri = "https://exchange.example.com/ews/Exchange.asmx";
            string username = "username";
            string password = "password";

            // Skip execution when placeholders are detected to avoid external calls during CI.
            if (exchangeUri.Contains("example.com") || username == "username" || password == "password")
            {
                Console.Error.WriteLine("Placeholder credentials detected. Skipping contact cleanup.");
                return;
            }

            // Define the cutoff date. Contacts created before this date will be deleted.
            DateTime cutoffDate = new DateTime(2022, 1, 1);

            // Create and use the Exchange client.
            try
            {
                using (ExchangeClient client = new ExchangeClient(exchangeUri, new NetworkCredential(username, password)))
                {
                    // Retrieve contacts from the default contacts folder (use appropriate folder URI if known).
                    MapiContact[] contacts = client.ListContacts("contacts");

                    // MAPI property tag for PR_CREATION_TIME (PT_SYSTIME).
                    const long CreationTimeTag = 0x30070040;

                    foreach (MapiContact contact in contacts)
                    {
                        DateTime creationTime = DateTime.MinValue;
                        bool hasCreationTime = contact.TryGetPropertyDateTime(CreationTimeTag, ref creationTime);

                        if (hasCreationTime && creationTime < cutoffDate)
                        {
                            try
                            {
                                client.DeleteContact(contact);
                                Console.WriteLine($"Deleted contact: {contact.NameInfo.DisplayName} (Created: {creationTime})");
                            }
                            catch (Exception ex)
                            {
                                Console.Error.WriteLine($"Failed to delete contact '{contact.NameInfo.DisplayName}': {ex.Message}");
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Exchange client error: {ex.Message}");
                return;
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
