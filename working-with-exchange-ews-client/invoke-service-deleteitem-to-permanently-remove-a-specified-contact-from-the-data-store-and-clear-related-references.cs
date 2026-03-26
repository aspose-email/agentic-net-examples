using System;
using System.Net;
using Aspose.Email.Clients.Exchange.WebService;

class Program
{
    static void Main()
    {
        var credential = new System.Net.NetworkCredential("username", "password", "domain");

        try
        {
            // Service URL and credentials for EWS
            string serviceUrl = "https://exchange.example.com/EWS/Exchange.asmx";
            NetworkCredential credentials = new NetworkCredential("username", "password");

            // Create the EWS client
            using (IEWSClient service = EWSClient.GetEWSClient(serviceUrl, credentials))
            {
                // URI of the contact to be deleted
                string contactUri = "https://exchange.example.com/EWS/Contacts/ContactId";

                // Permanently delete the contact
                service.DeleteItem(contactUri, DeletionOptions.DeletePermanently);

                Console.WriteLine("Contact deleted permanently.");
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine(ex.Message);
        }
    }
}