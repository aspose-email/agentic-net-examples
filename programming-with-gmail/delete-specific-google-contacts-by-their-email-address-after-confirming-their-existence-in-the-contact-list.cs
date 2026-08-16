using Aspose.Email;
using Aspose.Email.Clients.Google;
using Aspose.Email.PersonalInfo;
using System;
using System.Collections.Generic;

class Program
{
    static void Main()
    {
        try
        {
            // Placeholder credentials – replace with real values.
            string clientId = "YOUR_CLIENT_ID";
            string clientSecret = "YOUR_CLIENT_SECRET";
            string refreshToken = "YOUR_REFRESH_TOKEN";

            // If placeholders are still present, skip execution to avoid external calls.
            if (clientId.Contains("YOUR_") || clientSecret.Contains("YOUR_") || refreshToken.Contains("YOUR_"))
            {
                Console.Error.WriteLine("Placeholder credentials detected. Skipping Gmail operations.");
                return;
            }

            // Initialize Gmail client. Pass null for proxy (no proxy).
            using (IGmailClient gmailClient = GmailClient.GetInstance(clientId, null, clientSecret, refreshToken))
            {
                // List of email addresses to delete.
                List<string> emailsToDelete = new List<string>
                {
                    "example1@example.com",
                    "example2@example.com"
                };

                // Fetch all contacts.
                Contact[] allContacts = gmailClient.GetAllContacts();

                foreach (Contact contact in allContacts)
                {
                    // Ensure the contact has at least one email address.
                    if (contact.EmailAddresses == null || contact.EmailAddresses.Count == 0)
                        continue;

                    // Check each email address of the contact.
                    foreach (EmailAddress emailAddr in contact.EmailAddresses)
                    {
                        if (emailsToDelete.Contains(emailAddr.Address, StringComparer.OrdinalIgnoreCase))
                        {
                            // Delete the contact using its identifier.
                            gmailClient.DeleteContact(contact.Id.ToString());
                            Console.WriteLine($"Deleted contact with email: {emailAddr.Address}");
                            break; // Move to next contact after deletion.
                        }
                    }
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
