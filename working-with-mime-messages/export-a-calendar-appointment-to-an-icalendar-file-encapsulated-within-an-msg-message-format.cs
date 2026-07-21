using System;
using System.IO;
using Aspose.Email;
using Aspose.Email.Calendar;
using Aspose.Email.Mapi;

namespace AsposeEmailExample
{
    class Program
    {
        static void Main()
        {
            try
            {
                // Ensure output directory exists
                string outputDirectory = "output";
                if (!Directory.Exists(outputDirectory))
                {
                    Directory.CreateDirectory(outputDirectory);
                }

                // Destination MSG file path
                string msgFilePath = Path.Combine(outputDirectory, "appointment.msg");

                // Prepare attendees
                MailAddressCollection attendees = new MailAddressCollection();
                attendees.Add(new MailAddress("person1@domain.com"));
                attendees.Add(new MailAddress("person2@domain.com"));
                attendees.Add(new MailAddress("person3@domain.com"));

                // Create the appointment
                Appointment appointment = new Appointment(
                    "Room 112",
                    new DateTime(2026, 8, 1, 13, 0, 0),
                    new DateTime(2026, 8, 1, 14, 0, 0),
                    new MailAddress("organizer@domain.com"),
                    attendees);
                appointment.Summary = "Project Meeting";
                appointment.Description = "Discuss project milestones.";

                // Convert the appointment to a MAPI message (MSG) which embeds the iCalendar data
                using (MapiMessage mapiMessage = appointment.ToMapiMessage())
                {
                    // Save the MSG file
                    mapiMessage.Save(msgFilePath);
                }

                Console.WriteLine($"Appointment successfully saved to: {msgFilePath}");
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Error: {ex.Message}");
            }
        }
    }
}
