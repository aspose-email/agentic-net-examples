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
            // Define output MSG file path
            string outputPath = Path.Combine(Environment.CurrentDirectory, "appointment.msg");
            string outputDir = Path.GetDirectoryName(outputPath);
            if (!Directory.Exists(outputDir))
            {
                Directory.CreateDirectory(outputDir);
            }

            // Create appointment
            MailAddress organizer = new MailAddress("organizer@example.com");
            MailAddressCollection attendees = new MailAddressCollection
            {
                new MailAddress("attendee1@example.com"),
                new MailAddress("attendee2@example.com")
            };

            Appointment appointment = new Appointment(
                "Conference Room",
                new DateTime(2024, 5, 20, 10, 0, 0),
                new DateTime(2024, 5, 20, 11, 0, 0),
                organizer,
                attendees)
            {
                Summary = "Project Kickoff",
                Description = "Discuss project goals and timelines."
            };

            // Convert appointment to MailMessage
            MailMessage mailMessage = appointment.ToMailMessage();

            // Preserve properties (example: subject and body)
            mailMessage.Subject = appointment.Summary;
            mailMessage.Body = appointment.Description;

            // Convert MailMessage to MapiMessage and save as MSG
            using (MapiMessage mapiMessage = MapiMessage.FromMailMessage(mailMessage))
            {
                mapiMessage.Save(outputPath);
            }

            Console.WriteLine($"Appointment saved as MSG to: {outputPath}");
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
