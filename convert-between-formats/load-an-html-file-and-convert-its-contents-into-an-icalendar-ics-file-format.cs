using System;
using System.IO;
using Aspose.Email;
using Aspose.Email.Calendar;

class Program
{
    static void Main(string[] args)
    {
        try
        {
            string htmlFilePath = "input.html";
            string icsFilePath = "output.ics";

            // Ensure the HTML input file exists; create a minimal placeholder if missing.
            if (!File.Exists(htmlFilePath))
            {
                try
                {
                    File.WriteAllText(htmlFilePath, "<html><body>Sample Event</body></html>");
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to create placeholder HTML file: {ex.Message}");
                    return;
                }
            }

            // Read HTML content.
            string htmlContent;
            try
            {
                using (StreamReader reader = new StreamReader(htmlFilePath))
                {
                    htmlContent = reader.ReadToEnd();
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Error reading HTML file: {ex.Message}");
                return;
            }

            // Prepare minimal appointment data.
            DateTime startDate = DateTime.Now;
            DateTime endDate = startDate.AddHours(1);
            MailAddress organizer = new MailAddress("organizer@example.com");
            MailAddressCollection attendees = new MailAddressCollection();

            // Create the appointment and set HTML description.
            Appointment appointment = new Appointment("Sample Event", startDate, endDate, organizer, attendees);
            appointment.HtmlDescription = htmlContent;
            appointment.Summary = "Sample Event from HTML";

            // Save the appointment as an iCalendar (ICS) file.
            try
            {
                using (FileStream icsStream = new FileStream(icsFilePath, FileMode.Create, FileAccess.Write))
                {
                    appointment.Save(icsStream, AppointmentSaveFormat.Ics);
                }
                Console.WriteLine($"ICS file created at: {icsFilePath}");
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
