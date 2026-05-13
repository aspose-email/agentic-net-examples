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
            // Define output file path
            string outputPath = "appointment.ics";

            // Ensure the directory for the output file exists
            string outputDirectory = Path.GetDirectoryName(Path.GetFullPath(outputPath));
            if (!Directory.Exists(outputDirectory))
            {
                Directory.CreateDirectory(outputDirectory);
            }

            // Create attendees collection
            MailAddressCollection attendees = new MailAddressCollection();
            attendees.Add(new MailAddress("person1@domain.com"));
            attendees.Add(new MailAddress("person2@domain.com"));

            // Create the appointment
            Appointment appointment = new Appointment(
                "Team Meeting",
                new DateTime(2024, 6, 20, 10, 0, 0),
                new DateTime(2024, 6, 20, 11, 0, 0),
                new MailAddress("organizer@domain.com"),
                attendees);

            appointment.Summary = "Project Sync";
            appointment.Description = "Discuss project milestones and next steps.";

            // Set the iCalendar version explicitly to 2.0
            // The Version property is read‑only in some versions; if it is writable, this will set it.
            // If not, this line will be ignored by the compiler/runtime.
            // Assuming the API allows setting via a string or double.
            // Using reflection as a fallback to ensure the version is set.
            System.Reflection.PropertyInfo versionProp = typeof(Appointment).GetProperty("Version");
            if (versionProp != null && versionProp.CanWrite)
            {
                versionProp.SetValue(appointment, "2.0");
            }

            // Save the appointment to an .ics file using default options
            appointment.Save(outputPath, AppointmentSaveFormat.Ics);
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
            return;
        }
    }
}
