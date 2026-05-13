using Aspose.Email.Calendar;
using Aspose.Email;
using System;
using System.IO;
using Aspose.Email.Mapi;

class Program
{
    static void Main()
    {
        try
        {
            string outputPath = "output.ics";

            // Ensure the output directory exists
            string directory = Path.GetDirectoryName(outputPath);
            if (!string.IsNullOrEmpty(directory) && !Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }

            // Create a MAPI calendar and set basic properties
            using (MapiCalendar calendar = new MapiCalendar())
            {
                calendar.Location = "Conference Room A";
                calendar.Subject = "Project Meeting";
                calendar.StartDate = new DateTime(2023, 10, 20, 10, 0, 0);
                calendar.EndDate = new DateTime(2023, 10, 20, 11, 0, 0);
                calendar.ReminderSet = true;
                calendar.ReminderDelta = 15; // minutes before start

                // Add attendees
                MapiCalendarAttendees attendees = new MapiCalendarAttendees();
                attendees.AppointmentRecipients = new MapiRecipientCollection();
                attendees.AppointmentRecipients.Add("alice@example.com", "Alice", MapiRecipientType.MAPI_TO);
                attendees.AppointmentRecipients.Add("bob@example.com", "Bob", MapiRecipientType.MAPI_TO);
                calendar.Attendees = attendees;

                // Save the calendar to an iCalendar (.ics) file
                calendar.Save(outputPath, MapiCalendarSaveOptions.DefaultIcs);
                Console.WriteLine($"Calendar saved to {outputPath}");
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
