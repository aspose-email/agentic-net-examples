using System;
using System.Net;
using Aspose.Email;
using Aspose.Email.Clients.Exchange.WebService;

namespace AsposeEmailExample
{
    class Program
    {
        static void Main(string[] args)
        {
        var credential = new System.Net.NetworkCredential("username", "password", "domain");

            try
            {
                // Define the EWS service URL and credentials.
                string mailboxUri = "https://exchange.example.com/EWS/Exchange.asmx";
                NetworkCredential credentials = new NetworkCredential("username", "password");

                // Create the EWS client using the factory method.
                using (IEWSClient client = EWSClient.GetEWSClient(mailboxUri, credentials))
                {
                    // Define the user configuration name (name and folder identifier).
                    UserConfigurationName configName = new UserConfigurationName("MyConfig", "Inbox");

                    // Retrieve the user configuration from the server.
                    UserConfiguration userConfig = client.GetUserConfiguration(configName);

                    // Simple confirmation output.
                    Console.WriteLine("User configuration retrieved successfully.");
                }
            }
            catch (Exception ex)
            {
                // Write any errors to the error stream.
                Console.Error.WriteLine(ex.Message);
            }
        }
    }
}