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
            // Initialize the Gmail client with placeholder credentials
            IGmailClient gmailClient = GmailClient.GetInstance(
                "clientId",
                "clientSecret",
                "refreshToken",
                "user@example.com");

            // Retrieve all contacts from the Gmail account
            Contact[] contacts = gmailClient.GetAllContacts();

            // Example processing: output each contact's display name and primary email address
            foreach (Contact contact in contacts)
            {
                string email = contact.EmailAddresses.Count > 0 ? contact.EmailAddresses[0].Address : "No email";
                Console.WriteLine($"Name: {contact.DisplayName}, Email: {email}");
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
