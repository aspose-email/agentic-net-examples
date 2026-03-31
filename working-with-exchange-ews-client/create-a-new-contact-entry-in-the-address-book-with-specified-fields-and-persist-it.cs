using System;
using System.Net;
using Aspose.Email;
using Aspose.Email.PersonalInfo;
using Aspose.Email.Clients.Exchange.WebService;

class Program
{
    static void Main()
    {
        try
        {
            string mailboxUri = "https://exchange.example.com/EWS/Exchange.asmx";
            string username = "username";
            string password = "password";

            // Skip execution when placeholder credentials are detected
            if (mailboxUri.Contains("example.com") || username == "username")
            {
                Console.WriteLine("Placeholder credentials detected. Skipping contact creation.");
                return;
            }

            // Create and connect the EWS client
            using (IEWSClient client = EWSClient.GetEWSClient(mailboxUri, username, password))
            {
                // Build a new contact
                Contact contact = new Contact
                {
                    DisplayName = "John Doe",
                    GivenName = "John",
                    Surname = "Doe",
                    CompanyName = "Acme Corp"
                };
                contact.EmailAddresses.Add(new EmailAddress("john.doe@acme.com", "John Doe"));

                // Create the contact on the Exchange server
                string contactUri = client.CreateContact(contact);
                Console.WriteLine($"Contact created. URI: {contactUri}");
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
