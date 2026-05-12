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
            // Prepare output directory
            string outputDir = "Output";
            if (!Directory.Exists(outputDir))
                Directory.CreateDirectory(outputDir);

            // Create appointment details
            MailAddress organizer = new MailAddress("organizer@example.com");
            MailAddressCollection attendees = new MailAddressCollection
            {
                new MailAddress("attendee1@example.com"),
                new MailAddress("attendee2@example.com")
            };

            Appointment appointment = new Appointment(
                "Project Kickoff",
                new DateTime(2023, 12, 1, 10, 0, 0),
                new DateTime(2023, 12, 1, 11, 0, 0),
                organizer,
                attendees);

            appointment.Summary = "Project Kickoff Meeting";
            appointment.Description = "Discuss project scope and timeline.";

            // Convert appointment to a draft MIME message
            using (MailMessage draftMessage = appointment.ToMailMessage())
            {
                string draftPath = Path.Combine(outputDir, "DraftAppointment.eml");
                draftMessage.Save(draftPath, SaveOptions.DefaultEml);
                Console.WriteLine($"Draft appointment saved to: {draftPath}");
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
