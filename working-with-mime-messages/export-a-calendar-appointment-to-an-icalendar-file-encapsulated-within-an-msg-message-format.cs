using System;
using System.IO;
using Aspose.Email;
using Aspose.Email.Calendar;
using Aspose.Email.Mapi;

namespace ExportAppointment
{
    class Program
    {
        static void Main()
        {
            try
            {
                // Prepare output directory
                string outputDirectory = "Output";
                if (!Directory.Exists(outputDirectory))
                {
                    Directory.CreateDirectory(outputDirectory);
                }

                // Define file paths
                string icsFilePath = Path.Combine(outputDirectory, "appointment.ics");
                string msgFilePath = Path.Combine(outputDirectory, "appointment.msg");

                // Create attendees list
                MailAddressCollection attendees = new MailAddressCollection();
                attendees.Add(new MailAddress("person1@domain.com"));
                attendees.Add(new MailAddress("person2@domain.com"));

                // Create the appointment
                Appointment appointment = new Appointment(
                    "Room 112",
                    new DateTime(2023, 10, 1, 9, 0, 0),
                    new DateTime(2023, 10, 1, 10, 0, 0),
                    new MailAddress("organizer@domain.com"),
                    attendees);
                appointment.Summary = "Project Kickoff";
                appointment.Description = "Discuss project goals and timeline.";

                // Save the appointment as an iCalendar (.ics) file
                try
                {
                    appointment.Save(icsFilePath);
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to save iCalendar file: {ex.Message}");
                    return;
                }

                // Convert the appointment to a MSG message (encapsulating the iCalendar) and save
                try
                {
                    using (MapiMessage msg = appointment.ToMapiMessage())
                    {
                        msg.Save(msgFilePath);
                    }
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to create or save MSG file: {ex.Message}");
                    return;
                }

                Console.WriteLine("Appointment exported successfully.");
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Unexpected error: {ex.Message}");
            }
        }
    }
}
