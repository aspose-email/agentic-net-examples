using Aspose.Email.PersonalInfo;
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
            // EWS connection parameters
            string mailboxUri = "https://example.com/EWS/Exchange.asmx";
            string username = "user@example.com";
            string password = "password";

            // Create EWS client
            using (IEWSClient client = EWSClient.GetEWSClient(mailboxUri, username, password))
            {
                // Identifier of the contact to be updated
                string contactId = "contact-id";


                // Skip external calls when placeholder credentials are used
                if (mailboxUri.Contains("example.com") || username.Contains("example.com") || password == "password")
                {
                    Console.Error.WriteLine("Placeholder credentials detected. Skipping external calls.");
                    return;
                }

                // Retrieve the existing contact
                Contact contact = client.GetContact(contactId);

                // Add a new email address to the contact
                contact.EmailAddresses.Add(new EmailAddress("newaddress@example.com", "New Address"));

                // Update the contact in the Exchange store
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
