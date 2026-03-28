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
            // Define the output file path for the iCalendar file
            string outputPath = "appointment.ics";

            // Ensure the target directory exists
            string directory = Path.GetDirectoryName(outputPath);
            if (!string.IsNullOrEmpty(directory) && !Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }

            // Prepare attendees list
            MailAddressCollection attendees = new MailAddressCollection();
            attendees.Add(new MailAddress("person1@domain.com"));
            attendees.Add(new MailAddress("person2@domain.com"));
            attendees.Add(new MailAddress("person3@domain.com"));

            // Create an appointment
            Appointment appointment = new Appointment(
                "Conference Room",
                new DateTime(2023, 10, 1, 13, 0, 0),
                new DateTime(2023, 10, 1, 14, 0, 0),
                new MailAddress("organizer@domain.com"),
                attendees);
            appointment.Summary = "Project Kickoff";
            appointment.Description = "Initial meeting to discuss project scope and timeline.";

            // Save the appointment as an .ics file using default save options
            appointment.Save(outputPath);
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine(ex.Message);
        }
    }
}
