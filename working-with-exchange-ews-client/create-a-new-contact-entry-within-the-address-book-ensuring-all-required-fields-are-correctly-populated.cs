using System;
using System.Net;
using Aspose.Email.Clients.Exchange.WebService;

class Program
{
    static void Main()
    {
        try
        {
            // Initialize the Exchange WebDAV client (replace placeholders with real values)
            using (Aspose.Email.Clients.Exchange.Dav.ExchangeClient client = new Aspose.Email.Clients.Exchange.Dav.ExchangeClient("https://exchange.example.com/EWS/Exchange.asmx", "username", "password"))
            {
                // Create a new contact and populate required fields
                Aspose.Email.PersonalInfo.Contact contact = new Aspose.Email.PersonalInfo.Contact();
                contact.DisplayName = "John Doe";
                contact.EmailAddresses.Add(new Aspose.Email.PersonalInfo.EmailAddress("john.doe@example.com"));

                // Add the contact to the address book
                string contactUri = client.CreateContact(contact);
                Console.WriteLine("Contact created at URI: " + contactUri);
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine("Error: " + ex.Message);
        }
    }
}