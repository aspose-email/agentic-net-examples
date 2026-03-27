using System;
using Aspose.Email;
using Aspose.Email.Clients.Google;
using Aspose.Email.PersonalInfo;

class Program
{
    static void Main()
    {
        try
        {
            // Gmail client credentials (dummy values)
            string clientId = "clientId";
            string clientSecret = "clientSecret";
            string refreshToken = "refreshToken";
            string userEmail = "user@example.com";

            // Create Gmail client
            using (IGmailClient gmailClient = GmailClient.GetInstance(clientId, clientSecret, refreshToken, userEmail))
            {
                // Retrieve all contacts
                Contact[] contacts = gmailClient.GetAllContacts();

                // Populate and process the contact collection
                foreach (Contact contact in contacts)
                {
                    string displayName = contact.DisplayName ?? "(no name)";
                    string email = "(no email)";
                    if (contact.EmailAddresses != null && contact.EmailAddresses.Count > 0)
                    {
                        email = contact.EmailAddresses[0].Address;
                    }
                    Console.WriteLine($"Name: {displayName}, Email: {email}");
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
