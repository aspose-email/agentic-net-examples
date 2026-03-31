using Aspose.Email.Mapi;
using System;
using System.IO;
using Aspose.Email;
using Aspose.Email.Clients;
using Aspose.Email.Clients.Graph;

class Program
{
    static void Main(string[] args)
    {
        try
        {
            // Placeholder credentials – replace with real values for actual use.
            string clientId = "YOUR_CLIENT_ID";
            string clientSecret = "YOUR_CLIENT_SECRET";
            string refreshToken = "YOUR_REFRESH_TOKEN";
            string calendarEventId = "EVENT_ID";

            // Skip execution when placeholder credentials are detected.
            if (string.IsNullOrWhiteSpace(clientId) || clientId.StartsWith("YOUR_"))
            {
                Console.Error.WriteLine("Placeholder credentials detected. Skipping Graph call.");
                return;
            }

            // Create token provider (3‑argument overload is the verified one).
            Aspose.Email.Clients.ITokenProvider tokenProvider = TokenProvider.Outlook.GetInstance(clientId, clientSecret, refreshToken);

            // Create Graph client and ensure it is disposed.
            try
            {
                using (IGraphClient client = GraphClient.GetClient(tokenProvider, null))
                {
                    // Fetch the calendar event by its Graph identifier.
                    MapiCalendar calendar = client.FetchCalendarItem(calendarEventId);

                    // Display basic calendar information.
                    Console.WriteLine("Subject: " + calendar.Subject);
                    Console.WriteLine("Body: " + calendar.Body);
                    Console.WriteLine("Start: " + calendar.StartDate);
                    Console.WriteLine("End: " + calendar.EndDate);

                    // Optional: save the calendar as an MSG file.
                    string outputPath = "calendar.msg";
                    string outputDirectory = Path.GetDirectoryName(Path.GetFullPath(outputPath));

                    if (!Directory.Exists(outputDirectory))
                    {
                        Directory.CreateDirectory(outputDirectory);
                    }

                    try
                    {
                        calendar.Save(outputPath, MapiCalendarSaveOptions.DefaultMsg);
                        Console.WriteLine("Calendar saved to " + outputPath);
                    }
                    catch (Exception saveEx)
                    {
                        Console.Error.WriteLine("Failed to save calendar: " + saveEx.Message);
                    }
                }
            }
            catch (Exception clientEx)
            {
                Console.Error.WriteLine("Graph client error: " + clientEx.Message);
                return;
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine("Unexpected error: " + ex.Message);
        }
    }
}
