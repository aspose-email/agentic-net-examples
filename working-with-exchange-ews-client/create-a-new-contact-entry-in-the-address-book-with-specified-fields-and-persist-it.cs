using System;
using System.Net;
using Aspose.Email;
using Aspose.Email.PersonalInfo;
using Aspose.Email.Clients.Exchange.WebService;

class Program
{
    static void Main()
    {
        var credential = new System.Net.NetworkCredential("username", "password", "domain");

        try
        {
            // Mailbox URI and credentials (replace with real values)
            string mailboxUri = "https://exchange.example.com/EWS/Exchange.asmx";
            string username = "user@example.com";
            string password = "password";

            NetworkCredential credentials = new NetworkCredential(username, password);

            // Initialize EWS client
            using (IEWSClient client = EWSClient.GetEWSClient(mailboxUri, credentials))
            {
                // Create a new contact
                Contact newContact = new Contact();
                newContact.DisplayName = "John Doe";
                newContact.EmailAddresses.Add(new EmailAddress("john.doe@example.com"));

                // Add contact to the Exchange store
                string contactUri = client.CreateContact(newContact);

                Console.WriteLine("Contact created successfully. URI: " + contactUri);
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine("Error: " + ex.Message);
        }
    }
}