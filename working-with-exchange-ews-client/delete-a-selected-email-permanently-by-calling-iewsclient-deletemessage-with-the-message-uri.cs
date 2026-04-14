using System;
using Aspose.Email;
using Aspose.Email.Clients.Exchange.WebService;
using Aspose.Email.Clients.Exchange;

class Program
{
    static void Main(string[] args)
    {
        try
        {
            // EWS service URL and credentials
            string serviceUrl = "https://mail.example.com/EWS/Exchange.asmx";
            string username = "username";
            string password = "password";

            // Create the EWS client
            using (IEWSClient client = EWSClient.GetEWSClient(serviceUrl, username, password))
            {
                // URI of the message to be deleted
                string messageUri = "https://mail.example.com/EWS/Exchange.asmx/MessageId";


                // Skip external calls when placeholder credentials are used
                if (serviceUrl.Contains("example.com") || username == "username" || password == "password" || messageUri.Contains("example.com"))
                {
                    Console.Error.WriteLine("Placeholder credentials detected. Skipping external calls.");
                    return;
                }

                // Delete the message permanently by moving it to Deleted Items
                DeletionOptions deleteOptions = new DeletionOptions(DeletionType.MoveToDeletedItems);
                client.DeleteItem(messageUri, deleteOptions);

                Console.WriteLine("Message deleted successfully.");
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
