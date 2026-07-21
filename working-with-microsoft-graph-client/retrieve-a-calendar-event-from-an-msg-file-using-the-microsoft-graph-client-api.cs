using Aspose.Email.Clients;
using Aspose.Email.Clients.Graph;
using Aspose.Email;
using System;
using System.IO;
using Aspose.Email.Mapi;

class Program
{
    static void Main()
    {
        // Path to the MSG file containing the calendar event
        string msgPath = "calendar.msg";

        // Guard against missing input file
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
                    if (string.IsNullOrEmpty(placeholderCalendar.Subject))
                    {
                        placeholderCalendar.Subject = "Placeholder Summary";
                    }
                    if (string.IsNullOrEmpty(placeholderCalendar.Body))
                    {
                        placeholderCalendar.Body = "Placeholder Description";
                    }
                    placeholderCalendar.Save(msgPath, MapiCalendarSaveOptions.DefaultMsg);
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Error creating placeholder MSG: {ex.Message}");
                    return;
                }

            Console.Error.WriteLine($"Input file not found: {msgPath}");
            return;
        }

        try
        {
            // Load the MSG file
            MapiMessage msg = MapiMessage.Load(msgPath);

            // Verify that the MSG contains a calendar item
            if (msg.SupportedType == MapiItemType.Calendar)
            {
                // Convert the MAPI message to a MapiCalendar object
                MapiCalendar calendar = (MapiCalendar)msg.ToMapiMessageItem();

                // Display basic calendar information
                Console.WriteLine($"Subject: {calendar.Subject}");
                Console.WriteLine($"Start: {calendar.StartDate}");
                Console.WriteLine($"End: {calendar.EndDate}");

                // Placeholder: Here you could use Microsoft Graph client APIs
                // (e.g., IGraphClient) to upload or query the event in a
                // Microsoft 365 tenant. The required Graph client setup is
                // version‑specific and not demonstrated in this minimal example.
            }
            else
            {
                Console.WriteLine("The MSG file does not contain a calendar item.");
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error processing MSG file: {ex.Message}");
        }
    }
}
