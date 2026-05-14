using Aspose.Email.PersonalInfo;
using System;
using System.Xml.Linq;
using Aspose.Email;
using Aspose.Email.Clients.Google;

namespace Sample
{
    class Program
    {
        static void Main()
        {
            try
            {
                // Placeholder credentials – replace with real values or skip execution.
                string clientId = "YOUR_CLIENT_ID";
                string clientSecret = "YOUR_CLIENT_SECRET";
                string refreshToken = "YOUR_REFRESH_TOKEN";
                string defaultEmail = "user@example.com";

                if (clientId.StartsWith("YOUR_") || clientSecret.StartsWith("YOUR_") || refreshToken.StartsWith("YOUR_"))
                {
                    Console.Error.WriteLine("Gmail credentials are placeholders. Skipping execution.");
                    return;
                }

                // Create Gmail client.
                IGmailClient gmailClient;
                try
                {
                    gmailClient = GmailClient.GetInstance(clientId, clientSecret, refreshToken, defaultEmail);
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to create Gmail client: {ex.Message}");
                    return;
                }

                // Use the client within a using block to ensure disposal.
                using (gmailClient)
                {
                    // Retrieve all contacts.
                    Contact[] contacts;
                    try
                    {
                        contacts = gmailClient.GetAllContacts();
                    }
                    catch (Exception ex)
                    {
                        Console.Error.WriteLine($"Failed to retrieve contacts: {ex.Message}");
                        return;
                    }

                    // Build CRM XML document.
                    XDocument crmXml = new XDocument(new XElement("Contacts"));
                    foreach (Contact contact in contacts)
                    {
                        XElement contactElement = new XElement("Contact",
                            new XElement("DisplayName", contact.DisplayName ?? string.Empty),
                            new XElement("GivenName", contact.GivenName ?? string.Empty),
                            new XElement("Surname", contact.Surname ?? string.Empty));

                        // Email addresses.
                        if (contact.EmailAddresses != null && contact.EmailAddresses.Count > 0)
                        {
                            XElement emailsElement = new XElement("Emails");
                            foreach (EmailAddress email in contact.EmailAddresses)
                            {
                                emailsElement.Add(new XElement("Email", email.Address ?? string.Empty));
                            }
                            contactElement.Add(emailsElement);
                        }

                        // Phone numbers.
                        if (contact.PhoneNumbers != null && contact.PhoneNumbers.Count > 0)
                        {
                            XElement phonesElement = new XElement("PhoneNumbers");
                            foreach (PhoneNumber phone in contact.PhoneNumbers)
                            {
                                phonesElement.Add(new XElement("Phone",
                                    new XAttribute("Category", phone.Category.ToString()),
                                    phone.Number ?? string.Empty));
                            }
                            contactElement.Add(phonesElement);
                        }

                        crmXml.Root.Add(contactElement);
                    }

                    // Output XML to console.
                    Console.WriteLine(crmXml.Declaration + crmXml.ToString());
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Unexpected error: {ex.Message}");
            }
        }
    }
}
