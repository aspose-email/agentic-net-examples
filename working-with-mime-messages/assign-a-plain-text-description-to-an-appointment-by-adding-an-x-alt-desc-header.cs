using System;
using System.IO;
using Aspose.Email;
using Aspose.Email.Calendar;
using Aspose.Email.Mime;

class Program
{
    static void Main()
    {
        try
        {
            // Output file paths
            string icsPath = "appointment.ics";
            string emlPath = "appointment.eml";

            // Ensure output directories exist
            string icsDir = Path.GetDirectoryName(icsPath);
            if (!string.IsNullOrEmpty(icsDir) && !Directory.Exists(icsDir))
            {
                Directory.CreateDirectory(icsDir);
            }

            string emlDir = Path.GetDirectoryName(emlPath);
            if (!string.IsNullOrEmpty(emlDir) && !Directory.Exists(emlDir))
            {
                Directory.CreateDirectory(emlDir);
            }

            // Prepare attendees
            MailAddressCollection attendees = new MailAddressCollection();
            attendees.Add(new MailAddress("person1@example.com"));
            attendees.Add(new MailAddress("person2@example.com"));

            // Create the appointment
            Appointment appointment = new Appointment(
                "Conference Room",
                new DateTime(2023, 12, 1, 10, 0, 0),
                new DateTime(2023, 12, 1, 11, 0, 0),
                new MailAddress("organizer@example.com"),
                attendees);
            appointment.Summary = "Project Kickoff";
            appointment.Description = "Discuss project goals and timeline.";

            // Save the appointment as iCalendar (ICS)
            appointment.Save(icsPath);

            // Convert to MailMessage to add the X-ALT-DESC header
            using (MailMessage message = appointment.ToMailMessage())
            {
                // Add plain‑text description as a custom header
                message.Headers.Add("X-ALT-DESC", appointment.Description);

                // Save the message with the custom header as EML
                using (FileStream emlStream = new FileStream(emlPath, FileMode.Create, FileAccess.Write))
                {
                    message.Save(emlStream, SaveOptions.DefaultEml);
                }
            }

            Console.WriteLine("Appointment created, description header added, files saved.");
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine(ex.Message);
        }
    }
}
