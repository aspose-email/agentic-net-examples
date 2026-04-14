using System;
using System.Net;
using Aspose.Email;
using Aspose.Email.Clients.Exchange.WebService;
using Aspose.Email.PersonalInfo;

namespace AsposeEmailEwsContactExample
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                // Define connection parameters
                string mailboxUri = "https://exchange.example.com/EWS/Exchange.asmx";
                string username = "user@example.com";
                string password = "password";


                // Skip external calls when placeholder credentials are used
                if (mailboxUri.Contains("example.com") || username.Contains("example.com") || password == "password")
                {
                    Console.Error.WriteLine("Placeholder credentials detected. Skipping external calls.");
                    return;
                }

                // Create the EWS client inside a using block for proper disposal
                using (IEWSClient client = EWSClient.GetEWSClient(mailboxUri, username, password))
                {
                    // Build the contact with multiple email addresses and phone numbers
                    Contact contact = new Contact();
                    contact.DisplayName = "John Doe";

                    // Add email addresses
                    EmailAddress workEmail = new EmailAddress("john.doe@company.com");
                    EmailAddress personalEmail = new EmailAddress("john.doe@gmail.com");
                    contact.EmailAddresses.Add(workEmail);
                    contact.EmailAddresses.Add(personalEmail);

                    // Add phone numbers
                    PhoneNumber businessPhone = new PhoneNumber();
                    businessPhone.Number = "+1-555-0100";
                    businessPhone.Category = PhoneNumberCategory.Company; // Use Company instead of Business
                    PhoneNumber mobilePhone = new PhoneNumber();
                    mobilePhone.Number = "+1-555-0111";
                    mobilePhone.Category = PhoneNumberCategory.Mobile;
                    contact.PhoneNumbers.Add(businessPhone);
                    contact.PhoneNumbers.Add(mobilePhone);

                    // Add a custom note for CRM integration
                    contact.Notes = "CRM Integration: Preferred customer, VIP status.";

                    // Create the contact on the Exchange server
                    string contactUri = client.CreateContact(contact);
                    Console.WriteLine("Contact created successfully. URI: " + contactUri);
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine("Error: " + ex.Message);
            }
        }
    }
}
