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
            // Placeholder connection details
            string mailboxUri = "https://exchange.example.com/EWS/Exchange.asmx";
            string username = "username";
            string password = "password";

            // Skip execution when using placeholder credentials to avoid real network calls
            if (mailboxUri.Contains("example.com") || username == "username" || password == "password")
            {
                Console.WriteLine("Placeholder credentials detected. Skipping contact creation.");
                return;
            }

            // Create and configure the EWS client
            using (IEWSClient client = EWSClient.GetEWSClient(mailboxUri, username, password))
            {
                try
                {
                    // Build a new contact with required fields
                    Contact newContact = new Contact
                    {
                        DisplayName = "John Doe"
                    };
                    newContact.EmailAddresses.Add(new EmailAddress("john.doe@example.com", "John Doe"));

                    // Create the contact in the Exchange store
                    string contactUri = client.CreateContact(newContact);
                    Console.WriteLine($"Contact created successfully. URI: {contactUri}");
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Error while creating contact: {ex.Message}");
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unhandled exception: {ex.Message}");
        }
    }
}
