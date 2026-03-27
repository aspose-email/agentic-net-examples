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
            string emlPath = "input.eml";
            string icsPath = "output.ics";

            // Verify input file exists
            if (!File.Exists(emlPath))
            {
                Console.Error.WriteLine($"Error: File not found – {emlPath}");
                return;
            }

            // Load the EML message with default options
            using (MailMessage message = MailMessage.Load(emlPath, new EmlLoadOptions()))
            {
                // Prepare organizer and attendees
                MailAddress organizer = message.From ?? new MailAddress("organizer@example.com");
                MailAddressCollection attendees = new MailAddressCollection();
                if (message.To != null)
                {
                    foreach (MailAddress addr in message.To)
                    {
                        attendees.Add(addr);
                    }
                }

                // Create a simple appointment (using now + 1 hour as fallback dates)
                DateTime start = DateTime.Now;
                DateTime end = start.AddHours(1);
                Appointment appointment = new Appointment(
                    location: "Meeting Room",
                    startDate: start,
                    endDate: end,
                    organizer: organizer,
                    attendees: attendees);

                // Transfer basic information from the email
                appointment.Summary = message.Subject ?? "No Subject";
                appointment.Description = message.Body ?? string.Empty;

                // Save the appointment as an iCalendar file
                try
                {
                    appointment.Save(icsPath);
                    Console.WriteLine($"ICS file created at: {icsPath}");
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Error saving ICS file: {ex.Message}");
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
