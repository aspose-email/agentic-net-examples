using System;
using System.Net;
using Aspose.Email;
using Aspose.Email.Clients.Exchange.WebService;

namespace AsposeEmailExample
{
    class Program
    {
        static void Main()
        {
            try
            {
                // Define the EWS service URL and credentials.
                string mailboxUri = "https://exchange.example.com/EWS/Exchange.asmx";
                NetworkCredential credentials = new NetworkCredential("username", "password");

                // Create the EWS client using the factory method.
                using (IEWSClient client = EWSClient.GetEWSClient(mailboxUri, credentials))
                {
                    // Retrieve a user configuration.
                    // Using the default enum value to avoid referencing undefined members.
                    UserConfiguration userConfig = client.GetUserConfiguration(default(UserConfigurationName));

                    // Output a simple confirmation.
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
