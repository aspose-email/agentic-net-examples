using Aspose.Email.PersonalInfo;
using System;
using System.Text.RegularExpressions;
using Aspose.Email;
using Aspose.Email.Clients.Google;

class Program
{
    static void Main()
    {
        try
        {
            // Placeholder credentials – replace with real values for actual execution.
            string accessToken = "YOUR_ACCESS_TOKEN";
            string defaultEmail = "user@example.com";

            // Skip external calls when placeholders are detected.
            if (accessToken.StartsWith("YOUR_"))
            {
                Console.WriteLine("Placeholder credentials detected. Skipping Gmail operations.");
                return;
            }

            // Create Gmail client.
            using (IGmailClient gmailClient = GmailClient.GetInstance(accessToken, defaultEmail))
            {
                try
                {
                    // Retrieve all contacts.
                    Contact[] contacts = gmailClient.GetAllContacts();

                    // Simple regex for email validation.
                    Regex emailRegex = new Regex(@"^[^@\s]+@[^@\s]+\.[^@\s]+$", RegexOptions.Compiled);

                    foreach (Contact contact in contacts)
                    {
                        // Iterate through each email address of the contact.
                        foreach (var emailAddr in contact.EmailAddresses)
                        {
                            string address = emailAddr.Address;
                            if (!emailRegex.IsMatch(address))
                            {
                                Console.WriteLine($"Invalid email address in contact '{contact.DisplayName}': {address}");
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Error processing contacts: {ex.Message}");
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
