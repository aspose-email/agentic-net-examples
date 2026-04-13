using Aspose.Email.Clients.Exchange.WebService;
using System;
using System.Text;
using Aspose.Email;
using Aspose.Email.PersonalInfo;
using Aspose.Email.Mapi;

class Program
{
    static void Main()
    {
        try
        {
            // Initialize Exchange client (replace with actual server and credentials)
            string mailboxUri = "https://exchange.example.com/EWS/Exchange.asmx";
            string username = "username";
            string password = "password";

            using (IEWSClient client = EWSClient.GetEWSClient(mailboxUri, username, password))
            {
                // Create a new contact
                Contact contact = new Contact();
                contact.DisplayName = "John Doe";
                contact.CompanyName = "Example Corp";
                contact.EmailAddresses.Add(new EmailAddress("john.doe@example.com", "John Doe"));

                // Convert to MapiContact to add a custom extended property
                MapiContact mapiContact = (MapiContact)contact; // implicit conversion
                string customPropName = "X-Integration-Metadata";
                string customPropValue = "MetadataValue";


                // Skip external calls when placeholder credentials are used
                if (mailboxUri.Contains("example.com") || username == "username" || password == "password")
                {
                    Console.Error.WriteLine("Placeholder credentials detected. Skipping external calls.");
                    return;
                }

                // Add custom property (Unicode string)
                mapiContact.Properties.Add(new MapiProperty(0x8000, Encoding.Unicode.GetBytes(customPropValue)));

                // Create the contact on the Exchange server
                string contactUri = client.CreateContact(contact);
                Console.WriteLine("Contact created: " + contactUri);
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine(ex.Message);
        }
    }
}
