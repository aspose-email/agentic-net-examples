using System;
using System.IO;
using Aspose.Email;
using Aspose.Email.Calendar;

class Program
{
    static void Main(string[] args)
    {
        try
        {
            // Define custom output file name
            string outputPath = "CustomCalendar.ics";

            // Ensure the target directory exists
            string directory = Path.GetDirectoryName(outputPath);
            if (!string.IsNullOrEmpty(directory) && !Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }

            // Create organizer and attendees
            MailAddress organizer = new MailAddress("organizer@example.com");
            MailAddressCollection attendees = new MailAddressCollection();
            attendees.Add(new MailAddress("attendee1@example.com"));
            attendees.Add(new MailAddress("attendee2@example.com"));

            // Create an appointment
            Appointment appointment = new Appointment(
                "Team Meeting",
                new DateTime(2023, 10, 20, 10, 0, 0),
                new DateTime(2023, 10, 20, 11, 0, 0),
                organizer,
                attendees);
            appointment.Summary = "Project Sync";
            appointment.Description = "Discuss project milestones.";

            // Save the appointment to an .ics file with the custom name
            appointment.Save(outputPath);
            Console.WriteLine($"Appointment exported to \"{outputPath}\"");
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
