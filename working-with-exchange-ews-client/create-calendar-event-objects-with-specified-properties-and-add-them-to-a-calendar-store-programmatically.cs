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
            // Define the output file path for the calendar event
            string outputFilePath = "output/meeting.ics";

            // Ensure the output directory exists
            string outputDirectory = Path.GetDirectoryName(outputFilePath);
            if (!Directory.Exists(outputDirectory))
            {
                Directory.CreateDirectory(outputDirectory);
            }

            // Prepare attendees collection
            MailAddressCollection attendees = new MailAddressCollection();
            attendees.Add(new MailAddress("alice@example.com"));
            attendees.Add(new MailAddress("bob@example.com"));
            attendees.Add(new MailAddress("carol@example.com"));

            // Create the appointment
            Appointment appointment = new Appointment(
                "Conference Room 1",
                new DateTime(2023, 12, 15, 10, 0, 0),
                new DateTime(2023, 12, 15, 11, 0, 0),
                new MailAddress("organizer@example.com"),
                attendees);

            // Set additional properties
            appointment.Summary = "Project Kickoff Meeting";
            appointment.Description = "Discuss project goals, timelines, and responsibilities.";
            appointment.Location = "Conference Room 1";

            // Save the appointment to an iCalendar file
            try
            {
                appointment.Save(outputFilePath);
                Console.WriteLine("Appointment saved successfully to: " + outputFilePath);
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine("Failed to save appointment: " + ex.Message);
                return;
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine("Unexpected error: " + ex.Message);
        }
    }
}