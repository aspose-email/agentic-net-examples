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
            // Initialize network credentials
            NetworkCredential credential = new NetworkCredential("username", "password");

            // Create the EWS client using the factory method
            using (IEWSClient service = EWSClient.GetEWSClient("https://exchange.example.com/EWS/Exchange.asmx", credential))
            {
                // Identifier of the contact to be removed
                string contactUri = "contact-id-or-uri";

                // Permanently delete the contact
                service.DeleteItem(contactUri, DeletionOptions.DeletePermanently);

                Console.WriteLine("Contact deleted successfully.");
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
