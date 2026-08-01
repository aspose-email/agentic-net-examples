using System;
using System.IO;
using Aspose.Email;
using Aspose.Email.Mime;
using Aspose.Email.Calendar;

class Program
{
    static void Main()
    {
        try
        {
            // Prepare attendees collection
            MailAddressCollection attendees = new MailAddressCollection
            {
                new MailAddress("person1@domain.com"),
                new MailAddress("person2@domain.com"),
                new MailAddress("person3@domain.com")
            };

            // Create an appointment (location, start, end, organizer, attendees)
            Appointment appointment = new Appointment(
                "Room 112",
                new DateTime(2024, 10, 15, 13, 0, 0),
                new DateTime(2024, 10, 15, 14, 0, 0),
                new MailAddress("organizer@domain.com"),
                attendees);

            // Set additional properties
            appointment.Summary = "Project Review Meeting";
            appointment.Description = "Discuss project milestones and next steps.";

            // Convert the appointment to a MailMessage
            using (MailMessage message = appointment.ToMailMessage())
            {
                // Define output file path
                string outputPath = "appointment.msg";

                // Ensure the directory exists (if any)
                string directory = Path.GetDirectoryName(outputPath);
                if (!string.IsNullOrEmpty(directory) && !Directory.Exists(directory))
                {
                    Directory.CreateDirectory(directory);
                }

                // Save the message as MSG
                try
                {
                    message.Save(outputPath);
                    Console.WriteLine($"Appointment saved to '{outputPath}'.");
                }
                catch (Exception ioEx)
                {
                    Console.Error.WriteLine($"Failed to save MSG file: {ioEx.Message}");
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
