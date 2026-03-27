using System;
using System.Net;
using Aspose.Email;
using Aspose.Email.Clients.Exchange.WebService;
using Aspose.Email.Mapi;

class Program
{
    static void Main()
    {
        var credential = new System.Net.NetworkCredential("username", "password", "domain");

        try
        {
            // Exchange Web Services endpoint and credentials (replace with real values)
            string mailboxUri = "https://exchange.example.com/EWS/Exchange.asmx";
            string username = "username";
            string password = "password";

            // Create the EWS client
            using (IEWSClient client = EWSClient.GetEWSClient(mailboxUri, username, password))
            {
                // Create a MAPI contact
                using (MapiContact contact = new MapiContact("John Doe", "john.doe@example.com", "Acme Corp", "123-456-7890"))
                {
                    // Upload the contact to the server using CreateItem
                    string contactUri = client.CreateItem(contact);
                    Console.WriteLine("Contact created. Uri: " + contactUri);
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine(ex.Message);
        }
    }
}