using System;
using System.Net;
using Aspose.Email;
using Aspose.Email.Clients.Exchange.WebService;

class Program
{
    static void Main()
    {
        var credential = new System.Net.NetworkCredential("username", "password", "domain");

        try
        {
            // Initialize the EWS client using the factory method.
            // Replace the placeholder values with actual server URI and credentials.
            string mailboxUri = "https://exchange.example.com/EWS/Exchange.asmx";
            ICredentials credentials = new NetworkCredential("username", "password");

            using (IEWSClient client = EWSClient.GetEWSClient(mailboxUri, credentials))
            {
                try
                {
                    // Create a new folder named "NewFolder" in the root of the mailbox.
                    client.CreateFolder("NewFolder");
                    Console.WriteLine("Folder 'NewFolder' created successfully.");
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Error creating folder: {ex.Message}");
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unhandled exception: {ex.Message}");
        }
    }
}