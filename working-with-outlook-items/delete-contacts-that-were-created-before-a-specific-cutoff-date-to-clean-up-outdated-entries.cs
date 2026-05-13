using Aspose.Email.Clients.Exchange.Dav;
using Aspose.Email.PersonalInfo;
using System;
using Aspose.Email;
using Aspose.Email.Clients.Exchange;

class Program
{
    static void Main()
    {
        try
        {
            // Placeholder connection details – replace with real values.
            string mailboxUri = "https://exchange.example.com/ews/Exchange.asmx";
            string username = "user@example.com";
            string password = "password";

            // Skip execution when placeholder credentials are detected.
            if (mailboxUri.Contains("example.com") || username.Contains("example.com"))
            {
                Console.Error.WriteLine("Placeholder credentials detected. Skipping execution.");
                return;
            }

            // Define the cutoff date for outdated contacts.
            DateTime cutoffDate = new DateTime(2022, 1, 1);

            // Create and connect the Exchange client.
            using (ExchangeClient client = new ExchangeClient(mailboxUri, username, password))
            {
                try
                {
                    // Retrieve the URI of the contacts folder.
                    string contactsFolderUri = client.MailboxInfo.ContactsUri;

                    // List all contacts in the contacts folder.
                    Contact[] contacts = client.GetContacts(contactsFolderUri);

                    foreach (Contact contact in contacts)
                    {
                        // Attempt to obtain a creation date via reflection (property may not exist in all versions).
                        DateTime? creationDate = null;
                        var propInfo = contact.GetType().GetProperty("CreationDate");
                        if (propInfo != null && propInfo.PropertyType == typeof(DateTime))
                        {
                            creationDate = (DateTime)propInfo.GetValue(contact);
                        }

                        // If we couldn't get a creation date, skip this contact.
                        if (!creationDate.HasValue)
                            continue;

                        // Delete contacts created before the cutoff date.
                        if (creationDate.Value < cutoffDate)
                        {
                            try
                            {
                                client.DeleteContact(contact);
                                Console.WriteLine($"Deleted contact: {contact.DisplayName}");
                            }
                            catch (Exception ex)
                            {
                                Console.Error.WriteLine($"Failed to delete contact '{contact.DisplayName}': {ex.Message}");
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Error accessing contacts: {ex.Message}");
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
