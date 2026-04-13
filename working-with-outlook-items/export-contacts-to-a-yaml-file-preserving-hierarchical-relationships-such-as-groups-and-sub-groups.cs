using System;
using System.IO;
using System.Text;
using System.Collections.Generic;
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

            // Guard against placeholder credentials to avoid real network calls.
            if (string.IsNullOrWhiteSpace(accessToken) || accessToken.Contains("YOUR_ACCESS_TOKEN"))
            {
                Console.Error.WriteLine("Placeholder credentials detected. Skipping Gmail client operations.");
                return;
            }

            // Create Gmail client.
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

            // Fetch contacts and groups.
            Contact[] contacts;
            ContactGroupCollection groups;
            try
            {
                contacts = gmailClient.GetAllContacts();
                groups = gmailClient.GetAllGroups();
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Failed to retrieve contacts or groups: {ex.Message}");
                return;
            }

            // Build YAML content.
            var yamlBuilder = new StringBuilder();

            yamlBuilder.AppendLine("groups:");
            foreach (var group in groups)
            {
                yamlBuilder.AppendLine($"  - id: {group.Id}");
                yamlBuilder.AppendLine($"    title: \"{group.Title}\"");
            }

            yamlBuilder.AppendLine("contacts:");
            foreach (var contact in contacts)
            {
                yamlBuilder.AppendLine($"  - displayName: \"{contact.DisplayName}\"");

                // Get first email address if available.
                string email = null;
                foreach (var emailAddr in contact.EmailAddresses)
                {
                    email = emailAddr.Address;
                    break;
                }
                yamlBuilder.AppendLine($"    email: \"{email ?? string.Empty}\"");
            }

            // Define output file path.
            string outputPath = Path.Combine(Environment.CurrentDirectory, "contacts.yaml");
            string outputDir = Path.GetDirectoryName(outputPath);
            if (!Directory.Exists(outputDir))
            {
                try
                {
                    Directory.CreateDirectory(outputDir);
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to create output directory: {ex.Message}");
                    return;
                }
            }

            // Write YAML to file with error handling.
            try
            {
                File.WriteAllText(outputPath, yamlBuilder.ToString(), Encoding.UTF8);
                Console.WriteLine($"Contacts exported to {outputPath}");
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Failed to write YAML file: {ex.Message}");
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
