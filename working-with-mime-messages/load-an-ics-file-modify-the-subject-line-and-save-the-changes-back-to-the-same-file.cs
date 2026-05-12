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
            string icsPath = "sample.ics";

            // Ensure the .ics file exists; create a minimal placeholder if it does not.
            if (!File.Exists(icsPath))
            {
                try
                {
                    using (StreamWriter writer = new StreamWriter(icsPath))
                    {
                        writer.WriteLine("BEGIN:VCALENDAR");
                        writer.WriteLine("VERSION:2.0");
                        writer.WriteLine("END:VCALENDAR");
                    }
                    Console.WriteLine($"Placeholder .ics file created at {icsPath}");
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to create placeholder .ics file: {ex.Message}");
                    return;
                }
            }

            // Load the appointment from the .ics file, modify its subject, and save it back.
            try
            {
                Appointment appointment = Appointment.Load(icsPath);
                if (appointment == null)
                {
                    Console.Error.WriteLine("Failed to load appointment from the .ics file.");
                    return;
                }

                // Modify the subject line (Summary property).
                appointment.Summary = "Updated Subject Line";

                // Save changes back to the same file.
                appointment.Save(icsPath);
                Console.WriteLine("Appointment updated and saved.");
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Error processing .ics file: {ex.Message}");
                return;
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
