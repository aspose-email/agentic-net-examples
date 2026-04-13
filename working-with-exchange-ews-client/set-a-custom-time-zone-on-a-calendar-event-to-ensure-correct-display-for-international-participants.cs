using System;
using System.IO;
using Aspose.Email;
using Aspose.Email.Calendar;
using Aspose.Email.Mapi;

class Program
{
    static void Main()
    {
        try
        {
            // Define output file path
            string outputPath = Path.Combine(Directory.GetCurrentDirectory(), "event.ics");
            string outputDir = Path.GetDirectoryName(outputPath);
            if (!Directory.Exists(outputDir))
            {
                Directory.CreateDirectory(outputDir);
            }

            // Prepare attendees
            MailAddressCollection attendees = new MailAddressCollection();
            attendees.Add(new MailAddress("alice@example.com"));
            attendees.Add(new MailAddress("bob@example.com"));

            // Create the appointment
            Appointment appointment = new Appointment(
                "Conference Room",
                new DateTime(2024, 10, 15, 9, 0, 0),
                new DateTime(2024, 10, 15, 10, 0, 0),
                new MailAddress("organizer@example.com"),
                attendees);

            appointment.Summary = "Project Kickoff";
            appointment.Description = "Discuss project goals and timelines.";
            appointment.Location = "Headquarters";

            // Set a custom time zone for the event
            string timeZoneId = "Europe/London";
            appointment.SetTimeZone(timeZoneId);
            appointment.StartTimeZone = timeZoneId;
            appointment.EndTimeZone = timeZoneId;

            // Save the appointment to an iCalendar file
            try
            {
                appointment.Save(outputPath);
                Console.WriteLine($"Appointment saved to '{outputPath}'.");
            }
            catch (Exception ioEx)
            {
                Console.Error.WriteLine($"Failed to save appointment: {ioEx.Message}");
                return;
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
