using System;
using System.IO;
using Aspose.Email;
using Aspose.Email.Calendar;

public class Program
{
    public static void Main()
    {
        // Prepare attendees
        MailAddressCollection attendees = new MailAddressCollection();
        attendees.Add(new MailAddress("person1@domain.com"));
        attendees.Add(new MailAddress("person2@domain.com"));
        attendees.Add(new MailAddress("person3@domain.com"));

        // Create an appointment with required fields
        Appointment appointment = new Appointment(
            "Conference Room",
            new DateTime(2023, 12, 15, 10, 0, 0),
            new DateTime(2023, 12, 15, 11, 0, 0),
            new MailAddress("organizer@domain.com"),
            attendees);
        appointment.Summary = "Project Kickoff";
        appointment.Description = "Discuss project goals and timeline.";

        // Output file path
        string outputPath = "appointment.msg";

        // Guard file I/O
        try
        {
            string directory = Path.GetDirectoryName(outputPath);
            if (!string.IsNullOrEmpty(directory) && !Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }

            // Save the appointment as MSG
            appointment.Save(outputPath, AppointmentSaveFormat.Msg);
            Console.WriteLine($"Appointment saved to {outputPath}");
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Failed to save appointment: {ex.Message}");
        }
    }
}
