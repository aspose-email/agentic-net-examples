using System;
using System.Collections.Generic;
using System.Linq;
using Aspose.Email;
using Aspose.Email.Clients.Exchange.Dav;
using Aspose.Email.PersonalInfo;

class Program
{
    static void Main()
    {
        try
        {
            // Define allowed email domains
            List<string> allowedDomains = new List<string> { "example.com", "contoso.com" };

            // Placeholder connection parameters
            string mailboxUri = "https://exchange.example.com/ews/Exchange.asmx";
            string username = "user@example.com";
            string password = "password";

            // Guard against executing with placeholder credentials
            if (mailboxUri.Contains("example.com"))
            {
                Console.Error.WriteLine("Placeholder credentials detected. Skipping execution.");
                return;
            }

            // Create and use the Exchange client
            using (ExchangeClient client = new ExchangeClient(mailboxUri, username, password))
            {
                try
                {
                    // Retrieve contacts from the default "Contacts" folder
                    Contact[] contacts = client.GetContacts("Contacts");

                    foreach (Contact contact in contacts)
                    {
                        // Collect email addresses that are not from allowed domains
                        List<EmailAddress> toRemove = new List<EmailAddress>();

                        foreach (EmailAddress email in contact.EmailAddresses)
                        {
                            string emailStr = email.Address;
                            if (string.IsNullOrEmpty(emailStr) || !emailStr.Contains("@"))
                                continue;

                            string domain = emailStr.Split('@')[1];
                            if (!allowedDomains.Contains(domain, StringComparer.OrdinalIgnoreCase))
                            {
                                Console.WriteLine($"Unauthorized domain detected: {emailStr} (Contact: {contact.DisplayName})");
                                toRemove.Add(email);
                            }
                        }

                        // Remove unauthorized email addresses from the contact
                        foreach (EmailAddress remove in toRemove)
                        {
                            contact.EmailAddresses.Remove(remove);
                        }

                        // Optionally, update the contact on the server if needed
                        // client.UpdateContact(contact); // Uncomment if UpdateContact is available
                    }
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Error while processing contacts: {ex.Message}");
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
