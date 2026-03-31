using System;
using System.IO;
using Aspose.Email;
using Aspose.Email.Calendar;
using Aspose.Email.Calendar.Recurrences;

class Program
{
    static void Main(string[] args)
    {
        try
        {
            // Define output MSG file path
            string outputPath = "Appointment.msg";

            // Ensure the output directory exists
            string directory = Path.GetDirectoryName(outputPath);
            if (!string.IsNullOrEmpty(directory) && !Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }

            // Prepare attendees
            MailAddressCollection attendees = new MailAddressCollection();
            attendees.Add(new MailAddress("person1@domain.com"));
            attendees.Add(new MailAddress("person2@domain.com"));

            // Create the appointment
            Appointment appointment = new Appointment(
                "Team Meeting",
                new DateTime(2023, 12, 15, 10, 0, 0),
                new DateTime(2023, 12, 15, 11, 0, 0),
                new MailAddress("organizer@domain.com"),
                attendees);
            appointment.Summary = "Project Sync";
            appointment.Description = "Discuss project status and next steps.";

            // Save the appointment as MSG using AppointmentMsgSaveOptions
            AppointmentMsgSaveOptions saveOptions = new AppointmentMsgSaveOptions();
            appointment.Save(outputPath, saveOptions);

            Console.WriteLine($"Appointment saved to {outputPath}");
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
