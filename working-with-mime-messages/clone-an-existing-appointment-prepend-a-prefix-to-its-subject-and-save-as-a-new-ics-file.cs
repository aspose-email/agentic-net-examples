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
            string outputPath = "cloned.ics";
            string prefix = "Copy - ";

            if (!File.Exists(inputPath))
            {
                Console.Error.WriteLine($"Input file '{inputPath}' does not exist.");
                return;
            }

            Appointment originalAppointment;
            try
            {
                originalAppointment = Appointment.Load(inputPath);
            }
            catch (Exception loadEx)
            {
                Console.Error.WriteLine($"Failed to load appointment: {loadEx.Message}");
                return;
            }

            // Create a new appointment based on the original
            Appointment clonedAppointment = new Appointment(
                originalAppointment.Location,
                originalAppointment.Summary,
                originalAppointment.Description,
                originalAppointment.StartDate,
                originalAppointment.EndDate,
                originalAppointment.Organizer,
                originalAppointment.Attendees);

            // Preserve recurrence and reminders if present
            clonedAppointment.Recurrence = originalAppointment.Recurrence;
            foreach (AppointmentReminder reminder in originalAppointment.Reminders)
            {
                clonedAppointment.Reminders.Add(reminder);
            }

            // Prepend the prefix to the subject (Summary)
            clonedAppointment.Summary = prefix + clonedAppointment.Summary;

            try
            {
                clonedAppointment.Save(outputPath);
                Console.WriteLine($"Cloned appointment saved to '{outputPath}'.");
            }
            catch (Exception saveEx)
            {
                Console.Error.WriteLine($"Failed to save cloned appointment: {saveEx.Message}");
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
