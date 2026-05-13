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
            string outputPath = "meeting.ics";

            // Ensure the output directory exists
            string outputDirectory = Path.GetDirectoryName(outputPath);
            if (!string.IsNullOrEmpty(outputDirectory) && !Directory.Exists(outputDirectory))
            {
                Directory.CreateDirectory(outputDirectory);
            }

            // Prepare attendees collection
            Aspose.Email.MailAddressCollection attendees = new Aspose.Email.MailAddressCollection();
            attendees.Add(new Aspose.Email.MailAddress("alice@example.com"));
            attendees.Add(new Aspose.Email.MailAddress("bob@example.com"));

            // Create the appointment
            Appointment appointment = new Appointment(
                location: "Conference Room",
                startDate: new DateTime(2024, 10, 15, 10, 0, 0),
                endDate: new DateTime(2024, 10, 15, 11, 0, 0),
                organizer: new Aspose.Email.MailAddress("organizer@example.com"),
                attendees: attendees);

            // Set plain‑text description
            appointment.Description = "Team meeting to discuss project status.";

            // Set HTML description (will be stored as X‑ALT‑DESC)
            appointment.HtmlDescription = "<html><body><h1>Team Meeting</h1><p>Discuss project status.</p></body></html>";

            // Set additional properties
            appointment.Summary = "Team Meeting";

            // Save the appointment as iCalendar (ICS) file
            appointment.Save(outputPath, AppointmentSaveFormat.Ics);
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
