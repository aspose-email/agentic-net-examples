using System;
using System.IO;
using Aspose.Email;
using Aspose.Email.Calendar;

class Program
{
    static void Main(string[] args)
    {
        try
        {
            // Prepare attendees
            MailAddressCollection attendees = new MailAddressCollection();
            attendees.Add(new MailAddress("person1@example.com"));
            attendees.Add(new MailAddress("person2@example.com"));
            attendees.Add(new MailAddress("person3@example.com"));

            // Create the appointment
            Appointment appointment = new Appointment(
                "Conference Room 1",
                new DateTime(2023, 12, 15, 10, 0, 0),
                new DateTime(2023, 12, 15, 11, 0, 0),
                new MailAddress("organizer@example.com"),
                attendees);

            appointment.Summary = "Project Kickoff Meeting";
            appointment.Description = "Discuss project goals, timeline, and responsibilities.";

            // Define the file path to persist the appointment
            string filePath = "appointment.ics";

            // Ensure the target directory exists
            string directory = Path.GetDirectoryName(filePath);
            if (!string.IsNullOrEmpty(directory) && !Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }

            // Save the appointment to an iCalendar file
            try
            {
                using (FileStream fileStream = new FileStream(filePath, FileMode.Create, FileAccess.Write))
                {
                    appointment.Save(fileStream, AppointmentSaveFormat.Ics);
                }

                Console.WriteLine("Appointment saved to: " + Path.GetFullPath(filePath));
            }
            catch (Exception ioEx)
            {
                Console.Error.WriteLine("Failed to save appointment: " + ioEx.Message);
                return;
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine("Error: " + ex.Message);
        }
    }
}