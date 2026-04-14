using Aspose.Email.Clients;
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
            // Prepare attendees
            MailAddressCollection attendees = new MailAddressCollection();
            attendees.Add(new MailAddress("person1@domain.com"));
            attendees.Add(new MailAddress("person2@domain.com"));
            attendees.Add(new MailAddress("person3@domain.com"));

            // Create the appointment
            Appointment appointment = new Appointment(
                "Conference Room",
                new DateTime(2024, 5, 20, 9, 0, 0),
                new DateTime(2024, 5, 20, 10, 0, 0),
                new MailAddress("organizer@domain.com"),
                attendees);
            appointment.Summary = "International Project Sync";
            appointment.Description = "Discuss project milestones with global team.";

            // Set a custom time zone name (optional)
            appointment.SetTimeZone("UTC");

            // Convert to a MailMessage to set a custom UTC offset
            MailMessage message = appointment.ToMailMessage();

            // Define a custom UTC offset (e.g., UTC+05:30)
            TimeSpan customOffset = new TimeSpan(5, 30, 0);
            message.TimeZoneOffset = customOffset;

            // Define output path
            string outputPath = Path.Combine(Directory.GetCurrentDirectory(), "appointment.eml");

            // Ensure the directory exists
            string outputDir = Path.GetDirectoryName(outputPath);
            if (!Directory.Exists(outputDir))
            {
                Directory.CreateDirectory(outputDir);
            }

            // Save the message to file
            try
            {
                using (FileStream fileStream = new FileStream(outputPath, FileMode.Create, FileAccess.Write))
                {
                    message.Save(fileStream);
                }
                Console.WriteLine("Appointment saved to: " + outputPath);
            }
            catch (Exception ioEx)
            {
                Console.Error.WriteLine("Failed to save appointment: " + ioEx.Message);
                return;
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine("Error: " + ex.Message);
            return;
        }
    }
}
