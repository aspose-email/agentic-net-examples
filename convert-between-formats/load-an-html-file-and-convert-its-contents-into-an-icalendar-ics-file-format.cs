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
            // Input HTML file path
            string inputHtmlPath = "input.html";

            // Ensure the HTML file exists; create a minimal placeholder if missing
            if (!File.Exists(inputHtmlPath))
            {
                File.WriteAllText(inputHtmlPath, "<html><body>Sample Event</body></html>");
            }

            // Read HTML content
            string htmlContent = File.ReadAllText(inputHtmlPath);

            // Prepare appointment details
            MailAddress organizer = new MailAddress("organizer@example.com");
            MailAddressCollection attendees = new MailAddressCollection(); // no attendees for this example
            DateTime startTime = DateTime.Now.AddHours(1);
            DateTime endTime = startTime.AddHours(2);

            // Create the appointment (location, start, end, organizer, attendees)
            Appointment appointment = new Appointment("Location", startTime, endTime, organizer, attendees);
            appointment.Summary = "Event generated from HTML";
            appointment.Description = htmlContent; // embed HTML as description

            // Output iCalendar (ICS) file path
            string outputIcsPath = "output.ics";

            // Ensure the output directory exists
            string outputDir = Path.GetDirectoryName(outputIcsPath);
            if (!string.IsNullOrEmpty(outputDir) && !Directory.Exists(outputDir))
            {
                Directory.CreateDirectory(outputDir);
            }

            // Save the appointment as an iCalendar file
            appointment.Save(outputIcsPath);

            Console.WriteLine("iCalendar file created at: " + Path.GetFullPath(outputIcsPath));
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine("Error: " + ex.Message);
        }
    }
}
