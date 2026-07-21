using Aspose.Email.Clients.Exchange.WebService;
using System;
using Aspose.Email;
using Aspose.Email.PersonalInfo;

class Program
{
    static void Main(string[] args)
    {
        // Author note: Replace the service URL and credentials with valid values for your Exchange server.
        try
        {
            string mailboxUri = "https://exchange.example.com/EWS/Exchange.asmx";
            string username = "username";
            string password = "password";


            // Skip external calls when placeholder credentials are used
            if (mailboxUri.Contains("example.com") || username == "username" || password == "password")
            {
                Console.Error.WriteLine("Placeholder credentials detected. Skipping external calls.");
                return;
            }

            using (IEWSClient client = EWSClient.GetEWSClient(mailboxUri, username, password))
            {
                try
                {
                    // Retrieve contacts from the default contacts folder.
                    Contact[] contacts = client.GetContacts("contacts");
                    
                    // Process each contact.
                    foreach (Contact contact in contacts)
                    {
                        string email = contact.EmailAddresses.Count > 0 ? contact.EmailAddresses[0].Address : "(no email)";
                        Console.WriteLine($"{contact.DisplayName}: {email}");
                    }
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine("Error retrieving contacts: " + ex.Message);
                    return;
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine("Failed to create or use Exchange client: " + ex.Message);
            return;
        }
    }
}
