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
            string outputPath = "appointment.msg";
            string directory = Path.GetDirectoryName(outputPath);
            if (!string.IsNullOrEmpty(directory) && !Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }

            MailAddressCollection attendees = new MailAddressCollection();
            attendees.Add(new MailAddress("person1@domain.com"));
            attendees.Add(new MailAddress("person2@domain.com"));
            attendees.Add(new MailAddress("person3@domain.com"));

            Appointment appointment = new Appointment(
                "Conference Room",
                new DateTime(2023, 10, 1, 13, 0, 0),
                new DateTime(2023, 10, 1, 14, 0, 0),
                new MailAddress("organizer@domain.com"),
                attendees);
            appointment.Summary = "Project Kickoff";
            appointment.Description = "Initial project kickoff meeting.";

            AppointmentMsgSaveOptions saveOptions = new AppointmentMsgSaveOptions();
            appointment.Save(outputPath, saveOptions);

            Console.WriteLine($"Appointment saved to {outputPath}");
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine(ex.Message);
        }
    }
}
