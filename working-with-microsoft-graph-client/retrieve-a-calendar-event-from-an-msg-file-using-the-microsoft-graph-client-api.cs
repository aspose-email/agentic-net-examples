using System;
using System.IO;
using Aspose.Email;
using Aspose.Email.Mapi;
using Aspose.Email.Clients;
using Aspose.Email.Clients.Graph;

class Program
{
    static void Main(string[] args)
    {
        try
        {
            // Path to the MSG file containing the calendar event
            string msgPath = "event.msg";

            // Verify that the file exists before attempting to load it
            if (!File.Exists(msgPath))
            {
                Console.Error.WriteLine($"File not found: {msgPath}");
                return;
            }

            // Load the MSG file and extract the server item identifier
            string itemId;
            using (MapiMessage msg = MapiMessage.Load(msgPath))
            {
                itemId = msg.ItemId;
                if (string.IsNullOrEmpty(itemId))
                {
                    Console.Error.WriteLine("The MSG file does not contain a valid ItemId.");
                    return;
                }
            }

            // Create a token provider for Microsoft Graph authentication
            Aspose.Email.Clients.ITokenProvider tokenProvider =
                Aspose.Email.Clients.TokenProvider.Outlook.GetInstance(
                    "clientId",          // Replace with your Azure AD client ID
                    "clientSecret",      // Replace with your Azure AD client secret
                    "refreshToken");     // Replace with a valid refresh token

            // Initialize the Graph client
            using (IGraphClient client = GraphClient.GetClient(tokenProvider, null))
            {
                // Retrieve the calendar event from the server using the extracted ItemId
                using (MapiCalendar calendar = client.FetchCalendarItem(itemId))
                {
                    if (calendar != null)
                    {
                        Console.WriteLine($"Subject: {calendar.Subject}");
                        Console.WriteLine($"Start: {calendar.StartDate}");
                        Console.WriteLine($"End: {calendar.EndDate}");
                    }
                    else
                    {
                        Console.Error.WriteLine("Calendar item not found on the server.");
                    }
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
