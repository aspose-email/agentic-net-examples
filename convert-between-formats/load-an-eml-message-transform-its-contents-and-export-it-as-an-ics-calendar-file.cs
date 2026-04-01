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
            string inputPath = "input.eml";
            string outputPath = "output.ics";

            // Ensure the input EML file exists; create a minimal placeholder if it does not.
            if (!File.Exists(inputPath))
            {
                try
                {
                    string placeholderContent = "From: sender@example.com\r\nTo: recipient@example.com\r\nSubject: Sample\r\n\r\nThis is a sample email.";
                    File.WriteAllText(inputPath, placeholderContent);
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to create placeholder EML file: {ex.Message}");
                    return;
                }
            }

            // Load the EML message.
            MailMessage mailMessage;
            try
            {
                mailMessage = MailMessage.Load(inputPath, new EmlLoadOptions());
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Failed to load EML message: {ex.Message}");
                return;
            }

            using (mailMessage)
            {
                // Build an appointment using data from the email.
                MailAddress organizer = new MailAddress("organizer@example.com");
                MailAddressCollection attendees = new MailAddressCollection();
                attendees.Add(new MailAddress("attendee1@example.com"));
                attendees.Add(new MailAddress("attendee2@example.com"));

                DateTime start = DateTime.Now.AddDays(1).AddHours(9);
                DateTime end = start.AddHours(1);

                Appointment appointment = new Appointment(
                    mailMessage.Subject ?? "No Subject",
                    start,
                    end,
                    organizer,
                    attendees);
                appointment.Description = mailMessage.Body ?? string.Empty;

                // Save the appointment as an iCalendar (ICS) file.
                try
                {
                    string outputDirectory = Path.GetDirectoryName(outputPath);
                    if (!string.IsNullOrEmpty(outputDirectory) && !Directory.Exists(outputDirectory))
                    {
                        Directory.CreateDirectory(outputDirectory);
                    }

                    appointment.Save(outputPath, AppointmentSaveFormat.Ics);
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to save ICS file: {ex.Message}");
                    return;
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
