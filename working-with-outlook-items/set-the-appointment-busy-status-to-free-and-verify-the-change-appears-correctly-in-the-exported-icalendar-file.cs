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
            // Define output directory and file path
            string outputDir = "Output";
            string icsPath = Path.Combine(outputDir, "appointment.ics");

            // Ensure the output directory exists
            if (!Directory.Exists(outputDir))
            {
                Directory.CreateDirectory(outputDir);
            }

            // Prepare attendees
            MailAddressCollection attendees = new MailAddressCollection();
            attendees.Add(new MailAddress("person1@example.com"));
            attendees.Add(new MailAddress("person2@example.com"));

            // Create the appointment
            Appointment appointment = new Appointment(
                "Team Meeting",
                new DateTime(2023, 12, 1, 10, 0, 0),
                new DateTime(2023, 12, 1, 11, 0, 0),
                new MailAddress("organizer@example.com"),
                attendees);

            appointment.Summary = "Project discussion";
            appointment.Description = "Discuss project milestones.";
            // Set busy status to Free
            appointment.MicrosoftBusyStatus = MSBusyStatus.Free;

            // Save the appointment to an iCalendar file
            try
            {
                appointment.Save(icsPath);
                Console.WriteLine($"Appointment saved to {icsPath}");
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Failed to save appointment: {ex.Message}");
                return;
            }

            // Verify the busy status by loading the saved file
            if (!File.Exists(icsPath))
            {
                Console.Error.WriteLine("Saved iCalendar file not found.");
                return;
            }

            Appointment loadedAppointment = null;
            try
            {
                loadedAppointment = Appointment.Load(icsPath);
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Failed to load appointment: {ex.Message}");
                return;
            }

            if (loadedAppointment != null)
            {
                Console.WriteLine($"Loaded Busy Status: {loadedAppointment.MicrosoftBusyStatus}");
                bool isFree = loadedAppointment.MicrosoftBusyStatus == MSBusyStatus.Free;
                Console.WriteLine(isFree ? "Busy status correctly set to Free." : "Busy status not set correctly.");
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
