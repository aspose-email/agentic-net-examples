using Aspose.Email;
using System;
using System.Collections.Generic;
using System.Net;
using Aspose.Email.Clients.Exchange.WebService;

namespace AsposeEmailEwsSample
{
    class Program
    {
        static void Main()
        {
            try
            {
                // Mailbox URI of the Exchange server
                string mailboxUri = "https://mail.example.com/EWS/Exchange.asmx";


                // Skip external calls when placeholder credentials are used
                if (mailboxUri.Contains("example.com"))
                {
                    Console.Error.WriteLine("Placeholder credentials detected. Skipping external calls.");
                    return;
                }

                // Credentials for authentication
                NetworkCredential credentials = new NetworkCredential("username", "password");

                // Proxy settings (null if not required)
                WebProxy proxy = null;

                // Optional custom headers to be sent with each EWS request
                Dictionary<string, string> customHeaders = new Dictionary<string, string>
                {
                    { "X-Custom-Header", "MyValue" }
                };

                // Initialize the EWS client with the specified parameters
                using (IEWSClient client = EWSClient.GetEWSClient(mailboxUri, credentials, proxy, customHeaders))
                {
                    // The client is ready for further operations
                    Console.WriteLine("EWS client initialized successfully.");
                }
            }
            catch (Exception ex)
            {
                // Output any errors without crashing the application
                Console.Error.WriteLine($"Error: {ex.Message}");
            }
        }
    }
}
