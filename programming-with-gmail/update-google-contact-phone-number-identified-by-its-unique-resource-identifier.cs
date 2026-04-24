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
            string accessToken = "YOUR_ACCESS_TOKEN";
            string defaultEmail = "user@example.com";
            string contactUri = "YOUR_CONTACT_URI";

            // Skip actual network call when placeholders are detected.
            if (accessToken.StartsWith("YOUR_") || defaultEmail.StartsWith("user@") || contactUri.StartsWith("YOUR_"))
            {
                Console.Error.WriteLine("Placeholder credentials detected. Skipping Gmail operation.");
                return;
            }

            // Create Gmail client.
            using (IGmailClient gmailClient = GmailClient.GetInstance(accessToken, defaultEmail))
            {
                try
                {
                    // Fetch the contact by its unique URI.
                    Contact contact = gmailClient.GetContact(contactUri);
                    if (contact == null)
                    {
                        Console.Error.WriteLine("Contact not found.");
                        return;
                    }

                    // Update the phone number.
                    contact.PhoneNumbers.Clear();
                    contact.PhoneNumbers.Add(new PhoneNumber
                    {
                        Number = "555-1234",
                        Category = PhoneNumberCategory.Company
                    });

                    // Save the updated contact.
                    gmailClient.UpdateContact(contact);
                    Console.WriteLine("Contact phone number updated successfully.");
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Gmail operation failed: {ex.Message}");
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
