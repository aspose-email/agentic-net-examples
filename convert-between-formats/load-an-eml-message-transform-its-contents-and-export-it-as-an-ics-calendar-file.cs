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

            // Ensure input file exists; create a minimal placeholder if missing
            if (!File.Exists(emlPath))
            {
                try
                {
                    using (MailMessage placeholder = new MailMessage("from@example.com", "to@example.com"))
                    {
                        placeholder.Subject = "Placeholder";
                        placeholder.Body = "This is a placeholder EML file.";
                        placeholder.Save(emlPath);
                    }
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to create placeholder EML: {ex.Message}");
                    return;
                }
            }

            // Load the EML message
            MailMessage message;
            try
            {
                message = MailMessage.Load(emlPath);
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Failed to load EML file: {ex.Message}");
                return;
            }

            using (message)
            {
                // Prepare appointment data
                string subject = string.IsNullOrEmpty(message.Subject) ? "No Subject" : message.Subject;
                DateTime start = DateTime.Now;
                DateTime end = start.AddHours(1);
                MailAddress organizer = message.From ?? new MailAddress("organizer@example.com");

                // Collect attendees from To and CC fields
                MailAddressCollection attendees = new MailAddressCollection();
                if (message.To != null)
                {
                    foreach (MailAddress addr in message.To)
                    {
                        attendees.Add(addr);
                    }
                }
                if (message.CC != null)
                {
                    foreach (MailAddress addr in message.CC)
                    {
                        attendees.Add(addr);
                    }
                }

                // Create the appointment
                Appointment appointment = new Appointment(subject, start, end, organizer, attendees);
                appointment.Description = message.Body ?? string.Empty;

                // Ensure output directory exists
                try
                {
                    string outputDir = Path.GetDirectoryName(Path.GetFullPath(icsPath));
                    if (!string.IsNullOrEmpty(outputDir) && !Directory.Exists(outputDir))
                    {
                        Directory.CreateDirectory(outputDir);
                    }
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to ensure output directory: {ex.Message}");
                    return;
                }

                // Save as iCalendar (ICS) file
                try
                {
                    appointment.Save(icsPath);
                    Console.WriteLine($"ICS file saved to: {icsPath}");
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to save ICS file: {ex.Message}");
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
