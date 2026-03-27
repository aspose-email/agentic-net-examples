using System;
using System.IO;
using System.Net;
using Aspose.Email;
using Aspose.Email.Clients;
using Aspose.Email.Clients.Graph;
using Aspose.Email.Mapi;

class Program
{
    static void Main()
    {
        try
        {
            // File path for the MSG file containing the calendar event
            string msgPath = "calendar.msg";

            // Guard against missing file
            if (!File.Exists(msgPath))
            {
                Console.Error.WriteLine($"File not found: {msgPath}");
                return;
            }

            // Load the MSG file
            using (MapiMessage msg = MapiMessage.Load(msgPath))
            {
                // Verify that the message contains a calendar item
                if (msg.SupportedType != MapiItemType.Calendar)
                {
                    Console.Error.WriteLine("The MSG file does not contain a calendar event.");
                    return;
                }

                // Convert the MAPI message to a MapiCalendar object
                MapiCalendar localCalendar = (MapiCalendar)msg.ToMapiMessageItem();

                // Prepare Microsoft Graph client (placeholder credentials)
                string clientId = "clientId";
                string clientSecret = "clientSecret";
                string refreshToken = "refreshToken";
                string tenantId = "tenantId";

                // Token provider (Outlook family) – exactly three arguments as required
                Aspose.Email.Clients.ITokenProvider tokenProvider = Aspose.Email.Clients.TokenProvider.Outlook.GetInstance(
                    clientId, clientSecret, refreshToken);

                // Create the Graph client
                using (IGraphClient graphClient = GraphClient.GetClient(tokenProvider, tenantId))
                {
                    // Retrieve the same calendar event from Microsoft Graph using its ItemId
                    // (ItemId is the identifier used by the server)
                    MapiCalendar serverCalendar = graphClient.FetchCalendarItem(localCalendar.ItemId);

                    // Output some details of the retrieved calendar event
                    Console.WriteLine("Subject: " + serverCalendar.Subject);
                    Console.WriteLine("Location: " + serverCalendar.Location);
                    Console.WriteLine("Start: " + serverCalendar.StartDate);
                    Console.WriteLine("End: " + serverCalendar.EndDate);
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine("Error: " + ex.Message);
        }
    }
}