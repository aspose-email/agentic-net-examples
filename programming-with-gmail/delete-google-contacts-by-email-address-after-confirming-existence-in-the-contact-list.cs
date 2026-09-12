using System;
using Aspose.Email;
using Aspose.Email.Clients.Google;
using Aspose.Email.PersonalInfo;

class Program
{
    static void Main(string[] args)
    {
        try
        {
            // Placeholder credentials – replace with real values.
            string clientId = "YOUR_CLIENT_ID";
            string clientSecret = "YOUR_CLIENT_SECRET";
            string refreshToken = "YOUR_REFRESH_TOKEN";
            string userEmail = "user@example.com";
            string targetEmail = "contact_to_delete@example.com";

            // Skip execution when placeholders are not replaced.
            if (clientId.StartsWith("YOUR_") ||
                clientSecret.StartsWith("YOUR_") ||
                refreshToken.StartsWith("YOUR_") ||
                userEmail.StartsWith("user@") ||
                targetEmail.StartsWith("contact_to_delete@"))
            {
                Console.Error.WriteLine("Placeholder credentials or target email detected. Skipping execution.");
                return;
            }

            // Create Gmail client.
            using (IGmailClient gmailClient = GmailClient.GetInstance(clientId, clientSecret, refreshToken, userEmail))
            {
                try
                {
                    // Retrieve all contacts.
                    Contact[] contacts = gmailClient.GetAllContacts();

                    // Find contact(s) matching the target email address.
                    foreach (Contact contact in contacts)
                    {
                        if (contact.EmailAddresses != null && contact.EmailAddresses.Count > 0)
                        {
                            foreach (EmailAddress emailAddr in contact.EmailAddresses)
                            {
                                if (string.Equals(emailAddr.Address, targetEmail, StringComparison.OrdinalIgnoreCase))
                                {
                                    // Delete the contact using its identifier.
                                    gmailClient.DeleteContact(contact.Id.ToString());
                                    Console.WriteLine($"Deleted contact with email: {targetEmail}");
                                    // Assuming email addresses are unique; exit after deletion.
                                    return;
                                }
                            }
                        }
                    }

                    Console.WriteLine($"Contact with email '{targetEmail}' not found.");
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Error during contact processing: {ex.Message}");
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unhandled exception: {ex.Message}");
        }
    }
}
