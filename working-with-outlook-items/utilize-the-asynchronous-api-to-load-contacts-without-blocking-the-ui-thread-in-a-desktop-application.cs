using Aspose.Email.PersonalInfo;
using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Aspose.Email;
using Aspose.Email.Clients.Exchange.WebService;

class Program
{
    static async Task Main(string[] args)
    {
        try
        {
            // Placeholder values – replace with real ones for actual use.
            string mailboxUri = "https://exchange.example.com/EWS/Exchange.asmx";
            ICredentials credentials = new NetworkCredential("username", "password");

            // Guard against executing real network calls with placeholder data.
            if (mailboxUri.Contains("example.com") || credentials is NetworkCredential nc && nc.UserName == "username")
            {
                Console.Error.WriteLine("Placeholder credentials detected. Skipping network call.");
                return;
            }

            // Create the async EWS client.
            IAsyncEwsClient client = await EWSClient.GetEwsClientAsync(mailboxUri, credentials);
            using (client)
            {
                // Specify the folder URI that contains contacts.
                string contactsFolderUri = "contacts";

                // Asynchronously retrieve contacts.
                Contact[] contacts = await client.GetContactsAsync(contactsFolderUri, ExchangeListContactsOptions.FetchPhoto, CancellationToken.None);

                // Process the contacts (example: output names).
                foreach (Contact contact in contacts)
                {
                    Console.WriteLine($"Name: {contact.GivenName} {contact.Surname}");
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
