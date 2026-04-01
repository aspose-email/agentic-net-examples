using System;
using Aspose.Email;
using Aspose.Email.PersonalInfo;
using Aspose.Email.Clients.Exchange.WebService;

class Program
{
    static void Main(string[] args)
    {
        try
        {
            // Placeholder credentials detection – skip real network call in CI environments
            string mailboxUri = "https://exchange.example.com/EWS/Exchange.asmx";
            string username = "username";
            string password = "password";

            if (mailboxUri.Contains("example.com") || username == "username" || password == "password")
            {
                Console.WriteLine("Placeholder credentials detected. Skipping contact creation.");
                return;
            }

            // Create and configure the EWS client
            using (IEWSClient client = EWSClient.GetEWSClient(mailboxUri, username, password))
            {
                try
                {
                    // Instantiate a new contact and populate required fields
                    Contact contact = new Contact();
                    contact.DisplayName = "John Doe";
                    contact.GivenName = "John";
                    contact.Surname = "Doe";
                    contact.CompanyName = "Acme Corp";

                    // Add an email address – EmailAddress object required
                    contact.EmailAddresses.Add(new EmailAddress("john.doe@acme.com"));

                    // Add the contact to the Exchange address book
                    string contactUri = client.CreateContact(contact);
                    Console.WriteLine($"Contact created with URI: {contactUri}");
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Error creating contact: {ex.Message}");
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
