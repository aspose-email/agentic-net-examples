using System;
using Aspose.Email;
using Aspose.Email.Mapi;
using Aspose.Email.Clients.Exchange.Dav;

class Program
{
    static void Main()
    {
        try
        {
            // Placeholder connection details
            string mailboxUri = "https://exchange.example.com/ews/Exchange.asmx";
            string username = "user@example.com";
            string password = "password";

            // Skip execution when placeholders are detected
            if (mailboxUri.Contains("example.com"))
            {
                Console.Error.WriteLine("Placeholder credentials detected. Skipping external call.");
                return;
            }

            // Connect to Exchange using WebDAV client
            using (ExchangeClient client = new ExchangeClient(mailboxUri, username, password))
            {
                // Retrieve contacts from the default contacts folder
                string contactsFolderUri = "contacts";
                MapiContact[] contacts = client.ListContacts(contactsFolderUri);

                // Display a read‑only view of each contact
                foreach (MapiContact contact in contacts)
                {
                    string displayName = contact.NameInfo?.DisplayName ?? "(no name)";
                    string email = contact.ElectronicAddresses?.Email1?.EmailAddress ?? "(no email)";
                    Console.WriteLine($"Name: {displayName}, Email: {email}");
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine(ex.Message);
        }
    }
}
