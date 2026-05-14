using Aspose.Email;
using System;
using Aspose.Email.Clients.Google;
using Aspose.Email.PersonalInfo;

class Program
{
    static void Main()
    {
        try
        {
            // Placeholder credentials – replace with real values.
            string clientId = "your_client_id";
            string clientSecret = "your_client_secret";
            string refreshToken = "your_refresh_token";

            // Guard against executing with placeholder credentials.
            if (clientId.StartsWith("your_") || clientSecret.StartsWith("your_") || refreshToken.StartsWith("your_"))
            {
                Console.Error.WriteLine("Gmail client credentials are placeholders. Skipping execution.");
                return;
            }

            // Create Gmail client. The last parameter is a proxy (null for default).
            IGmailClient gmailClient = GmailClient.GetInstance(clientId, clientSecret, refreshToken, null);

            try
            {
                // Retrieve all contacts.
                Contact[] contacts = gmailClient.GetAllContacts();

                // Default domain to apply.
                const string defaultDomain = "example.com";

                foreach (Contact contact in contacts)
                {
                    bool hasEmail = false;
                    foreach (EmailAddress emailAddr in contact.EmailAddresses)
                    {
                        if (!string.IsNullOrWhiteSpace(emailAddr.Address))
                        {
                            hasEmail = true;
                            break;
                        }
                    }

                    if (!hasEmail)
                    {
                        // Build a fallback email address.
                        string fallbackLocalPart = string.IsNullOrWhiteSpace(contact.DisplayName)
                            ? "user"
                            : contact.DisplayName.Replace(" ", "").ToLowerInvariant();

                        string fallbackEmail = $"{fallbackLocalPart}@{defaultDomain}";

                        // Add the fallback email address.
                        contact.EmailAddresses.Add(new EmailAddress(fallbackEmail));

                        // Update the contact on the server.
                        try
                        {
                            gmailClient.UpdateContact(contact);
                            Console.WriteLine($"Updated contact '{contact.DisplayName}' with email '{fallbackEmail}'.");
                        }
                        catch (Exception ex)
                        {
                            Console.Error.WriteLine($"Failed to update contact '{contact.DisplayName}': {ex.Message}");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Gmail client operation failed: {ex.Message}");
            }
            finally
            {
                if (gmailClient is IDisposable disposableClient)
                {
                    disposableClient.Dispose();
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
