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
            string icsPath = "sample.ics";

            // Ensure the file exists; create a minimal placeholder if missing
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
                    Console.WriteLine($"Placeholder ICS file created at '{icsPath}'.");
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to create placeholder file: {ex.Message}");
                    return;
                }
            }

            // Load the appointment from the .ics file
            Appointment appointment;
            try
            {
                appointment = Appointment.Load(icsPath);
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Failed to load ICS file: {ex.Message}");
                return;
            }

            // Configure save options to remove the product identifier
            AppointmentIcsSaveOptions saveOptions = new AppointmentIcsSaveOptions();
            saveOptions.ProductId = null; // clear product identifier

            // Save the modified appointment back to the same file
            try
            {
                appointment.Save(icsPath, saveOptions);
                Console.WriteLine("Product identifier removed and file saved.");
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Failed to save modified ICS file: {ex.Message}");
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
