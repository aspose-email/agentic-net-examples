using System.Net;
using System;
using System.Collections.Generic;
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
                // Initialize the EWS client using the factory method.
                // Replace the placeholder values with actual server URI and credentials.
                using (IEWSClient client = EWSClient.GetEWSClient(
                    "https://exchange.example.com/EWS/Exchange.asmx",
                    "username",
                    "password"))
                {
                    // Prepare a list of unique message URIs to retrieve.
                    System.Collections.Generic.List<string> messageUris = new System.Collections.Generic.List<string>
                    {
                        "uniqueUri1",
                        "uniqueUri2"
                    };

                    // Fetch the messages corresponding to the supplied URIs.
                    MailMessageCollection messages = client.FetchMessages(messageUris);

                    // Iterate through the fetched messages and output their subjects.
                    foreach (Aspose.Email.MailMessage message in messages)
                    {
                        Console.WriteLine("Subject: " + message.Subject);
                    }
                }
            }
            catch (Exception ex)
            {
                // Log any unexpected errors to the error output.
                Console.Error.WriteLine("Error: " + ex.Message);
            }
        }
    }
}