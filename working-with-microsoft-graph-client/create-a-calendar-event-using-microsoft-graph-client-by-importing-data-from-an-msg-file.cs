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
            // Paths and credentials (replace with real values or keep placeholders)
            string msgFilePath = "appointment.msg";
            string clientId = "clientId";
            string clientSecret = "clientSecret";
            string refreshToken = "refreshToken";
            string tenantId = "tenantId";

            // Verify MSG file exists
            if (!File.Exists(msgFilePath))
            {
                Console.Error.WriteLine($"File not found: {msgFilePath}");
                return;
            }

            // Create token provider (Outlook OAuth)
            Aspose.Email.Clients.ITokenProvider tokenProvider = Aspose.Email.Clients.TokenProvider.Outlook.GetInstance(
                clientId, clientSecret, refreshToken);

            // Initialize Graph client
            using (IGraphClient graphClient = GraphClient.GetClient(tokenProvider, tenantId))
            {
                // List calendars and pick the first one
                var calendars = graphClient.ListCalendars();
                if (calendars == null || calendars.Count == 0)
                {
                    Console.Error.WriteLine("No calendars available.");
                    return;
                }

                // CalendarInfo uses ItemId as identifier
                string calendarId = calendars[0].ItemId;

                // Load MSG file as MapiMessage
                using (MapiMessage msg = MapiMessage.Load(msgFilePath))
                {
                    // Ensure the message is a calendar item
                    if (msg.SupportedType != MapiItemType.Calendar)
                    {
                        Console.Error.WriteLine("The MSG file does not contain a calendar item.");
                        return;
                    }

                    // Convert to MapiCalendar
                    MapiCalendar calendar = (MapiCalendar)msg.ToMapiMessageItem();

                    // Create the calendar event in the selected calendar
                    MapiCalendar created = graphClient.CreateCalendarItem(calendarId, calendar);
                    Console.WriteLine("Calendar event created with subject: " + created.Subject);
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}