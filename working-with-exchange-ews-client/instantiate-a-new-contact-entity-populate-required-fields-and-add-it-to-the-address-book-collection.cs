using System;
using Aspose.Email;
using Aspose.Email.Clients.Exchange.WebService;
using Aspose.Email.PersonalInfo;

namespace ContactSample
{
    class Program
    {
        static void Main()
        {
            try
            {
                // Placeholder credentials – replace with real values for actual execution.
                string serviceUrl = "https://exchange.example.com/EWS/Exchange.asmx";
                string username = "user@example.com";
                string password = "password";

                // Guard: skip network call when placeholders are detected.
                if (serviceUrl.Contains("example.com") ||
                    username.Contains("example.com") ||
                    password.Equals("password", StringComparison.OrdinalIgnoreCase))
                {
                    Console.WriteLine("Placeholder credentials detected. Skipping contact creation.");
                    return;
                }

                // Create and connect the EWS client.
                using (IEWSClient client = EWSClient.GetEWSClient(serviceUrl, username, password))
                {
                    // Instantiate a new contact and populate required fields.
                    Contact contact = new Contact
                    {
                        GivenName = "John",
                        Surname = "Doe"
                    };
                    contact.EmailAddresses.Add(new EmailAddress("john.doe@example.com"));

                    // Add the contact to the address book (default contacts folder).
                    string contactId = client.CreateContact(contact);
                    Console.WriteLine("Contact created with ID: " + contactId);
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine("Error: " + ex.Message);
            }
        }
    }
}
