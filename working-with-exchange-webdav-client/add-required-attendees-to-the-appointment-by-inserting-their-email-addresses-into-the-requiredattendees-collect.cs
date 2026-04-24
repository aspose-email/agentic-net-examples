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
            // Organizer of the meeting
            MailAddress organizer = new MailAddress("organizer@example.com");

            // Create a collection for required attendees
            MailAddressCollection requiredAttendees = new MailAddressCollection();
            requiredAttendees.Add(new MailAddress("required1@example.com"));
            requiredAttendees.Add(new MailAddress("required2@example.com"));

            // Create the appointment with initial required attendees
            Appointment appointment = new Appointment(
                "Conference Room",
                new DateTime(2023, 10, 1, 10, 0, 0),
                new DateTime(2023, 10, 1, 11, 0, 0),
                organizer,
                requiredAttendees);

            // Add additional required attendees after creation
            appointment.Attendees.Add(new MailAddress("required3@example.com"));

            // Set other appointment details
            appointment.Summary = "Project Meeting";
            appointment.Description = "Discuss project milestones.";

            // Define the output file path
            string filePath = "appointment.ics";

            // Ensure the target directory exists (if any)
            string directory = Path.GetDirectoryName(filePath);
            if (!string.IsNullOrEmpty(directory) && !Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }

            // Save the appointment to an iCalendar file
            try
            {
                appointment.Save(filePath);
                Console.WriteLine($"Appointment saved to {filePath}");
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
