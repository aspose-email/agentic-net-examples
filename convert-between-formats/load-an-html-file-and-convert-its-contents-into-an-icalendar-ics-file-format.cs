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
            // Paths for input HTML and output iCalendar file
            string htmlPath = "input.html";
            string icsPath = "output.ics";

            // Verify the HTML file exists
            if (!File.Exists(htmlPath))
            {
                Console.Error.WriteLine($"HTML file not found: {htmlPath}");
                return;
            }

            // Read HTML content
            string htmlContent;
            try
            {
                htmlContent = File.ReadAllText(htmlPath);
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Error reading HTML file: {ex.Message}");
                return;
            }

            // Ensure the output directory exists
            string outputDirectory = Path.GetDirectoryName(icsPath);
            if (!string.IsNullOrEmpty(outputDirectory) && !Directory.Exists(outputDirectory))
            {
                try
                {
                    Directory.CreateDirectory(outputDirectory);
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Error creating output directory: {ex.Message}");
                    return;
                }
            }

            // Prepare attendees
            MailAddressCollection attendees = new MailAddressCollection();
            attendees.Add(new MailAddress("attendee1@example.com"));
            attendees.Add(new MailAddress("attendee2@example.com"));

            // Create an appointment
            Appointment appointment = new Appointment(
                "Conference Room",
                new DateTime(2023, 12, 25, 10, 0, 0),
                new DateTime(2023, 12, 25, 11, 0, 0),
                new MailAddress("organizer@example.com"),
                attendees);

            appointment.Summary = "Team Meeting";
            appointment.Description = "Discussion of project status.";
            appointment.HtmlDescription = htmlContent;

            // Save the appointment as an iCalendar (ICS) file
            try
            {
                appointment.Save(icsPath, AppointmentSaveFormat.Ics);
                Console.WriteLine($"iCalendar file saved to {icsPath}");
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Error saving iCalendar file: {ex.Message}");
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
