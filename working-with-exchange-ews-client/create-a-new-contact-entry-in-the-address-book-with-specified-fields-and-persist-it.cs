using System;
using Aspose.Email;
using Aspose.Email.PersonalInfo;
using Aspose.Email.Clients.Exchange.WebService;

class Program
{
    static void Main()
    {
        try
        {
            // Initialize EWS client (replace placeholders with real values)
            using (IEWSClient client = EWSClient.GetEWSClient("https://exchange.example.com/EWS/Exchange.asmx", "username", "password"))
            {
                // Create a new contact and set its properties
                Contact contact = new Contact();
                contact.GivenName = "John";
                contact.Surname = "Doe";
                contact.CompanyName = "Example Corp";
                contact.EmailAddresses.Add(new EmailAddress("john.doe@example.com"));

                // Persist the contact to the Exchange store
                string contactUri = client.CreateContact(contact);
                Console.WriteLine("Contact created: " + contactUri);
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine("Error: " + ex.Message);
        }
    }
}
