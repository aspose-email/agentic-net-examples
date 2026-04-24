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
            // Placeholder credentials – replace with real values.
            string clientId = "YOUR_CLIENT_ID";
            string clientSecret = "YOUR_CLIENT_SECRET";
            string refreshToken = "YOUR_REFRESH_TOKEN";
            string defaultEmail = "user@example.com";

            // Guard against placeholder credentials to avoid external calls during CI.
            if (clientId.StartsWith("YOUR_") || clientSecret.StartsWith("YOUR_") ||
                refreshToken.StartsWith("YOUR_") || defaultEmail.StartsWith("user@"))
            {
                Console.Error.WriteLine("Placeholder credentials detected. Skipping Gmail operation.");
                return;
            }

            // Initialize Gmail client.
            using (IGmailClient gmailClient = GmailClient.GetInstance(clientId, clientSecret, refreshToken, defaultEmail))
            {
                // Unique resource identifier of the contact to update.
                string contactUri = "CONTACT_URI";

                // Guard against placeholder contact URI.
                if (string.IsNullOrWhiteSpace(contactUri) || contactUri.StartsWith("CONTACT_"))
                {
                    Console.Error.WriteLine("Placeholder contact URI detected. Skipping update.");
                    return;
                }

                // Fetch the existing contact.
                Contact contact;
                try
                {
                    contact = gmailClient.GetContact(contactUri);
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to retrieve contact: {ex.Message}");
                    return;
                }

                // Update the phone number.
                // Clear existing phone numbers and add the new one.
                contact.PhoneNumbers.Clear();
                PhoneNumber newPhone = new PhoneNumber
                {
                    Number = "555-1234",
                    Category = PhoneNumberCategory.Company
                };
                contact.PhoneNumbers.Add(newPhone);

                // Send the update request.
                try
                {
                    Contact updatedContact = gmailClient.UpdateContact(contact);
                    Console.WriteLine($"Contact updated successfully. New phone: {updatedContact.PhoneNumbers[0].Number}");
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to update contact: {ex.Message}");
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
