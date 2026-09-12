using Aspose.Email.Calendar;
using System;
using System.IO;
using Aspose.Email;
using Aspose.Email.Mapi;

class Program
{
    static void Main()
    {
        try
        {
            // Create a new MAPI calendar
            using (MapiCalendar calendar = new MapiCalendar())
            {
                calendar.Subject = "Team Meeting";
                calendar.StartDate = new DateTime(2023, 10, 1, 10, 0, 0);
                calendar.EndDate = new DateTime(2023, 10, 1, 11, 0, 0);
                calendar.Location = "Conference Room";

                // Initialize attendees collection
                calendar.Attendees = new MapiCalendarAttendees();

                // Add an attendee using the supported method
                calendar.Attendees.AppointmentRecipients.Add(
                    "john.doe@example.com",
                    "John Doe",
                    MapiRecipientType.MAPI_TO);

                // Mark the attendee as accepted
                MapiRecipient attendee = calendar.Attendees.AppointmentRecipients[0];
                attendee.RecipientTrackStatus = MapiRecipientTrackStatus.Accepted;

                // Prepare output path and ensure directory exists
                string outputPath = "calendar.msg";
                string directory = Path.GetDirectoryName(outputPath);
                if (!string.IsNullOrEmpty(directory) && !Directory.Exists(directory))
                {
                    Directory.CreateDirectory(directory);
                }

                // Save the calendar to a MSG file
                try
                {
                    calendar.Save(outputPath, MapiCalendarSaveOptions.DefaultMsg);
                    Console.WriteLine($"Calendar saved to {outputPath}");
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to save calendar: {ex.Message}");
                    return;
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
