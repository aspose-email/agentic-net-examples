using System;
using Aspose.Email;
using Aspose.Email.Clients.Exchange.WebService;
using Aspose.Email.PersonalInfo;

class Program
{
    static void Main()
    {
        // Placeholder values – replace with real credentials to run against Exchange
        string mailboxUri = "https://outlook.office365.com/EWS/Exchange.asmx";
        string username = "user@example.com";
        string password = "password";

        // Guard: skip external calls when placeholders are still in use
        if (mailboxUri.Contains("outlook.office365.com") &&
            username.Contains("example.com") &&
            password == "password")
        {
            Console.WriteLine("Placeholder credentials detected. Skipping EWS operations.");
            return;
        }

        try
        {
            using (IEWSClient client = EWSClient.GetEWSClient(mailboxUri, username, password))
            {
                // Create a new contact with desired fields
                Contact newContact = new Contact
                {
                    GivenName = "John",
                    Surname = "Doe",
                    DisplayName = "John Doe"
                };

                // Add email address
                newContact.EmailAddresses.Add(new EmailAddress("john.doe@example.com"));

                // Add phone number using property initialization
                newContact.PhoneNumbers.Add(new PhoneNumber
                {
                    Number = "555-1234",
                    Category = PhoneNumberCategory.Work
                });

                // Persist the contact in the Exchange store
                string contactId = client.CreateContact(newContact);
                Console.WriteLine($"Contact created with ID: {contactId}");
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
