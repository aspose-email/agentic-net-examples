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
            // Paths for input EML and output ICS files
            string inputPath = "sample.eml";
            string outputPath = "output.ics";

            // Verify that the input EML file exists
            if (!File.Exists(inputPath))
            {
                Console.Error.WriteLine($"Error: Input file not found – {inputPath}");
                return;
            }

            // Ensure the output directory exists
            string outputDir = Path.GetDirectoryName(outputPath);
            if (!string.IsNullOrEmpty(outputDir) && !Directory.Exists(outputDir))
            {
                Directory.CreateDirectory(outputDir);
            }

            // Load the EML message
            using (MailMessage emailMessage = MailMessage.Load(inputPath))
            {
                // Prepare data for the calendar event
                string eventSubject = emailMessage.Subject ?? "No Subject";
                string eventDescription = emailMessage.Body ?? string.Empty;

                // Example: event starts now and lasts one hour
                DateTime eventStart = DateTime.Now;
                DateTime eventEnd = eventStart.AddHours(1);

                // Create an attendee list and add the sender if available
                MailAddressCollection attendees = new MailAddressCollection();
                if (emailMessage.From != null)
                {
                    attendees.Add(emailMessage.From);
                }

                // Create the appointment (calendar event)
                Appointment calendarEvent = new Appointment(eventSubject, eventStart, eventEnd, emailMessage.From, attendees);
                calendarEvent.Description = eventDescription;

                // Save the appointment as an iCalendar (ICS) file
                calendarEvent.Save(outputPath);
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}