using System;
using System.IO;
using System.Text;
using Aspose.Email;
using Aspose.Email.Clients.Google;
using Aspose.Email.PersonalInfo;

class Program
{
    static void Main()
    {
        try
        {
            // Placeholder credentials – replace with real values or skip execution.
            string accessToken = "YOUR_ACCESS_TOKEN";
            string defaultEmail = "user@example.com";

            if (accessToken == "YOUR_ACCESS_TOKEN")
            {
                Console.Error.WriteLine("Placeholder credentials detected. Skipping Gmail contact export.");
                return;
            }

            // Create Gmail client.
            IGmailClient gmailClient = null;
            try
            {
                gmailClient = GmailClient.GetInstance(accessToken, defaultEmail);
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Failed to create Gmail client: {ex.Message}");
                return;
            }

            using (gmailClient)
            {
                // Fetch all contacts.
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

                // Prepare CSV output.
                string outputPath = "contacts.csv";
                string directory = Path.GetDirectoryName(Path.GetFullPath(outputPath));
                if (!Directory.Exists(directory))
                {
                    try
                    {
                        Directory.CreateDirectory(directory);
                    }
                    catch (Exception ex)
                    {
                        Console.Error.WriteLine($"Failed to create directory '{directory}': {ex.Message}");
                        return;
                    }
                }

                try
                {
                    using (var writer = new StreamWriter(outputPath, false, Encoding.UTF8))
                    {
                        // Write CSV header.
                        writer.WriteLine("Name,Email,Phone");

                        foreach (Contact contact in contacts)
                        {
                            // Name.
                            string name = contact.DisplayName ?? string.Empty;

                            // Email – take the first address if available.
                            string email = string.Empty;
                            if (contact.EmailAddresses != null && contact.EmailAddresses.Count > 0)
                            {
                                email = contact.EmailAddresses[0].Address ?? string.Empty;
                            }

                            // Phone – take the first number if available.
                            string phone = string.Empty;
                            if (contact.PhoneNumbers != null && contact.PhoneNumbers.Count > 0)
                            {
                                phone = contact.PhoneNumbers[0].Number ?? string.Empty;
                            }

                            // Escape commas in fields.
                            name = name.Replace(",", "\\,");
                            email = email.Replace(",", "\\,");
                            phone = phone.Replace(",", "\\,");

                            writer.WriteLine($"{name},{email},{phone}");
                        }
                    }

                    Console.WriteLine($"Contacts exported successfully to '{outputPath}'.");
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to write CSV file: {ex.Message}");
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
