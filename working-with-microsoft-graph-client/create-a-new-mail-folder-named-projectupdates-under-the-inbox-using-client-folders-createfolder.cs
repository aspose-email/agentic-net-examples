using Aspose.Email.Clients.Exchange.Dav;
using Aspose.Email;
using System;
using Aspose.Email.Clients.Exchange;

class Program
{
    static void Main()
    {
        try
        {
            // Placeholder connection details
            string serviceUri = "https://exchange.example.com/ews/Exchange.asmx";
            string username = "username";
            string password = "password";

            // Skip execution if placeholders are detected
            if (serviceUri.Contains("example.com") || username.Contains("YOUR_") || password.Contains("YOUR_"))
            {
                Console.WriteLine("Placeholder credentials detected. Skipping execution.");
                return;
            }

            // Create and use the Exchange client
            using (ExchangeClient client = new ExchangeClient(serviceUri, username, password))
            {
                try
                {
                    // Create a new folder named "ProjectUpdates" under the Inbox
                    client.CreateFolder("Inbox", "ProjectUpdates");
                    Console.WriteLine("Folder 'ProjectUpdates' created successfully under Inbox.");
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Error creating folder: {ex.Message}");
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
