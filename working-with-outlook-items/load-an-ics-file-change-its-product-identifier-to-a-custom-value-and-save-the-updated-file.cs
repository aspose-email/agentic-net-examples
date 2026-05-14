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
            string inputPath = "input.ics";
            string outputPath = "output.ics";

            // Ensure the input file exists; create a minimal placeholder if missing.
            if (!File.Exists(inputPath))
            {
                try
                {
                    File.WriteAllText(inputPath, "BEGIN:VCALENDAR\r\nEND:VCALENDAR");
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to create placeholder input file: {ex.Message}");
                    return;
                }
            }

            // Load the iCalendar file.
            Appointment appointment;
            try
            {
                appointment = Appointment.Load(inputPath);
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Failed to load iCalendar file: {ex.Message}");
                return;
            }

            // Configure save options with a custom product identifier.
            AppointmentIcsSaveOptions saveOptions = new AppointmentIcsSaveOptions();
            saveOptions.ProductId = "MyCustomProduct";

            // Save the updated iCalendar file.
            try
            {
                appointment.Save(outputPath, saveOptions);
                Console.WriteLine($"Updated iCalendar saved to '{outputPath}'.");
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Failed to save iCalendar file: {ex.Message}");
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
