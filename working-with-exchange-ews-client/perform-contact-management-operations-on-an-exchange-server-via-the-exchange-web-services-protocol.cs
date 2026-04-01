using System;
using System.Net;
using Aspose.Email;
using Aspose.Email.PersonalInfo;
using Aspose.Email.Clients.Exchange.WebService;
using Aspose.Email.Clients.Exchange;

class Program
{
    static void Main()
    {
        try
        {
            // Placeholder credentials – replace with real values.
            string serviceUrl = "https://exchange.example.com/EWS/Exchange.asmx";
            string username = "user@example.com";
            string password = "password";

            // Guard against running with placeholder data.
            if (serviceUrl.Contains("example.com") || username.Contains("example.com"))
            {
                Console.Error.WriteLine("Placeholder credentials detected. Skipping execution.");
                return;
            }

            // Create the EWS client.
            try
            {
                using (IEWSClient client = EWSClient.GetEWSClient(serviceUrl, username, password))
                {
                    // ------------------- Create a new contact -------------------
                    Contact newContact = new Contact
                    {
                        GivenName = "John",
                        Surname = "Doe",
                        EmailAddresses = { new EmailAddress("john.doe@example.com", "John Doe") }
                    };

                    string contactUri = client.CreateContact(newContact);
                    Console.WriteLine($"Contact created. URI: {contactUri}");

                    // ------------------- List contacts in the default contacts folder -------------------
                    string contactsFolderUri = client.MailboxInfo.ContactsUri;
                    Contact[] contacts = client.GetContacts(contactsFolderUri);
                    Console.WriteLine($"Total contacts in folder: {contacts.Length}");
                    foreach (Contact c in contacts)
                    {
                        Console.WriteLine($"- {c.GivenName} {c.Surname}");
                    }

                    // ------------------- Update the created contact -------------------
                    Contact fetchedContact = client.GetContact(contactUri);
                    fetchedContact.GivenName = "Jonathan";
                    client.UpdateContact(fetchedContact);
                    Console.WriteLine("Contact updated.");

                    // ------------------- Delete the contact -------------------
                    client.DeleteItem(contactUri, new DeletionOptions(DeletionType.Default));
                    Console.WriteLine("Contact deleted.");
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"EWS operation failed: {ex.Message}");
                return;
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
