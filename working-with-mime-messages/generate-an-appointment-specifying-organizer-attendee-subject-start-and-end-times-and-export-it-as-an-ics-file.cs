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
            // Define the output file path
            string outputPath = "appointment.ics";

            // Ensure the output directory exists
            string directory = Path.GetDirectoryName(Path.GetFullPath(outputPath));
            if (!Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }

            // Prepare attendees collection
            MailAddressCollection attendees = new MailAddressCollection();
            attendees.Add(new MailAddress("attendee@example.com"));

            // Create the appointment with organizer, attendees, subject, start and end times
            Appointment appointment = new Appointment(
                "Conference Room",                     // location
                "Team Meeting",                        // summary (subject)
                "Discuss project status and next steps.", // description
                new DateTime(2023, 12, 1, 10, 0, 0),   // start time
                new DateTime(2023, 12, 1, 11, 0, 0),   // end time
                new MailAddress("organizer@example.com"), // organizer
                attendees);                            // attendees

            // Optionally set additional properties
            appointment.Summary = "Team Meeting";
            appointment.Description = "Discuss project status and next steps.";

            // Save the appointment as an iCalendar (ICS) file
            appointment.Save(outputPath);

            Console.WriteLine("Appointment saved to: " + outputPath);
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine("Error: " + ex.Message);
        }
    }
}
