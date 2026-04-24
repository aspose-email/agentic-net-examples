using System;
using System.IO;
using Aspose.Email;
using Aspose.Email.Clients.Google;
using Aspose.Email.PersonalInfo;

class Program
{
    static void Main()
    {
        try
        {
            // Placeholder credentials – replace with real values.
            string accessToken = "YOUR_ACCESS_TOKEN";
            string defaultEmail = "user@example.com";

            // Guard against placeholder credentials to avoid live network calls.
            if (string.IsNullOrWhiteSpace(accessToken) || accessToken.Contains("YOUR_") ||
                string.IsNullOrWhiteSpace(defaultEmail) || defaultEmail.Contains("example.com"))
            {
                Console.Error.WriteLine("Please provide valid Gmail OAuth credentials. Skipping execution.");
                return;
            }

            // Create Gmail client.
            using (IGmailClient gmailClient = GmailClient.GetInstance(accessToken, defaultEmail))
            {
                // Fetch all contacts.
                Contact[] contacts = gmailClient.GetAllContacts();

                // Define CSV output path.
                string csvPath = "contacts.csv";

                // Ensure the output directory exists.
                string directory = Path.GetDirectoryName(csvPath);
                if (!string.IsNullOrEmpty(directory) && !Directory.Exists(directory))
                {
                    Directory.CreateDirectory(directory);
                }

                // Write contacts to CSV.
                try
                {
                    using (StreamWriter writer = new StreamWriter(csvPath, false))
                    {
                        // CSV header.
                        writer.WriteLine("Name,Email,Phone");

                        foreach (Contact contact in contacts)
                        {
                            // Resolve display name.
                            string name = string.IsNullOrWhiteSpace(contact.DisplayName)
                                ? $"{contact.GivenName} {contact.Surname}".Trim()
                                : contact.DisplayName;

                            // Resolve first email address.
                            string email = string.Empty;
                            if (contact.EmailAddresses != null && contact.EmailAddresses.Count > 0)
                            {
                                email = contact.EmailAddresses[0].Address;
                            }

                            // Resolve first phone number.
                            string phone = string.Empty;
                            if (contact.PhoneNumbers != null && contact.PhoneNumbers.Count > 0)
                            {
                                phone = contact.PhoneNumbers[0].Number;
                            }

                            // Escape commas in fields.
                            name = name?.Replace(",", " ");
                            email = email?.Replace(",", " ");
                            phone = phone?.Replace(",", " ");

                            writer.WriteLine($"{name},{email},{phone}");
                        }
                    }

                    Console.WriteLine($"Contacts exported successfully to '{csvPath}'.");
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Error writing CSV file: {ex.Message}");
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
