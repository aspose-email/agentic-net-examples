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
            // Initialize network credentials
            NetworkCredential credentials = new NetworkCredential("username", "password");

            // Create an EWS client instance
            using (IEWSClient client = EWSClient.GetEWSClient("https://exchange.example.com/EWS/Exchange.asmx", credentials))
            {
                // Identifier of the contact to be updated (replace with a real ID)
                string contactId = "contact-id-placeholder";

                // Retrieve the existing contact from the server
                Contact contact = client.GetContact(contactId);

                // Modify desired fields of the contact
                contact.DisplayName = "Updated Name";
                contact.EmailAddresses.Clear();
                contact.EmailAddresses.Add(new EmailAddress("updated@example.com"));

                // Persist the changes back to the Exchange store
                client.UpdateContact(contact);

                Console.WriteLine("Contact updated successfully.");
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
