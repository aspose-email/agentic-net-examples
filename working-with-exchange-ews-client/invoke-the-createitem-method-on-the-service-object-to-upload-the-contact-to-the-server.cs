using System;
using Aspose.Email;
using Aspose.Email.PersonalInfo;
using Aspose.Email.Clients.Exchange.WebService;

namespace AsposeEmailCreateContactSample
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                // Initialize EWS client (replace with actual service URL and credentials)
                string mailboxUri = "https://ews.example.com/EWS/Exchange.asmx";
                string username = "username";
                string password = "password";


                // Skip external calls when placeholder credentials are used
                if (mailboxUri.Contains("example.com") || username == "username" || password == "password")
                {
                    Console.Error.WriteLine("Placeholder credentials detected. Skipping external calls.");
                    return;
                }

                using (IEWSClient client = EWSClient.GetEWSClient(mailboxUri, username, password))
                {
                    // Create a new contact
                    Contact contact = new Contact
                    {
                        GivenName = "John",
                        Surname = "Doe",
                        DisplayName = "John Doe"
                    };
                    contact.EmailAddresses.Add(new EmailAddress("john.doe@example.com"));

                    // Upload the contact to the server using CreateContact (the appropriate method for contacts)
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
}
