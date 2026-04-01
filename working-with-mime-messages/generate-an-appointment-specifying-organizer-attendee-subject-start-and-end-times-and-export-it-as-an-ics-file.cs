using System;
using System.IO;
using Aspose.Email;
using Aspose.Email.Calendar;

class Program
{
    static void Main()
    {
        try
        {
            // Define output file path
            string outputPath = "appointment.ics";

            // Ensure the directory for the output file exists
            string outputDir = Path.GetDirectoryName(Path.GetFullPath(outputPath));
            if (!string.IsNullOrEmpty(outputDir) && !Directory.Exists(outputDir))
            {
                Directory.CreateDirectory(outputDir);
            }

            // Prepare attendees collection
            MailAddressCollection attendees = new MailAddressCollection();
            attendees.Add(new MailAddress("attendee1@example.com"));
            attendees.Add(new MailAddress("attendee2@example.com"));

            // Create the appointment with location, summary, description, start/end, organizer, attendees
            Appointment appointment = new Appointment(
                location: "Conference Room 1",
                summary: "Project Kickoff",
                description: "Discuss project goals and timelines.",
                startDate: new DateTime(2024, 5, 20, 10, 0, 0),
                endDate: new DateTime(2024, 5, 20, 11, 0, 0),
                organizer: new MailAddress("organizer@example.com"),
                attendees: attendees
            );

            // Save the appointment as an iCalendar (ICS) file
            appointment.Save(outputPath);
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
