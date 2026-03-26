using Aspose.Email.Clients.Exchange.WebService;
using System;
using System.Net;

class Program
{
    static void Main(string[] args)
    {
        var credential = new System.Net.NetworkCredential("username", "password", "domain");

        try
        {
            string mailboxUri = "https://exchange.example.com/EWS/Exchange.asmx";
            string username = "user@example.com";
            string password = "password";

            Aspose.Email.Clients.Exchange.WebService.IEWSClient client = null;
            try
            {
                client = Aspose.Email.Clients.Exchange.WebService.EWSClient.GetEWSClient(
                    mailboxUri,
                    new NetworkCredential(username, password));
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine("Failed to create EWS client: " + ex.Message);
                return;
            }

            using (client)
            {
                Aspose.Email.PersonalInfo.Contact contact = new Aspose.Email.PersonalInfo.Contact();
                contact.DisplayName = "John Doe";
                contact.EmailAddresses.Add(
                    new Aspose.Email.PersonalInfo.EmailAddress("john.doe@example.com"));

                string contactUri = null;
                try
                {
                    contactUri = client.CreateContact(contact);
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine("Failed to create contact: " + ex.Message);
                    return;
                }

                Console.WriteLine("Contact created with URI: " + contactUri);
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine("Error: " + ex.Message);
        }
    }
}