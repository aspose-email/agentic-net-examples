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

            // Ensure the input file exists; create a minimal placeholder if it does not.
            if (!File.Exists(inputPath))
            {
                try
                {
                    File.WriteAllText(inputPath, "BEGIN:VCALENDAR\r\nVERSION:2.0\r\nEND:VCALENDAR");
                }
                catch (Exception ioEx)
                {
                    Console.Error.WriteLine($"Failed to create placeholder input file: {ioEx.Message}");
                    return;
                }
            }

            // Load the iCalendar file.
            Appointment appointment;
            try
            {
                appointment = Appointment.Load(inputPath);
            }
            catch (Exception loadEx)
            {
                Console.Error.WriteLine($"Failed to load iCalendar file: {loadEx.Message}");
                return;
            }

            // Prepare save options with a custom product identifier.
            AppointmentIcsSaveOptions saveOptions = new AppointmentIcsSaveOptions();
            saveOptions.ProductId = "MyCustomProduct/1.0";

            // Save the updated iCalendar file.
            try
            {
                appointment.Save(outputPath, saveOptions);
                Console.WriteLine($"Updated iCalendar saved to '{outputPath}'.");
            }
            catch (Exception saveEx)
            {
                Console.Error.WriteLine($"Failed to save iCalendar file: {saveEx.Message}");
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
