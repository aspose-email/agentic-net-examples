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
            // Define the output iCalendar file path
            string icsPath = "appointment.ics";

            // Ensure the directory for the output file exists
            string directory = Path.GetDirectoryName(icsPath);
            if (!string.IsNullOrEmpty(directory) && !Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }

            // Create a new appointment
            Appointment appointment = new Appointment(
                "Room 101",
                new DateTime(2023, 12, 25, 10, 0, 0),
                new DateTime(2023, 12, 25, 11, 0, 0),
                new MailAddress("organizer@example.com"),
                new MailAddressCollection());

            // Set the busy status to Free
            appointment.MicrosoftBusyStatus = MSBusyStatus.Free;

            // Save the appointment to an iCalendar file
            try
            {
                appointment.Save(icsPath);
                Console.WriteLine($"Appointment saved to '{icsPath}'.");
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Failed to save appointment: {ex.Message}");
                return;
            }

            // Verify that the busy status was saved correctly
            if (!File.Exists(icsPath))
            {
                Console.Error.WriteLine("The iCalendar file was not created.");
                return;
            }

            Appointment loadedAppointment;
            try
            {
                loadedAppointment = Appointment.Load(icsPath);
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Failed to load appointment: {ex.Message}");
                return;
            }

            if (loadedAppointment.MicrosoftBusyStatus == MSBusyStatus.Free)
            {
                Console.WriteLine("Busy status verified as Free in the exported iCalendar file.");
            }
            else
            {
                Console.WriteLine($"Busy status verification failed. Current status: {loadedAppointment.MicrosoftBusyStatus}");
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
