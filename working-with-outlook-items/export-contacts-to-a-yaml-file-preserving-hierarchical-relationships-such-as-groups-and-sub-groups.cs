using Aspose.Email;
using Aspose.Email.Clients.Google;
using Aspose.Email.PersonalInfo;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;

namespace AsposeEmailGmailExport
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                // Placeholder credentials – replace with real values.
                string clientId = "your-client-id";
                string clientSecret = "your-client-secret";
                string refreshToken = "your-refresh-token";

                if (clientId.StartsWith("your-") || clientSecret.StartsWith("your-") || refreshToken.StartsWith("your-"))
                {
                    Console.Error.WriteLine("Please provide valid Gmail API credentials.");
                    return;
                }

                string outputPath = "gmail_contacts.yaml";
                string outputDir = Path.GetDirectoryName(outputPath);
                if (!string.IsNullOrEmpty(outputDir) && !Directory.Exists(outputDir))
                {
                    Directory.CreateDirectory(outputDir);
                }

                // Create Gmail client.
                using (IGmailClient gmailClient = GmailClient.GetInstance(clientId, clientSecret, refreshToken, null, null))
                {
                    // Retrieve all contact groups.
                    ContactGroupCollection groups;
                    try
                    {
                        groups = gmailClient.GetAllGroups();
                    }
                    catch (Exception ex)
                    {
                        Console.Error.WriteLine($"Failed to retrieve groups: {ex.Message}");
                        return;
                    }

                    List<string> yamlLines = new List<string>();
                    yamlLines.Add("contacts:");

                    foreach (GoogleContactGroup group in groups)
                    {
                        yamlLines.Add($"  - group: \"{group.Title}\"");
                        yamlLines.Add("    contacts:");

                        Contact[] contactsInGroup;
                        try
                        {
                            contactsInGroup = gmailClient.GetContactsFromGroup(group.Id);
                        }
                        catch (Exception ex)
                        {
                            Console.Error.WriteLine($"Failed to retrieve contacts for group '{group.Title}': {ex.Message}");
                            continue;
                        }

                        foreach (Contact contact in contactsInGroup)
                        {
                            string displayName = contact.DisplayName ?? string.Empty;
                            string email = string.Empty;
                            if (contact.EmailAddresses.Count > 0)
                            {
                                email = contact.EmailAddresses[0].Address;
                            }

                            yamlLines.Add($"      - name: \"{displayName.Replace("\"", "\\\"")}\"");
                            yamlLines.Add($"        email: \"{email}\"");
                        }
                    }

                    // Write YAML to file.
                    try
                    {
                        File.WriteAllLines(outputPath, yamlLines);
                        Console.WriteLine($"Contacts exported to {outputPath}");
                    }
                    catch (Exception ex)
                    {
                        Console.Error.WriteLine($"Failed to write YAML file: {ex.Message}");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Unexpected error: {ex.Message}");
            }
        }
    }
}
