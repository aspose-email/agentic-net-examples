using System;
using System.Collections.Generic;
using System.Net;
using Aspose.Email;
using Aspose.Email.Clients.Exchange.WebService;

class Program
{
    static void Main(string[] args)
    {
        try
        {
            // Define connection parameters (replace with actual values)
            string mailboxUri = "https://example.com/EWS/Exchange.asmx";
            string username = "user@example.com";
            string password = "password";

            // Create EWS client
            using (IEWSClient client = EWSClient.GetEWSClient(mailboxUri, username, password))
            {
                // URI of the message to retrieve (replace with actual message URI)
                string messageUri = "https://example.com/EWS/MessageId";


                // Skip external calls when placeholder credentials are used
                if (mailboxUri.Contains("example.com") || username.Contains("example.com") || password == "password" || messageUri.Contains("example.com"))
                {
                    Console.Error.WriteLine("Placeholder credentials detected. Skipping external calls.");
                    return;
                }

                // Fetch the message (extended properties are available via Headers)
                MailMessage mailMessage = client.FetchMessage(messageUri);

                // Log each header (standard and custom extended properties)
                foreach (string headerName in mailMessage.Headers.Keys)
                {
                    string headerValue = mailMessage.Headers[headerName];
                    Console.WriteLine($"{headerName}: {headerValue}");
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine(ex.Message);
        }
    }
}
