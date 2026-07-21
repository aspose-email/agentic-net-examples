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
            // Input EML file path
            string inputPath = "input.eml";
            // Output iCalendar file path
            string outputPath = "output.ics";

            // Verify that the input file exists
            if (!File.Exists(inputPath))
            {
                try
                {
                    using (MailMessage placeholder = new MailMessage(
                        "sender@example.com",
                        "recipient@example.com",
                        "Placeholder Subject",
                        "Placeholder body."))
                    {
                        placeholder.Save(inputPath, SaveOptions.DefaultEml);
                    }
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Error creating placeholder message: {ex.Message}");
                    return;
                }

                Console.Error.WriteLine($"Input file '{inputPath}' does not exist.");
                return;
            }

            // Load the EML message
            using (MailMessage email = MailMessage.Load(inputPath))
            {
                // Transform the email into a calendar appointment
                DateTime start = email.Date;
                DateTime end = start.AddHours(1); // default duration 1 hour
                MailAddress organizer = email.From;
                MailAddressCollection attendees = new MailAddressCollection(); // no attendees in this example

                Appointment appointment = new Appointment(
                    location: "Location not specified",
                    startDate: start,
                    endDate: end,
                    organizer: organizer,
                    attendees: attendees);

                appointment.Summary = email.Subject ?? "No Subject";
                appointment.Description = email.Body ?? string.Empty;

                // Export the appointment as an iCalendar (ICS) file
                appointment.Save(outputPath);
                Console.WriteLine($"Appointment saved to '{outputPath}'.");
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
