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
            // Define output MSG file path
            string outputPath = "calendar.msg";

            // Ensure the directory for the output file exists
            string outputDir = Path.GetDirectoryName(Path.GetFullPath(outputPath));
            if (!Directory.Exists(outputDir))
            {
                Directory.CreateDirectory(outputDir);
            }

            // Create a MAPI calendar item
            DateTime start = new DateTime(2024, 12, 1, 10, 0, 0);
            DateTime end = new DateTime(2024, 12, 1, 11, 0, 0);

            using (MapiCalendar calendar = new MapiCalendar())
            {
                // Set required properties
                calendar.Subject = "Project Kickoff";
                calendar.Body = "Discuss project goals and milestones.";
                calendar.Location = "Conference Room A";
                calendar.StartDate = start;
                calendar.EndDate = end;

                // Prepare attendees container
                MapiCalendarAttendees attendees = new MapiCalendarAttendees
                {
                    AppointmentRecipients = new MapiRecipientCollection()
                };

                // Attendee 1 – Accepted
                attendees.AppointmentRecipients.Add("alice@example.com", "Alice", MapiRecipientType.MAPI_TO);
                attendees.AppointmentRecipients[0].RecipientTrackStatus = MapiRecipientTrackStatus.Accepted;

                // Attendee 2 – Declined
                attendees.AppointmentRecipients.Add("bob@example.com", "Bob", MapiRecipientType.MAPI_TO);
                attendees.AppointmentRecipients[1].RecipientTrackStatus = MapiRecipientTrackStatus.Declined;

                // Attendee 3 – Tentative
                attendees.AppointmentRecipients.Add("carol@example.com", "Carol", MapiRecipientType.MAPI_TO);
                attendees.AppointmentRecipients[2].RecipientTrackStatus = MapiRecipientTrackStatus.Tentative;

                // Assign attendees to the calendar
                calendar.Attendees = attendees;

                // Save the calendar as MSG
                MapiCalendarSaveOptions saveOptions = MapiCalendarSaveOptions.DefaultMsg;
                calendar.Save(outputPath, saveOptions);
                Console.WriteLine($"Calendar saved successfully to '{outputPath}'.");
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
