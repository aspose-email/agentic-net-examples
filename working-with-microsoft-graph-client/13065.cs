using System;
using System.IO;
using Aspose.Email;
using Aspose.Email.Mapi;
using Aspose.Email.Clients;
using Aspose.Email.Clients.Graph;

class Program
{
    static void Main()
    {
        try
        {
            // Input MSG file path
            string msgPath = "event.msg";

            // Placeholder credentials – replace with real values
            string clientId = "your-client-id";
            string clientSecret = "your-client-secret";
            string refreshToken = "your-refresh-token";

            // Target calendar identifier in Microsoft Graph
            string calendarId = "your-calendar-id";

            // Guard file existence
            if (!File.Exists(msgPath))
            {
                try
                {
                    MapiCalendar placeholderCalendar = new MapiCalendar(
                        "Placeholder Location",
                        "Placeholder Summary",
                        "Placeholder Description",
                        DateTime.Now,
                        DateTime.Now.AddHours(1));
                    placeholderCalendar.Save(msgPath, new MapiCalendarMsgSaveOptions());
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Error creating placeholder MSG: {ex.Message}");
                    return;
                }

                Console.Error.WriteLine($"Input file not found: {msgPath}");
                return;
            }

            // Skip external calls when placeholders are present
            if (clientId.StartsWith("your-") || clientSecret.StartsWith("your-") || refreshToken.StartsWith("your-"))
            {
                Console.Error.WriteLine("Placeholder credentials detected. Skipping Graph operation.");
                return;
            }

            // Load the MSG file
            using (MapiMessage msg = MapiMessage.Load(msgPath))
            {
                // Verify the MSG contains a calendar item
                if (msg.SupportedType != MapiItemType.Calendar)
                {
                    // Create a minimal placeholder ICS file
                    string placeholderIcs = "placeholder.ics";
                    File.WriteAllText(placeholderIcs, "BEGIN:VCALENDAR\r\nEND:VCALENDAR");
                    Console.WriteLine($"MSG is not a calendar. Placeholder ICS created at {placeholderIcs}");
                    return;
                }

                // Convert to MapiCalendar
                MapiCalendar mapiCalendar = (MapiCalendar)msg.ToMapiMessageItem();

                // Ensure the body (description) is set
                if (string.IsNullOrEmpty(mapiCalendar.Body))
                {
                    mapiCalendar.Body = "Imported from MSG file.";
                }

                // Create token provider (3‑argument overload)
                Aspose.Email.Clients.ITokenProvider tokenProvider = Aspose.Email.Clients.TokenProvider.Outlook.GetInstance(
                    clientId,
                    clientSecret,
                    refreshToken);

                // Initialize Graph client
                using (IGraphClient client = GraphClient.GetClient(tokenProvider, null))
                {
                    // Create the calendar event in the specified calendar
                    MapiCalendar created = client.CreateCalendarItem(calendarId, mapiCalendar);
                    Console.WriteLine($"Created calendar event with subject: {created.Subject}");
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine(ex.Message);
        }
    }
}
