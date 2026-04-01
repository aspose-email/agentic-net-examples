using Aspose.Email.PersonalInfo;
using System;
using System.Net;
using Aspose.Email.Clients.Exchange.WebService;

class Program
{
    static void Main()
    {
        try
        {
            // Placeholder credentials and service URL
            string serviceUrl = "https://exchange.example.com/EWS/Exchange.asmx";
            string username = "username";
            string password = "password";

            // Detect placeholder values and skip actual network call
            if (serviceUrl.Contains("example.com") || username == "username")
            {
                Console.WriteLine("Placeholder credentials detected. Skipping operation.");
                return;
            }

            // Create the EWS client
            using (IEWSClient service = EWSClient.GetEWSClient(serviceUrl, username, password))
            {
                try
                {
                    // Identifier of the contact to be deleted
                    string contactUri = "contact-uri-to-delete";

                    // Permanently delete the contact using the static DeletePermanently option
                    service.DeleteItem(contactUri, DeletionOptions.DeletePermanently);

                    Console.WriteLine("Contact deleted permanently.");
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Error during deletion: {ex.Message}");
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
