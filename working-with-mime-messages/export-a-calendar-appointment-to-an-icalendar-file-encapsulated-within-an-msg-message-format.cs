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
            // Ensure output directory exists
            string outputDir = "Output";
            if (!Directory.Exists(outputDir))
            {
                Directory.CreateDirectory(outputDir);
            }

            // Define file paths
            string icsPath = Path.Combine(outputDir, "appointment.ics");
            string msgPath = Path.Combine(outputDir, "appointment.msg");

            // Prepare attendees
            MailAddressCollection attendees = new MailAddressCollection();
            attendees.Add(new MailAddress("person1@domain.com"));
            attendees.Add(new MailAddress("person2@domain.com"));
            attendees.Add(new MailAddress("person3@domain.com"));

            // Create the appointment
            Appointment appointment = new Appointment(
                "Room 112",
                new DateTime(2023, 10, 1, 13, 0, 0),
                new DateTime(2023, 10, 1, 14, 0, 0),
                new MailAddress("organizer@domain.com"),
                attendees);
            appointment.Summary = "Project Meeting";
            appointment.Description = "Discuss project milestones.";

            // Export as iCalendar file
            appointment.Save(icsPath); // default iCalendar format

            // Export as MSG with the iCalendar embedded
            appointment.Save(msgPath, AppointmentSaveFormat.Msg);

            Console.WriteLine("Appointment exported successfully.");
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
