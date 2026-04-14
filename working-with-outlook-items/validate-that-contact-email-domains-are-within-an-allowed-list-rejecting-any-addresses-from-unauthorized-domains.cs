using Aspose.Email.PersonalInfo;
using System;
using System.Collections.Generic;
using System.Linq;
using Aspose.Email;
using Aspose.Email.Clients.Google;

namespace AsposeEmailDomainValidation
{
    class Program
    {
        static void Main()
        {
            try
            {
                // Placeholder credentials – replace with real values when running against Gmail.
                const string accessToken = "YOUR_ACCESS_TOKEN";
                const string defaultEmail = "user@example.com";

                // Guard against placeholder credentials to avoid external calls during CI.
                if (accessToken.StartsWith("YOUR_") || defaultEmail.StartsWith("user@"))
                {
                    Console.WriteLine("Placeholder credentials detected. Skipping Gmail operations.");
                    return;
                }

                // Create Gmail client instance.
                using (IGmailClient gmailClient = GmailClient.GetInstance(accessToken, defaultEmail))
                {
                    // Validate client connection.
                    try
                    {
                        // Attempt a lightweight operation to ensure credentials are valid.
                        // This call does not fetch data; it just checks the token.
                        gmailClient.GetAllContacts();
                    }
                    catch (Exception ex)
                    {
                        Console.Error.WriteLine($"Failed to authenticate Gmail client: {ex.Message}");
                        return;
                    }

                    // Define allowed email domains.
                    var allowedDomains = new HashSet<string>(StringComparer.OrdinalIgnoreCase)
                    {
                        "example.com",
                        "mydomain.org"
                    };

                    // Retrieve all contacts.
                    Contact[] contacts;
                    try
                    {
                        contacts = gmailClient.GetAllContacts();
                    }
                    catch (Exception ex)
                    {
                        Console.Error.WriteLine($"Error retrieving contacts: {ex.Message}");
                        return;
                    }

                    // Validate each contact's email addresses.
                    foreach (var contact in contacts)
                    {
                        // The Contact class provides EmailAddresses collection.
                        // Iterate through each email address associated with the contact.
                        foreach (var email in contact.EmailAddresses)
                        {
                            // Extract domain part of the email address.
                            var atIndex = email.Address?.IndexOf('@') ?? -1;
                            if (atIndex <= 0 || atIndex == email.Address.Length - 1)
                            {
                                Console.WriteLine($"Invalid email format: {email.Address}");
                                continue;
                            }

                            var domain = email.Address.Substring(atIndex + 1);
                            if (!allowedDomains.Contains(domain))
                            {
                                Console.WriteLine($"Unauthorized domain detected for contact '{contact.DisplayName}': {email.Address}");
                            }
                            else
                            {
                                Console.WriteLine($"Authorized contact: {contact.DisplayName} <{email.Address}>");
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Unhandled exception: {ex.Message}");
            }
        }
    }
}
