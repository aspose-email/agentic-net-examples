using System;
using System.Net;
using Aspose.Email;
using Aspose.Email.Clients.Exchange.WebService;
using Aspose.Email.PersonalInfo;

class Program
{
    static void Main()
    {
        try
        {
            // Exchange server URI and credentials (replace with real values)
            string mailboxUri = "https://exchange.example.com/EWS/Exchange.asmx";
            string username = "user@example.com";
            string password = "password";

            // Create the EWS client
            using (IEWSClient client = EWSClient.GetEWSClient(mailboxUri, username, password))
            {
                // Build a new contact
                Contact newContact = new Contact();
                newContact.DisplayName = "John Doe";
                newContact.EmailAddresses.Add(new EmailAddress("john.doe@example.com"));
                newContact.PhoneNumbers.Add(new PhoneNumber { Number = "+1234567890" });

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
