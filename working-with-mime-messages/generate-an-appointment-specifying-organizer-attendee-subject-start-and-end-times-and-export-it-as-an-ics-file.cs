using System;
using System.IO;
using Aspose.Email;
using Aspose.Email.Calendar;

// Author: Aspose.Email example - creates an appointment and saves it as an iCalendar (ICS) file.
class Program
{
    static void Main()
    {
        try
        {
            // Define the output file path for the .ics file.
            string outputPath = "appointment.ics";

            // Ensure the target directory exists.
            string directory = Path.GetDirectoryName(Path.GetFullPath(outputPath));
            if (!Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }

            // Prepare attendees.
            MailAddressCollection attendees = new MailAddressCollection();
            attendees.Add(new MailAddress("attendee@example.com"));

            // Create the appointment with location, summary, description, start/end times, organizer, and attendees.
            Appointment appointment = new Appointment(
                "Conference Room",                     // location
                "Team Sync",                           // summary (subject)
                "Weekly team sync meeting.",           // description
                new DateTime(2023, 12, 1, 10, 0, 0),   // start time
                new DateTime(2023, 12, 1, 11, 0, 0),   // end time
                new MailAddress("organizer@example.com"), // organizer
                attendees);                            // attendees

            // Save the appointment as an iCalendar file using default options.
            appointment.Save(outputPath);

            Console.WriteLine($"Appointment successfully saved to '{outputPath}'.");
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
