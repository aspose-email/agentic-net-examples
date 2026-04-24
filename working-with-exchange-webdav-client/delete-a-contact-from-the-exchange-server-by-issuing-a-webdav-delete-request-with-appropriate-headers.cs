using Aspose.Email.PersonalInfo;
using Aspose.Email;
using System;
using Aspose.Email.Clients.Exchange.Dav;

class Program
{
    static void Main()
    {
        try
        {
            // Placeholder connection details
            string mailboxUri = "https://exchange.example.com/ews/Exchange.asmx";
            string username = "username";
            string password = "password";

            // Skip execution when placeholders are detected to avoid external calls during CI
            if (string.IsNullOrEmpty(mailboxUri) || mailboxUri.Contains("example.com"))
            {
                Console.Error.WriteLine("Placeholder credentials detected. Skipping execution.");
                return;
            }

            // Initialize the Exchange WebDAV client
            using (ExchangeClient client = new ExchangeClient(mailboxUri, username, password))
            {
                // Optional: enable pre-authentication
                client.PreAuthenticate = true;

                // Identifier of the contact to delete (replace with a real ID in production)
                string contactId = "contact-id-placeholder";

                // Delete the contact using WebDAV DELETE internally
                client.DeleteContact(contactId);
                Console.WriteLine("Contact deleted successfully.");
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
