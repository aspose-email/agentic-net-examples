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
            // Define the EWS service URL and credentials
            string mailboxUri = "https://example.com/EWS/Exchange.asmx";
            NetworkCredential credentials = new NetworkCredential("username", "password");

            // Create the EWS client using the factory method
            using (IEWSClient client = EWSClient.GetEWSClient(mailboxUri, credentials))
            {
                // The IEWSClient type does not expose a TraceEnabled property.
                // If tracing were supported, it would be enabled here.
                // Example (if available): client.EnableLogger = true;

                Console.WriteLine("EWS client initialized successfully.");
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}