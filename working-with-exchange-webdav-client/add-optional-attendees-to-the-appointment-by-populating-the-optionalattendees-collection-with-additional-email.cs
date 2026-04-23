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
            // Define appointment times
            DateTime start = new DateTime(2023, 12, 1, 10, 0, 0);
            DateTime end = new DateTime(2023, 12, 1, 11, 0, 0);

            // Organizer and required attendees
            MailAddress organizer = new MailAddress("organizer@example.com");
            MailAddressCollection requiredAttendees = new MailAddressCollection();
            requiredAttendees.Add(new MailAddress("required1@example.com"));

            // Create the appointment
            Appointment appointment = new Appointment(
                "Meeting Room",
                start,
                end,
                organizer,
                requiredAttendees);
            appointment.Summary = "Project Update";
            appointment.Description = "Discuss project status and next steps.";

            // Add optional attendees
            MailAddressCollection optionalAttendees = appointment.OptionalAttendees;
            optionalAttendees.Add(new MailAddress("optional1@example.com"));
            optionalAttendees.Add(new MailAddress("optional2@example.com"));

            // Prepare output path
            string outputPath = "appointment.ics";
            string directory = Path.GetDirectoryName(outputPath);
            if (!string.IsNullOrEmpty(directory) && !Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }

            // Save the appointment
            try
            {
                appointment.Save(outputPath);
                Console.WriteLine($"Appointment saved to {outputPath}");
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Failed to save appointment: {ex.Message}");
                return;
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
