using Aspose.Email.Calendar;
using System;
using System.IO;
using Aspose.Email;

class Program
{
    static void Main()
    {
        try
        {
            string outputPath = "appointment.ics";

            // Ensure the output directory exists
            string directory = Path.GetDirectoryName(Path.GetFullPath(outputPath));
            if (!Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }

            // Define appointment details
            DateTime startDate = new DateTime(2023, 10, 1, 9, 0, 0);
            DateTime endDate = startDate.AddHours(1);
            MailAddress organizer = new MailAddress("organizer@example.com");
            MailAddressCollection attendees = new MailAddressCollection();
            attendees.Add(new MailAddress("attendee1@example.com"));
            attendees.Add(new MailAddress("attendee2@example.com"));

            // Create the appointment
            Appointment appointment = new Appointment("Conference Room", startDate, endDate, organizer, attendees);
            appointment.Summary = "Team Meeting";
            appointment.Description = "Discuss project status";

            // Save the appointment to an iCalendar file
            appointment.Save(outputPath);

            // Load the appointment back from the file
            Appointment loadedAppointment = Appointment.Load(outputPath);

            // Verify that the duration is exactly 60 minutes
            TimeSpan duration = loadedAppointment.EndDate - loadedAppointment.StartDate;
            if (Math.Abs(duration.TotalMinutes - 60) < 0.001)
            {
                Console.WriteLine("Duration verified: 60 minutes.");
            }
            else
            {
                Console.WriteLine($"Duration mismatch: {duration.TotalMinutes} minutes.");
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine(ex.Message);
        }
    }
}
