using System;
using Aspose.Email;
using Aspose.Email.Clients.Google;
using Aspose.Email.PersonalInfo;
using Aspose.Email.Tools.Verifications;
using System.Collections.Generic;

class Program
{
    static void Main()
    {
        try
        {
            // Placeholder credentials – replace with real values.
            string accessToken = "YOUR_ACCESS_TOKEN";
            string defaultEmail = "YOUR_EMAIL@example.com";

            // Skip execution if placeholders are not replaced.
            if (accessToken.StartsWith("YOUR_") || defaultEmail.StartsWith("YOUR_"))
            {
                Console.Error.WriteLine("Please provide valid Gmail OAuth credentials before running the sample.");
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

            // Prepare validator.
            EmailValidator validator = new EmailValidator();

            // List to hold invalid email entries.
            List<string> invalidEmails = new List<string>();

            // Iterate contacts and validate each email address.
            foreach (Contact contact in contacts)
            {
                if (contact == null) continue;

                // EmailAddressList may contain multiple entries.
                EmailAddressList emailList = contact.EmailAddresses;
                if (emailList == null) continue;

                foreach (EmailAddress emailAddr in emailList)
                {
                    if (emailAddr == null) continue;

                    ValidationResult result;
                    try
                    {
                        validator.Validate(emailAddr.Address, out result);
                    }
                    catch (Exception ex)
                    {
                        Console.Error.WriteLine($"Validation error for '{emailAddr.Address}': {ex.Message}");
                        continue;
                    }

                    if (result.ReturnCode != ValidationResponseCode.ValidationSuccess)
                    {
                        invalidEmails.Add($"{contact.DisplayName ?? "(no name)"}: {emailAddr.Address} – {result.Message}");
                    }
                }
            }

            // Output results.
            if (invalidEmails.Count == 0)
            {
                Console.WriteLine("All contact email addresses are valid.");
            }
            else
            {
                Console.WriteLine("Invalid email addresses found:");
                foreach (string entry in invalidEmails)
                {
                    Console.WriteLine(entry);
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
