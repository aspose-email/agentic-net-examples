using System;
using System.Net;
using Aspose.Email.Clients.Exchange.WebService;
using Aspose.Email.PersonalInfo;

class Program
{
    static void Main()
    {
        var credential = new System.Net.NetworkCredential("username", "password", "domain");

        try
        {
            // Exchange server connection details (replace with real values)
            string mailboxUri = "https://exchange.example.com/EWS/Exchange.asmx";
            string username = "user@example.com";
            string password = "password";

            // Create EWS client
            using (Aspose.Email.Clients.Exchange.WebService.IEWSClient client = Aspose.Email.Clients.Exchange.WebService.EWSClient.GetEWSClient(mailboxUri, username, password))
            {
                // -------------------- Create a new contact --------------------
                Aspose.Email.PersonalInfo.Contact newContact = new Aspose.Email.PersonalInfo.Contact();
                newContact.DisplayName = "John Doe";
                newContact.EmailAddresses.Add(new Aspose.Email.PersonalInfo.EmailAddress("john.doe@example.com"));

                // Create the contact in the default contacts folder
                string contactUri = client.CreateContact(newContact);
                Console.WriteLine("Created contact URI: " + contactUri);

                // -------------------- List contacts in the default contacts folder --------------------
                string contactsFolderUri = client.MailboxInfo.ContactsUri;
                Aspose.Email.PersonalInfo.Contact[] contacts = client.GetContacts(contactsFolderUri);
                Console.WriteLine("Contacts in folder:");
                foreach (Aspose.Email.PersonalInfo.Contact c in contacts)
                {
                    Console.WriteLine("- " + c.DisplayName);
                }

                // -------------------- Update the contact --------------------
                newContact.DisplayName = "John A. Doe";
                client.UpdateContact(newContact);
                Console.WriteLine("Contact updated.");

                // -------------------- Delete the contact --------------------
                client.DeleteItem(contactUri, Aspose.Email.Clients.Exchange.WebService.DeletionOptions.DeletePermanently);
                Console.WriteLine("Contact deleted.");
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine("Error: " + ex.Message);
        }
    }
}