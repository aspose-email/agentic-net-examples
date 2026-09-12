using Aspose.Email.PersonalInfo;
using Aspose.Email;
using System;
using Aspose.Email.Clients.Exchange.WebService;
using Aspose.Email.Clients.Exchange;

class Program
{
    static void Main()
    {
        try
        {
            // Connection settings – replace with actual values
            string serviceUrl = "https://ews.example.com/EWS/Exchange.asmx";
            string username = "user@example.com";
            string password = "password";

            // Create the EWS client
            using (IEWSClient service = EWSClient.GetEWSClient(serviceUrl, username, password))
            {
                // URI of the contact to delete – replace with the real contact URI
                string contactUri = "https://ews.example.com/EWS/Exchange.asmx/Contacts/12345";


                // Skip external calls when placeholder credentials are used
                if (serviceUrl.Contains("example.com") || username.Contains("example.com") || password == "password" || contactUri.Contains("example.com"))
                {
                    Console.Error.WriteLine("Placeholder credentials detected. Skipping external calls.");
                    return;
                }

                // Permanently delete the contact
                service.DeleteItem(contactUri, DeletionOptions.DeletePermanently);
                Console.WriteLine("Contact deleted permanently.");
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
