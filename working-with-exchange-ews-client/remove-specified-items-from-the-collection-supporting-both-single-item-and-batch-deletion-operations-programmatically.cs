using System;
using System.Collections.Generic;
using Aspose.Email;
using Aspose.Email.Clients.Exchange.WebService;
using Aspose.Email.Clients.Exchange;

class Program
{
    static void Main()
    {
        try
        {
            // Initialize the EWS client (replace with real credentials and URL)
            string serviceUrl = "https://ews.example.com/EWS/Exchange.asmx";
            string username = "user@example.com";
            string password = "password";

            IEWSClient client = EWSClient.GetEWSClient(serviceUrl, username, password);

            // ----- Single-item deletion -----
            string singleItemUri = "https://ews.example.com/EWS/Item/12345";

            // Skip external calls when placeholder credentials are used
            if (serviceUrl.Contains("example.com") || username.Contains("example.com") || password == "password" || singleItemUri.Contains("example.com"))
            {
                Console.Error.WriteLine("Placeholder credentials detected. Skipping external calls.");
                return;
            }

            client.DeleteItems(new List<string> { singleItemUri }, DeletionOptions.DeletePermanently);

            // ----- Batch deletion (multiple items) -----
            List<string> batchItemUris = new List<string>
            {
                "https://ews.example.com/EWS/Item/12346",
                "https://ews.example.com/EWS/Item/12347",
                "https://ews.example.com/EWS/Item/12348"
            };
            client.DeleteItems(batchItemUris, DeletionOptions.DeletePermanently);
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
