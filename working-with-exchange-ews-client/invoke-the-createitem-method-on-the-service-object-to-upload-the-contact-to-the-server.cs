using Aspose.Email.PersonalInfo;
using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Aspose.Email;
using Aspose.Email.Mapi;
using Aspose.Email.Clients.Exchange.WebService;

class Program
{
    static void Main()
    {
        try
        {
            // Placeholder credentials and service URL
            string serviceUrl = "https://exchange.example.com/EWS/Exchange.asmx";
            string username = "username";
            string password = "password";

            // Guard against placeholder credentials to avoid real network calls
            if (serviceUrl.Contains("example.com") || username == "username")
            {
                Console.Error.WriteLine("Placeholder credentials detected. Skipping server interaction.");
                return;
            }

            // Create the EWS client
            using (IEWSClient client = EWSClient.GetEWSClient(serviceUrl, username, password))
            {
                try
                {
                    // Prepare a MAPI contact
                    MapiContact contact = new MapiContact();
                    contact.NameInfo.DisplayName = "John Doe";
                    contact.ElectronicAddresses.Email1.EmailAddress = "john.doe@example.com";

                    // Cast to async interface to use CreateItemAsync
                    if (client is IAsyncEwsClient asyncClient)
                    {
                        // Upload the contact to the Contacts folder
                        string contactUri = asyncClient
                            .CreateItemAsync(contact, client.MailboxInfo.ContactsUri, CancellationToken.None)
                            .GetAwaiter()
                            .GetResult();

                        Console.WriteLine("Contact created at URI: " + contactUri);
                    }
                    else
                    {
                        Console.Error.WriteLine("Client does not support async operations.");
                    }
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine("Error during contact creation: " + ex.Message);
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine("Unhandled exception: " + ex.Message);
        }
    }
}
