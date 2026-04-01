using Aspose.Email.PersonalInfo;
using System;
using Aspose.Email.Clients.Exchange.WebService;
using Aspose.Email.Mapi;

class Program
{
    static void Main()
    {
        try
        {
            // Placeholder credentials – skip actual network call in CI environments
            string serviceUrl = "https://example.com/EWS";
            string username = "username";
            string password = "password";

            if (serviceUrl.Contains("example.com"))
            {
                Console.WriteLine("Placeholder credentials detected. Skipping execution.");
                return;
            }

            // Create the EWS client
            using (IEWSClient client = EWSClient.GetEWSClient(serviceUrl, username, password))
            {
                // Prepare a contact with updated information
                MapiContact contact = new MapiContact();
                contact.NameInfo.DisplayName = "John Doe";
                contact.ElectronicAddresses.Email1.EmailAddress = "john.doe@example.com";

                // Update the contact in the Exchange store
                client.UpdateContact(contact);
                Console.WriteLine("Contact updated successfully.");
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine(ex.Message);
        }
    }
}
