using System;
using System.Net;
using Aspose.Email;
using Aspose.Email.PersonalInfo;
using Aspose.Email.Clients.Exchange.WebService;

class Program
{
    static void Main()
    {
        try
        {
            // Initialize EWS client
            IEWSClient client = null;
            try
            {
                string mailboxUri = "https://exchange.example.com/EWS/Exchange.asmx";
                NetworkCredential credentials = new NetworkCredential("username", "password");
                client = EWSClient.GetEWSClient(mailboxUri, credentials);
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Failed to create EWS client: {ex.Message}");
                return;
            }

            // Create a new contact
            Contact newContact = new Contact
            {
                DisplayName = "John Doe"
            };
            newContact.EmailAddresses.Add(new EmailAddress("john.doe@example.com"));
            newContact.PhoneNumbers.Add(new PhoneNumber { Number = "123-456-7890" });

            // Upload the contact to the server
            try
            {
                string contactUri = client.CreateContact(newContact);
                Console.WriteLine($"Contact created successfully. URI: {contactUri}");
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Failed to create contact: {ex.Message}");
            }
            finally
            {
                client?.Dispose();
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
