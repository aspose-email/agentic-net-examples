using System;
using Aspose.Email;
using Aspose.Email.Clients.Exchange.WebService;
using System.Net;

namespace AsposeEmailExample
{
    class Program
    {
        static void Main()
        {
            try
            {
                // Exchange server endpoint and credentials
                string serviceUrl = "https://exchange.example.com/EWS/Exchange.asmx";
                string username = "user@example.com";
                string password = "password";

                // Create the EWS client using the factory method
                using (IEWSClient client = EWSClient.GetEWSClient(serviceUrl, username, password))
                {
                    // Example operation: retrieve and display the server version
                    string versionInfo = client.GetVersionInfo();
                    Console.WriteLine("Exchange Server Version: " + versionInfo);
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine("Error: " + ex.Message);
            }
        }
    }
}
