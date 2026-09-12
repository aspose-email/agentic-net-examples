using System;
using System.IO;
using System.Collections.Generic;
using System.Text.Json;
using Aspose.Email;
using Aspose.Email.PersonalInfo;
using Aspose.Email.Clients.Google;

class Program
{
    static void Main()
    {
        try
        {
            // Paths and placeholder credentials
            const string jsonConfigPath = "contacts_update_config.json";
            const string accessToken = "YOUR_ACCESS_TOKEN";
            const string defaultEmail = "user@example.com";

            // Guard against placeholder credentials – skip actual Gmail calls in CI
            if (accessToken.StartsWith("YOUR_") || defaultEmail.StartsWith("user@"))
            {
                Console.Error.WriteLine("Placeholder Gmail credentials detected. Skipping update operation.");
                return;
            }

            // Ensure JSON configuration file exists; create minimal placeholder if missing
            if (!File.Exists(jsonConfigPath))
            {
                try
                {
                    var placeholder = new Dictionary<string, string>();
                    string placeholderJson = JsonSerializer.Serialize(placeholder, new JsonSerializerOptions { WriteIndented = true });
                    File.WriteAllText(jsonConfigPath, placeholderJson);
                    Console.Error.WriteLine($"Configuration file not found. Created empty placeholder at '{jsonConfigPath}'.");
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to create placeholder config file: {ex.Message}");
                    return;
                }
            }

            // Load JSON configuration mapping email -> organization name
            Dictionary<string, string> orgUpdates;
            try
            {
                string jsonContent = File.ReadAllText(jsonConfigPath);
                orgUpdates = JsonSerializer.Deserialize<Dictionary<string, string>>(jsonContent);
                if (orgUpdates == null)
                {
                    Console.Error.WriteLine("Configuration file is empty or malformed.");
                    return;
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Error reading configuration file: {ex.Message}");
                return;
            }

            // Create Gmail client
            IGmailClient gmailClient;
            try
            {
                gmailClient = GmailClient.GetInstance(accessToken, defaultEmail);
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Failed to create Gmail client: {ex.Message}");
                return;
            }

            // Fetch all contacts
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

            // Iterate and update organization (CompanyName) where applicable
            foreach (Contact contact in contacts)
            {
                // Use the primary email address for matching
                string primaryEmail = null;
                if (contact.EmailAddresses != null && contact.EmailAddresses.Count > 0)
                {
                    primaryEmail = contact.EmailAddresses[0].Address;
                }

                if (primaryEmail != null && orgUpdates.TryGetValue(primaryEmail, out string newOrg))
                {
                    // Update the CompanyName property (represents organization)
                    contact.CompanyName = newOrg;

                    try
                    {
                        gmailClient.UpdateContact(contact);
                        Console.WriteLine($"Updated organization for contact '{primaryEmail}' to '{newOrg}'.");
                    }
                    catch (Exception ex)
                    {
                        Console.Error.WriteLine($"Failed to update contact '{primaryEmail}': {ex.Message}");
                    }
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
