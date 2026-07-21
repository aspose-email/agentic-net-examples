using Aspose.Email.Calendar;
using Aspose.Email;
using System;
using System.IO;
using Aspose.Email.Mapi;
using Aspose.Email.Storage.Pst;

class Program
{
    static void Main()
    {
        string pstPath = "sample.pst";

        try
        {
            // Create PST file if it does not exist
            if (!File.Exists(pstPath))
            {
                PersonalStorage.Create(pstPath, FileFormatVersion.Unicode);
            }

            // Open PST storage
            using (PersonalStorage pst = PersonalStorage.FromFile(pstPath))
            {
                // Get or create Calendar folder
                FolderInfo calendarFolder;
                try
                {
                    calendarFolder = pst.RootFolder.GetSubFolder("Calendar");
                }
                catch (Exception)
                {
                    calendarFolder = pst.RootFolder.AddSubFolder("Calendar");
                }

                // Build attendees collection
                MapiCalendarAttendees attendees = new MapiCalendarAttendees();
                attendees.AppointmentRecipients = new MapiRecipientCollection();
                attendees.AppointmentRecipients.Add(
                    "attendee1@example.com",
                    "Attendee One",
                    MapiRecipientType.MAPI_TO);
                attendees.AppointmentRecipients.Add(
                    "attendee2@example.com",
                    "Attendee Two",
                    MapiRecipientType.MAPI_TO);

                // Create calendar item
                MapiCalendar calendar = new MapiCalendar(
                    "Conference Room",                     // location
                    "Project Kickoff",                     // summary
                    "Discuss project scope and timeline.", // description
                    new DateTime(2024, 8, 1, 10, 0, 0),    // start time
                    new DateTime(2024, 8, 1, 11, 0, 0),    // end time
                    "organizer@example.com",               // organizer email
                    null);                                 // attendees set later

                // Assign attendees
                calendar.Attendees = attendees;

                // Add calendar item to PST
                calendarFolder.AddMapiMessageItem(calendar);
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
