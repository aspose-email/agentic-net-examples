using Aspose.Email.Clients.Exchange.WebService;
using System;
using Aspose.Email;
class Program
{
    static void Main()
    {
        try
        {
            // Initialize the Exchange client with server URI and credentials
            string hostUri = "https://exchange.example.com/EWS/Exchange.asmx";
            string username = "user@example.com";
            string password = "password";

            using (IEWSClient client = EWSClient.GetEWSClient(hostUri, username, password))
            {
                // Unique identifier (URI) of the email to retrieve
                string messageUri = "https://exchange.example.com/EWS/MessageId";


                // Skip external calls when placeholder credentials are used
                if (hostUri.Contains("example.com") || username.Contains("example.com") || password == "password" || messageUri.Contains("example.com"))
                {
                    Console.Error.WriteLine("Placeholder credentials detected. Skipping external calls.");
                    return;
                }

                // Retrieve the email as a MailMessage object
                MailMessage mapiMessage = client.FetchMessage(messageUri);

                // Example usage: display the subject of the retrieved message
                Console.WriteLine("Subject: " + mapiMessage.Subject);
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine("Error: " + ex.Message);
        }
    }
}
