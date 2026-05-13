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
            // Placeholder credentials – replace with real values when needed.
            string clientId = "your-client-id";
            string clientSecret = "your-client-secret";
            string refreshToken = "your-refresh-token";

            // Guard against placeholder credentials to avoid live network calls.
            if (string.IsNullOrWhiteSpace(clientId) ||
                string.IsNullOrWhiteSpace(clientSecret) ||
                string.IsNullOrWhiteSpace(refreshToken) ||
                clientId.Contains("your-") ||
                clientSecret.Contains("your-") ||
                refreshToken.Contains("your-"))
            {
                Console.Error.WriteLine("Placeholder credentials detected. Skipping contact export.");
                return;
            }

            // Create Gmail client. Adjusted overload to match expected parameters.
            IGmailClient client;
            try
            {
                client = GmailClient.GetInstance(clientId, null, clientSecret, refreshToken);
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Failed to create Gmail client: {ex.Message}");
                return;
            }

            // Fetch contacts.
            Contact[] contacts;
            try
            {
                contacts = client.GetAllContacts();
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Failed to retrieve contacts: {ex.Message}");
                return;
            }

            // Define CSV output path.
            string outputPath = "contacts.csv";
            string outputDirectory = Path.GetDirectoryName(outputPath);
            if (!string.IsNullOrEmpty(outputDirectory) && !Directory.Exists(outputDirectory))
            {
                try
                {
                    Directory.CreateDirectory(outputDirectory);
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to create output directory: {ex.Message}");
                    return;
                }
            }

            // Write contacts to CSV with custom delimiter ';' and header row.
            try
            {
                using (StreamWriter writer = new StreamWriter(outputPath, false))
                {
                    // Header row.
                    writer.WriteLine("GivenName;Surname;Email");

                    foreach (Contact contact in contacts)
                    {
                        string givenName = contact.GivenName ?? string.Empty;
                        string surname = contact.Surname ?? string.Empty;
                        string email = string.Empty;

                        // Extract the first email address if available.
                        if (contact.EmailAddresses != null && contact.EmailAddresses.Count > 0)
                        {
                            EmailAddress firstEmail = contact.EmailAddresses[0];
                            if (firstEmail != null && !string.IsNullOrEmpty(firstEmail.Address))
                            {
                                email = firstEmail.Address;
                            }
                        }

                        // Escape delimiter in fields if needed.
                        givenName = givenName.Replace(";", "\\;");
                        surname = surname.Replace(";", "\\;");
                        email = email.Replace(";", "\\;");

                        writer.WriteLine($"{givenName};{surname};{email}");
                    }
                }

                Console.WriteLine($"Contacts exported successfully to '{outputPath}'.");
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Failed to write CSV file: {ex.Message}");
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
