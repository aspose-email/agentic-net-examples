using System;
using System.Net;
using Aspose.Email;
using Aspose.Email.Clients.Exchange.WebService;
using Aspose.Email.Clients.Exchange;
using Aspose.Email.PersonalInfo;

namespace AsposeEmailExample
{
    class Program
    {
        static void Main(string[] args)
        {
        var credential = new System.Net.NetworkCredential("username", "password", "domain");

            try
            {
                // Define connection parameters (replace with real values)
                string mailboxUri = "https://exchange.example.com/EWS/Exchange.asmx";
                string username = "user@example.com";
                string password = "password";

                // Create the EWS client using the factory method
                using (IEWSClient client = EWSClient.GetEWSClient(mailboxUri, username, password))
                {
                    try
                    {
                        // Retrieve mailbox information to get the contacts folder URI
                        ExchangeMailboxInfo mailboxInfo = client.MailboxInfo;
                        string contactsFolderUri = mailboxInfo.ContactsUri;

                        // Fetch all contacts from the contacts folder
                        Contact[] contacts = client.GetContacts(contactsFolderUri);

                        // Update each contact (example: prepend "Updated " to the display name)
                        foreach (Contact contact in contacts)
                        {
                            string originalName = contact.DisplayName ?? string.Empty;
                            contact.DisplayName = "Updated " + originalName;

                            // Optionally add a new email address if none exists
                            if (contact.EmailAddresses.Count == 0)
                            {
                                contact.EmailAddresses.Add(new EmailAddress("newaddress@example.com"));
                            }

                            // Persist the changes back to the server
                            client.UpdateContact(contact);
                            Console.WriteLine($"Contact '{originalName}' updated successfully.");
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.Error.WriteLine($"Error during contact processing: {ex.Message}");
                        return;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Unhandled exception: {ex.Message}");
                return;
            }
        }
    }
}