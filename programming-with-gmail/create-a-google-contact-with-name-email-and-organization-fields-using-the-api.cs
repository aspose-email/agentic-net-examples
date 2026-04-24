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
            // Placeholder credentials – replace with real values or skip execution.
            string clientId = "YOUR_CLIENT_ID";
            string clientSecret = "YOUR_CLIENT_SECRET";
            string refreshToken = "YOUR_REFRESH_TOKEN";
            string defaultEmail = "user@example.com";

            if (clientId.StartsWith("YOUR_") ||
                clientSecret.StartsWith("YOUR_") ||
                refreshToken.StartsWith("YOUR_") ||
                defaultEmail.StartsWith("user@"))
            {
                Console.Error.WriteLine("Placeholder credentials detected. Skipping Gmail contact creation.");
                return;
            }

            // Create Gmail client.
            IGmailClient gmailClient = null;
            try
            {
                gmailClient = GmailClient.GetInstance(clientId, clientSecret, refreshToken, defaultEmail);
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Failed to create Gmail client: {ex.Message}");
                return;
            }

            using (gmailClient)
            {
                // Build a new contact.
                Contact contact = new Contact
                {
                    DisplayName = "John Doe",
                    CompanyName = "Acme Corp"
                };
                contact.EmailAddresses.Add(new EmailAddress("john.doe@acme.com"));

                // Create the contact in Gmail.
                string contactUri = null;
                try
                {
                    contactUri = gmailClient.CreateContact(contact);
                    Console.WriteLine($"Contact created successfully. URI: {contactUri}");
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to create contact: {ex.Message}");
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
