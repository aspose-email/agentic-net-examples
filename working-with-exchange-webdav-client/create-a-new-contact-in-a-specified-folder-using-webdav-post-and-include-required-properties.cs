using Aspose.Email.PersonalInfo;
using System;
using Aspose.Email;
using Aspose.Email.Clients.Exchange.Dav;
using System.Net;

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
                Console.Error.WriteLine("Placeholder credentials detected. Skipping contact creation.");
                return;
            }

            // Create and configure the Exchange WebDAV client
            using (ExchangeClient client = new ExchangeClient(mailboxUri, username, password))
            {
                // Build a new contact with required properties
                Contact newContact = new Contact();
                newContact.DisplayName = "John Doe";
                newContact.EmailAddresses.Add(new EmailAddress("john.doe@example.com", "John Doe"));

                // Create the contact in the default contacts folder
                string contactUri = client.CreateContact(newContact);
                Console.WriteLine("Contact created. URI: " + contactUri);
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine("Error: " + ex.Message);
        }
    }
}
