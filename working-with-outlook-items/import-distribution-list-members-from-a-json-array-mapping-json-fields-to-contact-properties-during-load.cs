using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.Json;
using Aspose.Email;
using Aspose.Email.Clients.Exchange.WebService;
using Aspose.Email.Clients.Exchange;
using Aspose.Email.Mapi;

namespace DistributionListImport
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                // Input JSON file containing an array of contacts
                string jsonFilePath = "contacts.json";

                // Guard file existence
                if (!File.Exists(jsonFilePath))
                {
                    try
                    {
                        // Create a minimal placeholder JSON file (empty array)
                        File.WriteAllText(jsonFilePath, "[]", Encoding.UTF8);
                        Console.WriteLine($"Placeholder JSON file created at '{jsonFilePath}'.");
                    }
                    catch (Exception ex)
                    {
                        Console.Error.WriteLine($"Failed to create placeholder JSON file: {ex.Message}");
                        return;
                    }
                }

                // Read and deserialize JSON
                List<ContactInfo> contacts;
                try
                {
                    string jsonContent = File.ReadAllText(jsonFilePath, Encoding.UTF8);
                    contacts = JsonSerializer.Deserialize<List<ContactInfo>>(jsonContent);
                    if (contacts == null)
                    {
                        Console.Error.WriteLine("JSON deserialization returned null.");
                        return;
                    }
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Error reading or parsing JSON file: {ex.Message}");
                    return;
                }

                // Prepare MailAddressCollection for distribution list members
                MailAddressCollection members = new MailAddressCollection();
                foreach (ContactInfo contact in contacts)
                {
                    if (string.IsNullOrWhiteSpace(contact.EmailAddress))
                        continue; // Skip entries without email

                    // Use display name if provided, otherwise email as display name
                    string displayName = string.IsNullOrWhiteSpace(contact.DisplayName) ? contact.EmailAddress : contact.DisplayName;
                    members.Add(new MailAddress(contact.EmailAddress, displayName));
                }

                // Define the distribution list to update/create
                ExchangeDistributionList distributionList = new ExchangeDistributionList
                {
                    DisplayName = "Imported Distribution List"
                };

                // Connection parameters (replace with real values)
                string host = "https://exchange.example.com/EWS/Exchange.asmx";
                string username = "user@example.com";
                string password = "password";

                // Guard placeholder credentials to avoid live network calls
                if (host.Contains("example.com") || username.Contains("example.com") || password == "password")
                {
                    Console.WriteLine("Placeholder credentials detected. Skipping actual server call.");
                    return;
                }

                // Create and use IEWSClient
                try
                {
                    using (IEWSClient client = EWSClient.GetEWSClient(host, username, password))
                    {
                        // Add members to the distribution list on the server
                        client.AddToDistributionList(distributionList, members);
                        Console.WriteLine("Distribution list members imported successfully.");
                    }
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Error interacting with Exchange server: {ex.Message}");
                    return;
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Unexpected error: {ex.Message}");
            }
        }

        // Simple POCO matching the expected JSON structure
        private class ContactInfo
        {
            public string DisplayName { get; set; }
            public string EmailAddress { get; set; }
        }
    }
}
