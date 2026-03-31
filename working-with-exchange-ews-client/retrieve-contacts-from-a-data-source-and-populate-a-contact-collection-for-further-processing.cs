using System;
using System.Collections.Generic;
using Aspose.Email;
using Aspose.Email.PersonalInfo;
using Aspose.Email.Clients.Google;

class Program
{
    static void Main()
    {
        try
        {
            // Placeholder credentials – replace with real values for actual use
            string clientId = "clientId";
            string clientSecret = "clientSecret";
            string refreshToken = "refreshToken";
            string defaultEmail = "user@example.com";

            // Guard against executing network calls with placeholder credentials
            if (clientId == "clientId")
            {
                Console.WriteLine("Placeholder credentials detected. Skipping Gmail operation.");
                return;
            }

            // Initialize Gmail client
            using (IGmailClient gmailClient = GmailClient.GetInstance(clientId, clientSecret, refreshToken, defaultEmail))
            {
                // Retrieve all contacts
                Contact[] contactsArray = gmailClient.GetAllContacts();

                // Populate a strongly‑typed collection for further processing
                List<Contact> contacts = new List<Contact>(contactsArray);

                // Example processing: output basic contact info
                foreach (Contact contact in contacts)
                {
                    string email = contact.EmailAddresses.Count > 0 ? contact.EmailAddresses[0].Address : "N/A";
                    Console.WriteLine($"Name: {contact.GivenName} {contact.Surname}, Email: {email}");
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine(ex.Message);
        }
    }
}
