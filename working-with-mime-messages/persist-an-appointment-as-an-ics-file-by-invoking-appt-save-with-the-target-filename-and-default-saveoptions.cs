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
            // Prepare attendees
            MailAddressCollection attendees = new MailAddressCollection();
            attendees.Add(new MailAddress("person1@domain.com"));
            attendees.Add(new MailAddress("person2@domain.com"));

            // Create an appointment
            Appointment appt = new Appointment(
                "Conference Room",
                new DateTime(2024, 5, 20, 10, 0, 0),
                new DateTime(2024, 5, 20, 11, 0, 0),
                new MailAddress("organizer@domain.com"),
                attendees);

            appt.Summary = "Project Kickoff";
            appt.Description = "Discuss project goals and timelines.";

            // Define output file path
            string outputPath = Path.Combine(Directory.GetCurrentDirectory(), "appointment.ics");

            // Ensure the output directory exists
            string outputDir = Path.GetDirectoryName(outputPath);
            if (!string.IsNullOrEmpty(outputDir) && !Directory.Exists(outputDir))
            {
                Directory.CreateDirectory(outputDir);
            }

            // Save the appointment as an iCalendar (ICS) file using default save options
            appt.Save(outputPath);
            Console.WriteLine($"Appointment saved to: {outputPath}");
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
