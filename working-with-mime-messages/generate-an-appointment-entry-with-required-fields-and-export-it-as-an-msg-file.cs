using System;
using System.IO;
using Aspose.Email;
using Aspose.Email.Calendar;
using Aspose.Email.Mapi;

class Program
{
    static void Main()
    {
        try
        {
            // Ensure output directory exists
            string outputDir = "Output";
            if (!Directory.Exists(outputDir))
            {
                Directory.CreateDirectory(outputDir);
            }

            string msgPath = Path.Combine(outputDir, "appointment.msg");

            // Prepare attendees
            MailAddressCollection attendees = new MailAddressCollection();
            attendees.Add(new MailAddress("person1@domain.com"));
            attendees.Add(new MailAddress("person2@domain.com"));
            attendees.Add(new MailAddress("person3@domain.com"));

            // Create appointment with required fields
            Appointment appointment = new Appointment(
                "Conference Room 101",
                new DateTime(2023, 12, 15, 10, 0, 0),
                new DateTime(2023, 12, 15, 11, 0, 0),
                new MailAddress("organizer@domain.com"),
                attendees);
            appointment.Summary = "Project Kickoff Meeting";
            appointment.Description = "Discuss project objectives, timeline, and responsibilities.";

            // Convert to MAPI message and save as MSG file
            using (MapiMessage mapiMessage = appointment.ToMapiMessage())
            {
                mapiMessage.Save(msgPath);
            }

            Console.WriteLine("Appointment saved to: " + msgPath);
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine("Error: " + ex.Message);
        }
    }
}
