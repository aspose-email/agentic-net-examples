using Aspose.Email;
using System;
using Aspose.Email.Clients.Exchange.Dav;
using Aspose.Email.PersonalInfo;

class Program
{
    static void Main()
    {
        try
        {
            // Placeholder credentials – replace with real values when needed.
            string serviceUrl = "https://exchange.example.com/EWS/Exchange.asmx";
            string username = "user@example.com";
            string password = "password";

            // Prevent accidental network calls with placeholder data.
            if (serviceUrl.Contains("example.com"))
            {
                Console.Error.WriteLine("Placeholder Exchange service URL detected. Skipping network call.");
                return;
            }

            // Use ExchangeClient to retrieve contacts.
            using (ExchangeClient client = new ExchangeClient(serviceUrl, username, password))
            {
                // Retrieve contacts from the default contacts folder.
                Contact[] contacts = client.GetContacts("contacts");

                foreach (Contact contact in contacts)
                {
                    // Display selected properties, omitting notes and private comments.
                    Console.WriteLine("Display Name: " + contact.DisplayName);

                    string email = string.Empty;
                    if (contact.EmailAddresses != null && contact.EmailAddresses.Count > 0)
                    {
                        email = contact.EmailAddresses[0].Address;
                    }
                    Console.WriteLine("Email Address: " + email);

                    string phone = string.Empty;
                    if (contact.PhoneNumbers != null && contact.PhoneNumbers.Count > 0)
                    {
                        phone = contact.PhoneNumbers[0].Number;
                    }
                    Console.WriteLine("Phone: " + phone);
                    Console.WriteLine(new string('-', 40));
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine("Unhandled exception: " + ex.Message);
        }
    }
}
