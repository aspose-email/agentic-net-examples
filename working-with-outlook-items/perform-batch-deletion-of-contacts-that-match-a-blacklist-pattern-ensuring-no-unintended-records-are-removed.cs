using System;
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
            string clientId = "YOUR_CLIENT_ID";
            string clientSecret = "YOUR_CLIENT_SECRET";
            string refreshToken = "YOUR_REFRESH_TOKEN";
            string defaultEmail = ""; // Required parameter for the overload.

            // Guard against placeholder values to avoid unintended network calls.
            if (clientId.StartsWith("YOUR_") || clientSecret.StartsWith("YOUR_") || refreshToken.StartsWith("YOUR_"))
            {
                Console.Error.WriteLine("Placeholder Gmail credentials detected. Skipping execution.");
                return;
            }

            // Create Gmail client (required defaultEmail argument).
            using (IGmailClient gmailClient = GmailClient.GetInstance(clientId, clientSecret, refreshToken, null, defaultEmail))
            {
                // Define blacklist patterns (e.g., domains to remove).
                List<string> blacklist = new List<string> { "spamdomain.com", "blocked.com" };

                Contact[] allContacts;
                try
                {
                    allContacts = gmailClient.GetAllContacts();
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to retrieve contacts: {ex.Message}");
                    return;
                }

                foreach (Contact contact in allContacts)
                {
                    bool shouldDelete = false;

                    if (contact.EmailAddresses != null)
                    {
                        foreach (EmailAddress email in contact.EmailAddresses)
                        {
                            foreach (string blocked in blacklist)
                            {
                                if (!string.IsNullOrEmpty(email.Address) &&
                                    email.Address.IndexOf(blocked, StringComparison.OrdinalIgnoreCase) >= 0)
                                {
                                    shouldDelete = true;
                                    break;
                                }
                            }
                            if (shouldDelete) break;
                        }
                    }

                    if (shouldDelete)
                    {
                        try
                        {
                            // Delete by contact identifier (convert ObjectIdentifier to string).
                            string contactId = contact.Id?.ToString() ?? string.Empty;
                            if (!string.IsNullOrEmpty(contactId))
                            {
                                gmailClient.DeleteContact(contactId);
                                Console.WriteLine($"Deleted contact: {contact.DisplayName ?? contactId}");
                            }
                            else
                            {
                                Console.Error.WriteLine("Contact ID is missing; cannot delete.");
                            }
                        }
                        catch (Exception ex)
                        {
                            Console.Error.WriteLine($"Failed to delete contact {contact.Id?.ToString()}: {ex.Message}");
                        }
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
