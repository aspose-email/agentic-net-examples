using System;
using System.IO;
using Aspose.Email;
using Aspose.Email.Calendar;
using Aspose.Email.Mime;

class Program
{
    static void Main(string[] args)
    {
        try
        {
            // Input HTML file path
            string htmlFilePath = "input.html";
            // Output iCalendar file path
            string icsFilePath = "output.ics";

            // Verify input file exists
            if (!File.Exists(htmlFilePath))
            {
                Console.Error.WriteLine($"Input file not found: {htmlFilePath}");
                return;
            }

            // Ensure output directory exists
            string outputDirectory = Path.GetDirectoryName(icsFilePath);
            if (!string.IsNullOrEmpty(outputDirectory) && !Directory.Exists(outputDirectory))
            {
                try
                {
                    Directory.CreateDirectory(outputDirectory);
                }
                catch (Exception dirEx)
                {
                    Console.Error.WriteLine($"Failed to create output directory: {dirEx.Message}");
                    return;
                }
            }

            // Read HTML content
            string htmlContent;
            try
            {
                htmlContent = File.ReadAllText(htmlFilePath);
            }
            catch (Exception readEx)
            {
                Console.Error.WriteLine($"Failed to read HTML file: {readEx.Message}");
                return;
            }

            // Prepare minimal appointment data
            string location = "Online";
            DateTime start = DateTime.Now.AddHours(1);
            DateTime end = start.AddHours(2);
            MailAddress organizer = new MailAddress("organizer@example.com");
            MailAddressCollection attendees = new MailAddressCollection();

            // Create appointment
            Appointment appointment = new Appointment(location, start, end, organizer, attendees);
            appointment.Summary = "Generated Appointment";
            appointment.Description = "Appointment generated from HTML content.";
            appointment.HtmlDescription = htmlContent;

            // Save as iCalendar (.ics)
            try
            {
                appointment.Save(icsFilePath, AppointmentSaveFormat.Ics);
                Console.WriteLine($"iCalendar file created at: {icsFilePath}");
            }
            catch (Exception saveEx)
            {
                Console.Error.WriteLine($"Failed to save iCalendar file: {saveEx.Message}");
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}