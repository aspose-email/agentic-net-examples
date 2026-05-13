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
            string outputPath = "output.ics";

            // Ensure the output directory exists
            string outputDirectory = Path.GetDirectoryName(outputPath);
            if (!string.IsNullOrEmpty(outputDirectory) && !Directory.Exists(outputDirectory))
            {
                Directory.CreateDirectory(outputDirectory);
            }

            // Create attendees collection
            MailAddressCollection attendees = new MailAddressCollection();
            attendees.Add(new MailAddress("person1@domain.com"));
            attendees.Add(new MailAddress("person2@domain.com"));

            // Create an appointment
            Appointment appointment = new Appointment(
                "Conference Room",
                new DateTime(2024, 6, 15, 10, 0, 0),
                new DateTime(2024, 6, 15, 11, 0, 0),
                new MailAddress("organizer@domain.com"),
                attendees);

            appointment.Summary = "Project Sync";
            appointment.Description = "Discuss project milestones.";

            // Set custom product identifier for the iCalendar file
            AppointmentIcsSaveOptions saveOptions = new AppointmentIcsSaveOptions();
            saveOptions.ProductId = "MyCustomApp v1.0";

            // Save the appointment to an iCalendar file
            using (FileStream fileStream = new FileStream(outputPath, FileMode.Create, FileAccess.Write))
            {
                appointment.Save(fileStream, saveOptions);
            }

            Console.WriteLine("iCalendar file saved to: " + Path.GetFullPath(outputPath));
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine("Error: " + ex.Message);
        }
    }
}
