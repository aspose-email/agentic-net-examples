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
            // Initialize EWS client
            string mailboxUri = "https://exchange.example.com/EWS/Exchange.asmx";
            string username = "user@example.com";
            string password = "password";

            using (IEWSClient client = EWSClient.GetEWSClient(mailboxUri, username, password))
            {
                // Retrieve contacts (mailboxes) and pick the first one
                Contact[] contacts = client.GetMailboxes();
                if (contacts == null || contacts.Length == 0)
                {
                    Console.WriteLine("No contacts found.");
                    return;
                }

                Contact contact = contacts[0];

                // Update desired fields
                contact.DisplayName = "Updated Display Name";
                contact.CompanyName = "Updated Company";

                // Apply the update to the server
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
