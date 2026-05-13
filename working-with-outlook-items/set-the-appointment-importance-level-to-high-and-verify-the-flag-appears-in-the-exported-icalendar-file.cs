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
            // Define output iCalendar file path
            string outputPath = "appointment_high_importance.ics";

            // Ensure the output directory exists
            string outputDir = Path.GetDirectoryName(Path.GetFullPath(outputPath));
            if (!Directory.Exists(outputDir))
            {
                Directory.CreateDirectory(outputDir);
            }

            // Create an appointment
            MailAddress organizer = new MailAddress("organizer@example.com");
            MailAddressCollection attendees = new MailAddressCollection();
            attendees.Add(new MailAddress("attendee1@example.com"));
            attendees.Add(new MailAddress("attendee2@example.com"));

            Appointment appointment = new Appointment(
                "Conference Room",
                new DateTime(2024, 12, 15, 10, 0, 0),
                new DateTime(2024, 12, 15, 11, 0, 0),
                organizer,
                attendees);

            // Set importance to High
            appointment.MicrosoftImportance = MSImportance.High;

            // Save the appointment to iCalendar format
            try
            {
                appointment.Save(outputPath);
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Error saving appointment: {ex.Message}");
                return;
            }

            // Verify that the importance flag appears in the exported file
            if (!File.Exists(outputPath))
            {
                Console.Error.WriteLine("The iCalendar file was not created.");
                return;
            }

            try
            {
                string icsContent = File.ReadAllText(outputPath);
                if (icsContent.IndexOf("HIGH", StringComparison.OrdinalIgnoreCase) >= 0)
                {
                    Console.WriteLine("Importance flag set to High is present in the iCalendar file.");
                }
                else
                {
                    Console.WriteLine("Importance flag not found in the iCalendar file.");
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Error reading iCalendar file: {ex.Message}");
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
