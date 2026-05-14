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
            // Define output MSG file path
            string outputPath = "appointment.msg";

            // Ensure the output directory exists
            string outputDir = Path.GetDirectoryName(outputPath);
            if (!string.IsNullOrEmpty(outputDir) && !Directory.Exists(outputDir))
            {
                Directory.CreateDirectory(outputDir);
            }

            // Create attendees collection
            MailAddressCollection attendees = new MailAddressCollection();
            attendees.Add(new MailAddress("person1@domain.com"));
            attendees.Add(new MailAddress("person2@domain.com"));

            // Create the appointment
            Appointment appointment = new Appointment(
                "Conference Room A",
                new DateTime(2024, 6, 1, 10, 0, 0),
                new DateTime(2024, 6, 1, 11, 0, 0),
                new MailAddress("organizer@domain.com"),
                attendees);

            // Set additional properties
            appointment.Summary = "Team Sync";
            appointment.Description = "Weekly team synchronization meeting.";
            appointment.Location = "Conference Room A";

            // Save the appointment as MSG using AppointmentMsgSaveOptions
            AppointmentMsgSaveOptions saveOptions = new AppointmentMsgSaveOptions();
            appointment.Save(outputPath, saveOptions);

            Console.WriteLine("Appointment saved to MSG file: " + outputPath);
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine("Error: " + ex.Message);
        }
    }
}
