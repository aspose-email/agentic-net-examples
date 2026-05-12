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
            // Output file path for the appointment
            string outputPath = "appointment.ics";

            // Ensure the directory for the output file exists
            string directory = Path.GetDirectoryName(Path.GetFullPath(outputPath));
            if (!Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }

            // Create a collection of required attendees
            MailAddressCollection attendees = new MailAddressCollection();
            attendees.Add(new MailAddress("required.attendee@example.com"));

            // Create the appointment with required attendee
            Appointment appointment = new Appointment(
                location: "Conference Room",
                startDate: new DateTime(2024, 12, 1, 10, 0, 0),
                endDate: new DateTime(2024, 12, 1, 11, 0, 0),
                organizer: new MailAddress("organizer@example.com"),
                attendees: attendees);

            // Set additional appointment details
            appointment.Summary = "Project Kickoff";
            appointment.Description = "Discuss project goals and timeline.";

            // Save the appointment to an iCalendar file
            appointment.Save(outputPath);
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine(ex.Message);
        }
    }
}
