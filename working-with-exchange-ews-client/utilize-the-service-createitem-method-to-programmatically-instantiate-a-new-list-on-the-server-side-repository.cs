using System;
using System.Net;
using Aspose.Email;
using Aspose.Email.Clients.Exchange.WebService;
using Aspose.Email.Mapi;

class Program
{
    static void Main()
    {
        try
        {
            // Initialize the EWS client
            string mailboxUri = "https://exchange.example.com/EWS/Exchange.asmx";
            string username = "user@example.com";
            string password = "password";

            using (IEWSClient client = EWSClient.GetEWSClient(mailboxUri, username, password))
            {
                // Create a MAPI message item to represent the new list entry
                using (MapiMessage mapiMessage = new MapiMessage())
                {
                    mapiMessage.Subject = "New List Item";
                    mapiMessage.Body = "Created via IEWSClient.CreateItem method.";

                    // Create the item on the server
                    client.CreateItem(mapiMessage);
                }

                Console.WriteLine("Item created successfully.");
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
