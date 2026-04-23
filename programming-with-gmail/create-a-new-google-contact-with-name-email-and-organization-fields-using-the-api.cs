using Aspose.Email.PersonalInfo;
using System;
using Aspose.Email;
using Aspose.Email.Clients.Google;

class Program
{
    static void Main()
    {
        try
        {
            // Placeholder credentials – replace with real values to run against Gmail.
            string accessToken = "YOUR_ACCESS_TOKEN";
            string defaultEmail = "user@example.com";

            // Skip execution when placeholders are used.
            if (accessToken == "YOUR_ACCESS_TOKEN")
            {
                Console.Error.WriteLine("Please provide a valid OAuth access token and default email.");
                return;
            }

            // Create Gmail client and ensure proper disposal.
            try
            {
                using (IGmailClient gmailClient = GmailClient.GetInstance(accessToken, defaultEmail))
                {
                    // Build a new contact.
                    Contact newContact = new Contact
                    {
                        DisplayName = "John Doe",
                        CompanyName = "Acme Corp"
                    };
                    newContact.EmailAddresses.Add(new EmailAddress("john.doe@example.com"));

                    // Create the contact in Gmail.
                    string contactUri = gmailClient.CreateContact(newContact);
                    Console.WriteLine($"Contact created: {contactUri}");
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Gmail operation failed: {ex.Message}");
                return;
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
