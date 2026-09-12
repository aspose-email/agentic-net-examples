using Aspose.Email.Mapi;
using System;
using Aspose.Email;
using Aspose.Email.PersonalInfo;
using Aspose.Email.Clients.Exchange.Dav;

class Program
{
    // Detect placeholder values to avoid real network calls in sample code
    static bool IsPlaceholder(string value)
    {
        return string.IsNullOrEmpty(value) ||
               value.Equals("username", StringComparison.OrdinalIgnoreCase) ||
               value.Equals("password", StringComparison.OrdinalIgnoreCase) ||
               value.Equals("url", StringComparison.OrdinalIgnoreCase) ||
               value.Equals("domain", StringComparison.OrdinalIgnoreCase);
    }

    static void Main()
    {
        // Exchange WebDAV service URL and credentials
        string serviceUrl = "https://exchange.example.com/EWS/Exchange.asmx";
        string username   = "username";
        string password   = "password";

        // Skip network operation if placeholders are used
        if (IsPlaceholder(serviceUrl) || IsPlaceholder(username) || IsPlaceholder(password))
        {
            Console.Error.WriteLine("Placeholder credentials detected. Skipping network operation.");
            return;
        }

        try
        {
            // Initialize the Exchange WebDAV client using the required using pattern
            using (ExchangeClient client = new ExchangeClient(serviceUrl, username, password))
            {
                // Build a new contact
                Contact contact = new Contact
                {
                    DisplayName = "John Doe"
                };
                contact.EmailAddresses.Add(new EmailAddress("john.doe@example.com"));

                // NOTE: Aspose.Email's Contact class does not expose a direct
                // extended property collection in recent versions.
                // If a custom attribute is required, it can be stored in the
                // "UserDefinedFields" of a MapiContact and then converted,
                // or handled via server‑side PROPPATCH outside of this SDK.
                // The following line is a placeholder to illustrate intent:
                // contact.SetExtendedProperty("CustomAttribute", "CustomValue");

                // Create the contact on the Exchange server
                string contactId = client.CreateContact(contact);
                Console.WriteLine($"Contact created with ID: {contactId}");
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
