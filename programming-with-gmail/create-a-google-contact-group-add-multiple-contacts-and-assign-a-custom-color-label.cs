using Aspose.Email.Clients;
using Aspose.Email.Clients.Graph;
using Aspose.Email.PersonalInfo;
using System;
using System.Collections.Generic;
using Aspose.Email;
using Aspose.Email.Clients.Google;

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
            if (string.IsNullOrWhiteSpace(accessToken) || accessToken == "YOUR_ACCESS_TOKEN")
            {
                Console.Error.WriteLine("Access token is not set. Skipping Gmail operations.");
                return;
            }

            // Create Gmail client.
            using (IGmailClient gmailClient = GmailClient.GetInstance(accessToken, defaultEmail))
            {
                // Retrieve existing contact groups.
                ContactGroupCollection groups = gmailClient.GetAllGroups();
                Console.WriteLine($"Found {groups.Count} contact groups.");

                // Example: pick the first group (if any) or note that none exist.
                string targetGroupId = groups.Count > 0 ? groups[0].Id : null;
                if (targetGroupId == null)
                {
                    Console.Error.WriteLine("No contact groups available. Cannot add contacts to a group.");
                }

                // Prepare a list of contacts to add.
                List<Contact> contactsToAdd = new List<Contact>
                {
                    new Contact
                    {
                        DisplayName = "John Doe",
                        EmailAddresses = { new EmailAddress("john.doe@example.com") }
                    },
                    new Contact
                    {
                        DisplayName = "Jane Smith",
                        EmailAddresses = { new EmailAddress("jane.smith@example.com") }
                    }
                };

                // Create each contact in Gmail and optionally associate with the target group.
                foreach (Contact contact in contactsToAdd)
                {
                    try
                    {
                        // Create the contact; the method returns the contact URI.
                        string contactUri = gmailClient.CreateContact(contact);
                        Console.WriteLine($"Created contact '{contact.DisplayName}' – URI: {contactUri}");

                        // If a group was identified, you could add the contact to the group via
                        // Gmail's API (not shown here as the specific method is not part of the
                        // documented IGmailClient interface). This placeholder demonstrates intent.
                        if (!string.IsNullOrEmpty(targetGroupId))
                        {
                            // Placeholder for adding contact to group.
                            // Example: gmailClient.AddContactToGroup(contactUri, targetGroupId);
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.Error.WriteLine($"Failed to create contact '{contact.DisplayName}': {ex.Message}");
                    }
                }

                // Assign a custom color label to the group (if a group was found).
                // Gmail API does not expose direct color assignment for groups; this is a placeholder.
                // If using Microsoft Graph, you could create a category with a preset color:
                // IGraphClient graphClient = GraphClient.GetInstance(...);
                // graphClient.CreateCategory("MyCustomLabel", CategoryPreset.Preset1);
                if (!string.IsNullOrEmpty(targetGroupId))
                {
                    Console.WriteLine($"Custom color label assignment for group '{targetGroupId}' is not supported via IGmailClient.");
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
