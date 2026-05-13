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
            string directory = Path.GetDirectoryName(outputPath);
            if (!string.IsNullOrEmpty(directory) && !Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }

            // Prepare attendees
            MailAddressCollection attendees = new MailAddressCollection();
            attendees.Add(new MailAddress("person1@example.com"));
            attendees.Add(new MailAddress("person2@example.com"));
            attendees.Add(new MailAddress("person3@example.com"));

            // Create the appointment
            Appointment appointment = new Appointment(
                location: "Conference Room 1",
                startDate: new DateTime(2023, 12, 15, 10, 0, 0),
                endDate: new DateTime(2023, 12, 15, 11, 0, 0),
                organizer: new MailAddress("organizer@example.com"),
                attendees: attendees);

            appointment.Summary = "Project Kickoff Meeting";
            appointment.Description = "Discuss project goals, timeline, and responsibilities.";

            // Save the appointment to an .ics file using default calendar settings
            appointment.Save(outputPath, AppointmentSaveFormat.Ics);
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
