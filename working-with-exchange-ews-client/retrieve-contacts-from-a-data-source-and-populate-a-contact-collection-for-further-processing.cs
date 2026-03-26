using System;
using System.Net;
using System.Collections.Generic;
using Aspose.Email;
using Aspose.Email.Clients.Exchange.WebService;
using Aspose.Email.PersonalInfo;

class Program
{
    static void Main()
    {
        var credential = new System.Net.NetworkCredential("username", "password", "domain");

        try
        {
            // Placeholder mailbox URI and credentials
            string mailboxUri = "https://exchange.example.com/EWS/Exchange.asmx";
            string username = "user@example.com";
            string password = "password";

            // Create the EWS client using the factory method
            using (IEWSClient client = EWSClient.GetEWSClient(mailboxUri, username, password))
            {
                // Get the contacts folder URI from mailbox info
                string contactsFolderUri = client.MailboxInfo.ContactsUri;

                // Retrieve contacts from the specified folder
                Contact[] contactsArray = client.GetContacts(contactsFolderUri);

                // Populate a strongly-typed collection for further processing
                List<Contact> contacts = new List<Contact>(contactsArray);

                // Example processing: output each contact's display name
                foreach (Contact contactItem in contacts)
                {
                    Console.WriteLine(contactItem.DisplayName);
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine(ex.Message);
        }
    }
}