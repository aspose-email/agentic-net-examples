using System;
using Aspose.Email;
using Aspose.Email.Clients.Exchange.WebService;
using Aspose.Email.PersonalInfo;

class Program
{
    static void Main()
    {
        // Connection parameters for the Exchange Web Services endpoint
        string serviceUrl = "https://exchange.example.com/EWS/Exchange.asmx";
        string username = "user@example.com";
        string password = "password";

        // Guard: skip external calls when placeholder credentials are used
        bool placeholdersInUse = serviceUrl.Contains("example.com") &&
                                 username.Contains("example.com") &&
                                 password.Equals("password", StringComparison.OrdinalIgnoreCase);

        if (placeholdersInUse)
        {
            Console.WriteLine("Placeholder credentials detected. Skipping Exchange operations.");
            return;
        }

        try
        {
            // Create the EWS client (IEWSClient) and ensure it is disposed properly
            using (IEWSClient client = EWSClient.GetEWSClient(serviceUrl, username, password))
            {
                // Identifier (URI) of the contact to be updated
                string contactId = "https://exchange.example.com/EWS/Contacts/12345";

                // Retrieve the existing contact
                Contact contact = client.GetContact(contactId);
                if (contact == null)
                {
                    Console.Error.WriteLine("Contact not found.");
                    return;
                }

                // Modify desired fields
                contact.GivenName = "John";
                contact.Surname = "Doe";

                // Update the primary email address (replace if present, otherwise add)
                EmailAddress newEmail = new EmailAddress("john.doe@example.com");
                if (contact.EmailAddresses.Count > 0)
                {
                    contact.EmailAddresses[0] = newEmail;
                }
                else
                {
                    contact.EmailAddresses.Add(newEmail);
                }

                // Persist the changes back to the Exchange store
                client.UpdateContact(contact);
                Console.WriteLine("Contact updated successfully.");
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
