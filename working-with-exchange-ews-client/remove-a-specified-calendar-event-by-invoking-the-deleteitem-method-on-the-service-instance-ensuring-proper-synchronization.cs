using System;
using System.Net;
using Aspose.Email;
using Aspose.Email.Clients.Exchange.WebService;
using Aspose.Email.Clients.Exchange;

namespace AsposeEmailExample
{
    class Program
    {
        static void Main(string[] args)
        {
        var credential = new System.Net.NetworkCredential("username", "password", "domain");

            try
            {
                // EWS service URL and user credentials
                string ewsUrl = "https://exchange.example.com/EWS/Exchange.asmx";
                string userName = "user@example.com";
                string userPassword = "password";

                NetworkCredential networkCredential = new NetworkCredential(userName, userPassword);

                // Create the EWS client
                using (IEWSClient ewsClient = EWSClient.GetEWSClient(ewsUrl, networkCredential))
                {
                    // URI of the calendar event to delete
                    string eventUri = "https://exchange.example.com/EWS/Exchange.asmx/UniqueItemId";

                    // Delete the event permanently
                    ewsClient.DeleteItem(eventUri, DeletionOptions.DeletePermanently);

                    Console.WriteLine("Calendar event deleted successfully.");
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Error: {ex.Message}");
            }
        }
    }
}