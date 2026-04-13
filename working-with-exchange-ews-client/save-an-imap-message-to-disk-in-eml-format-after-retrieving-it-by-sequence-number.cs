using System;
using System.IO;
using Aspose.Email;
using Aspose.Email.Clients.Exchange;
using Aspose.Email.Clients.Exchange.WebService;

class Program
{
    static void Main(string[] args)
    {
        try
        {
            // Connection parameters
            string mailboxUri = "https://mail.example.com/EWS/Exchange.asmx";
            string username = "user@example.com";
            string password = "password";

            // Output file path
            string outputDirectory = "Output";

            // Skip external calls when placeholder credentials are used
            if (mailboxUri.Contains("example.com") || username.Contains("example.com") || password == "password")
            {
                Console.Error.WriteLine("Placeholder credentials detected. Skipping external calls.");
                return;
            }

            string outputFilePath = Path.Combine(outputDirectory, "message.eml");

            // Ensure the output directory exists
            if (!Directory.Exists(outputDirectory))
            {
                Directory.CreateDirectory(outputDirectory);
            }

            // Create and use the EWS client
            using (IEWSClient client = EWSClient.GetEWSClient(mailboxUri, username, password))
            {
                // Retrieve messages from the Inbox folder
                ExchangeMessageInfoCollection messages = client.ListMessages("Inbox");

                if (messages != null && messages.Count > 0)
                {
                    // Get the URI of the first message
                    string messageUri = messages[0].UniqueUri;

                    // Save the message in .eml format
                    client.SaveMessage(messageUri, outputFilePath);
                    Console.WriteLine("Message saved to: " + outputFilePath);
                }
                else
                {
                    Console.WriteLine("No messages found in the Inbox folder.");
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine("Error: " + ex.Message);
        }
    }
}
