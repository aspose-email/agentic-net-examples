using System;
using System.IO;
using Aspose.Email;
using Aspose.Email.Mapi;

namespace CalendarLoader
{
    // Author: Generated example for loading a calendar entry from an MSG file.
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                // Define the path to the MSG file containing the calendar entry.
                string msgPath = "calendar.msg";

                // Verify that the file exists before attempting to load it.
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

                // Load the MSG file as a MapiMessage.
                MapiMessage msg;
                try
                {
                    msg = MapiMessage.Load(msgPath);
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to load MSG file: {ex.Message}");
                    return;
                }

                // Check whether the loaded message contains a calendar item.
                if (msg.SupportedType == MapiItemType.Calendar)
                {
                    // Convert the MapiMessage to a MapiCalendar object.
                    MapiCalendar calendar = (MapiCalendar)msg.ToMapiMessageItem();

                    // Display basic calendar information.
                    Console.WriteLine($"Subject: {calendar.Subject}");
                    Console.WriteLine($"Start Date: {calendar.StartDate}");
                    Console.WriteLine($"End Date: {calendar.EndDate}");
                    Console.WriteLine($"Location: {calendar.Location}");
                }
                else
                {
                    Console.WriteLine("The loaded MSG file does not contain a calendar entry.");
                }
            }
            catch (Exception ex)
            {
                // Catch any unexpected exceptions and report them.
                Console.Error.WriteLine($"Unexpected error: {ex.Message}");
            }
        }
    }
}
