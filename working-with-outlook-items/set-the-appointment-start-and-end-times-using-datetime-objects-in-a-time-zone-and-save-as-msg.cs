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
            // Output MSG file path
            string outputPath = "appointment.msg";

            // Ensure the output directory exists
            string directory = Path.GetDirectoryName(outputPath);
            if (!string.IsNullOrEmpty(directory) && !Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }

            // Prepare attendees
            MailAddressCollection attendees = new MailAddressCollection();
            attendees.Add(new MailAddress("person1@domain.com"));
            attendees.Add(new MailAddress("person2@domain.com"));

            // Define start and end times (local to the specified time zone)
            DateTime start = new DateTime(2023, 10, 1, 9, 0, 0, DateTimeKind.Unspecified);
            DateTime end = new DateTime(2023, 10, 1, 10, 0, 0, DateTimeKind.Unspecified);

            // Create the appointment
            Appointment appointment = new Appointment(
                "Conference Room",
                start,
                end,
                new MailAddress("organizer@domain.com"),
                attendees);
            appointment.Summary = "Team Meeting";
            appointment.Description = "Discuss project status.";

            // Set the time zone for the appointment
            appointment.SetTimeZone("America/New_York");

            // Convert to MAPI message and save as MSG
            using (MapiMessage mapiMessage = appointment.ToMapiMessage())
            {
                mapiMessage.Save(outputPath);
            }

            Console.WriteLine("Appointment saved to " + outputPath);
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine("Error: " + ex.Message);
        }
    }
}
