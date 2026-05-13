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
            attendees.Add(new MailAddress("alice@example.com"));
            attendees.Add(new MailAddress("bob@example.com"));

            // Organizer address
            MailAddress organizer = new MailAddress("organizer@example.com");

            // Create appointment using constructor that sets subject (summary) and times
            Appointment appointment = new Appointment(
                location: "Conference Room",
                summary: "Team Meeting",
                description: "Discuss project milestones",
                startDate: new DateTime(2023, 10, 1, 9, 0, 0),
                endDate: new DateTime(2023, 10, 1, 10, 0, 0),
                organizer: organizer,
                attendees: attendees
            );

            // Save the appointment to an iCalendar file
            string outputPath = "appointment.ics";
            string outputDir = Path.GetDirectoryName(outputPath);
            if (!string.IsNullOrEmpty(outputDir) && !Directory.Exists(outputDir))
            {
                Directory.CreateDirectory(outputDir);
            }

            try
            {
                using (FileStream fs = new FileStream(outputPath, FileMode.Create, FileAccess.Write))
                {
                    appointment.Save(fs);
                }
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
