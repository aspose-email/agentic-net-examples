using System;
using System.Net;
using Aspose.Email;
using Aspose.Email.Clients.Exchange.WebService;

class Program
{
    static void Main()
    {
        try
        {
            // Initialize the EWS client with placeholder credentials.
            // Replace the placeholders with actual server URI, username, and password.
            string serviceUrl = "https://exchange.example.com/EWS/Exchange.asmx";
            string username = "user@example.com";
            string password = "password";

            using (IEWSClient client = EWSClient.GetEWSClient(serviceUrl, new NetworkCredential(username, password)))
            {
                // The URI of the contact to be permanently deleted.
                // Replace with the actual contact URI obtained from a prior operation.
                string contactUri = "https://exchange.example.com/EWS/Contacts/ContactId";

                // Delete the contact permanently.
                client.DeleteItem(contactUri, DeletionOptions.DeletePermanently);
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
