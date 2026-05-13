using System;
using System.IO;
using System.Linq;
using Aspose.Email;
using Aspose.Email.Calendar;

class Program
{
    static void Main()
    {
        try
        {
            string icsPath = "appointment.ics";

            // Ensure the input file exists; create a minimal placeholder if missing
            if (!File.Exists(icsPath))
            {
                try
                {
                    Directory.CreateDirectory(Path.GetDirectoryName(icsPath) ?? ".");
                    using (var writer = new StreamWriter(icsPath, false))
                    {
                        writer.WriteLine("BEGIN:VCALENDAR");
                        writer.WriteLine("VERSION:2.0");
                        writer.WriteLine("PRODID:-//Aspose.Email//EN");
                        writer.WriteLine("END:VCALENDAR");
                    }
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to create placeholder iCalendar file: {ex.Message}");
                    return;
                }
            }

            // Load the appointment from the iCalendar file
            Appointment appointment;
            try
            {
                appointment = Appointment.Load(icsPath);
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Failed to load appointment: {ex.Message}");
                return;
            }

            // Ensure a plain‑text description exists
            if (string.IsNullOrEmpty(appointment.Description))
            {
                appointment.Description = "No description provided.";
            }

            // Save appointment to a string (ICS format)
            string icsContent;
            try
            {
                using (var ms = new MemoryStream())
                {
                    appointment.Save(ms, AppointmentSaveFormat.Ics);
                    ms.Position = 0;
                    using (var reader = new StreamReader(ms))
                    {
                        icsContent = reader.ReadToEnd();
                    }
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Failed to serialize appointment: {ex.Message}");
                return;
            }

            // Insert X-ALT-DESC header if missing
            var lines = icsContent.Split(new[] { "\r\n", "\n" }, StringSplitOptions.None).ToList();

            bool hasAltDesc = lines.Any(l => l.StartsWith("X-ALT-DESC:", StringComparison.OrdinalIgnoreCase));
            if (!hasAltDesc)
            {
                int descIndex = lines.FindIndex(l => l.StartsWith("DESCRIPTION:", StringComparison.OrdinalIgnoreCase));
                if (descIndex != -1)
                {
                    string altDesc = $"X-ALT-DESC:{appointment.Description}";
                    lines.Insert(descIndex + 1, altDesc);
                }
                else
                {
                    // If no DESCRIPTION line, add at the end of the component
                    lines.Add($"X-ALT-DESC:{appointment.Description}");
                }
            }

            string outputPath = "appointment_with_alt.ics";
            try
            {
                string outputDir = Path.GetDirectoryName(outputPath);
                if (!string.IsNullOrEmpty(outputDir))
                {
                    Directory.CreateDirectory(outputDir);
                }

                using (var writer = new StreamWriter(outputPath, false))
                {
                    foreach (var line in lines)
                    {
                        writer.WriteLine(line);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Failed to write output iCalendar: {ex.Message}");
                return;
            }

            Console.WriteLine("Appointment processed and X-ALT-DESC header ensured.");
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
