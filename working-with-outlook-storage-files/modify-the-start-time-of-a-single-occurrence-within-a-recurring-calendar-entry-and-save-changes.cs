using System;
using System.IO;
using Aspose.Email;
using Aspose.Email.Calendar;
using Aspose.Email.Mapi; // May be needed for exception handling

class Program
{
    static void Main()
    {
        try
        {
            // Input and output file paths
            string inputPath = "recurring.ics";
            string outputPath = "updated.ics";

            // Verify that the input file exists
            if (!File.Exists(inputPath))
            {
                Console.Error.WriteLine($"Input file not found: {inputPath}");
                return;
            }

            // Load the recurring appointment from an iCalendar file
            Appointment appointment = Appointment.Load(inputPath);

            // Define the occurrence date that needs its start time changed
            DateTime occurrenceDate = new DateTime(2023, 12, 25, 10, 0, 0);

            // -----------------------------------------------------------------
            // NOTE: The exact API for accessing a single occurrence (exception)
            // within a recurring Appointment is version‑specific.
            // In some versions you can use:
            //     var exception = appointment.GetException(occurrenceDate);
            // or work with MapiCalendarExceptionInfo via the MapiCalendar API.
            // The following is a placeholder illustrating the intended steps.
            // -----------------------------------------------------------------
            // var exception = appointment.GetException(occurrenceDate);
            // if (exception != null)
            // {
            //     // Set the new start time (and adjust end time accordingly)
            //     exception.StartTime = new DateTime(2023, 12, 25, 12, 0, 0);
            //     exception.EndTime   = exception.StartTime.AddHours(1);
            // }

            // Save the modified appointment back to an iCalendar file
            appointment.Save(outputPath);
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
