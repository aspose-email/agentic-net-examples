using System;
using Aspose.Email;
using Aspose.Email.PersonalInfo;
using Aspose.Email.Clients.Google;

class Program
{
    static void Main()
    {
        try
        {
            // Placeholder credentials – replace with real values for actual execution.
            string accessToken = "YOUR_ACCESS_TOKEN";
            string defaultEmail = "user@example.com";

            // Skip execution when placeholder credentials are detected.
            if (accessToken.StartsWith("YOUR_"))
            {
                Console.Error.WriteLine("Placeholder credentials detected. Skipping Gmail operations.");
                return;
            }

            // Create Gmail client instance.
            using (IGmailClient gmailClient = GmailClient.GetInstance(accessToken, defaultEmail))
            {
                // Retrieve all contacts.
                Contact[] contacts = gmailClient.GetAllContacts();

                foreach (Contact contact in contacts)
                {
                    // Ensure the contact has at least one email address.
                    if (contact.EmailAddresses == null || contact.EmailAddresses.Count == 0)
                    {
                        // Build a default email using the contact's given name or a generic user name.
                        string email = $"{contact.GivenName ?? "user"}@example.com";

                        // Add the default email address to the contact.
                        contact.EmailAddresses.Add(new EmailAddress(email));

                        // Update the contact on Gmail.
                        gmailClient.UpdateContact(contact);

                        Console.WriteLine($"Updated contact '{contact.GivenName}' with email {email}");
                    }
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
