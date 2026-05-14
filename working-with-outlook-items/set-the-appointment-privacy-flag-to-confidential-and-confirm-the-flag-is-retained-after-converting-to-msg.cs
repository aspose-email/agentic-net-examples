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
            // Define output MSG file path
            string msgPath = "appointment.msg";

            // Ensure the directory for the output file exists
            string directory = Path.GetDirectoryName(Path.GetFullPath(msgPath));
            if (!Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }

            // Create organizer and attendees
            MailAddress organizer = new MailAddress("organizer@example.com");
            MailAddressCollection attendees = new MailAddressCollection();
            attendees.Add(new MailAddress("attendee1@example.com"));
            attendees.Add(new MailAddress("attendee2@example.com"));

            // Create an appointment and set its privacy to Confidential
            Appointment appointment = new Appointment(
                "Project Discussion",
                DateTime.Now.AddHours(1),
                DateTime.Now.AddHours(2),
                organizer,
                attendees);
            appointment.Summary = "Project Discussion";
            appointment.Description = "Discuss project milestones.";
            appointment.Class = AppointmentClass.Confidential; // privacy flag

            // Convert the appointment to a MAPI message and save it as MSG
            using (MapiMessage mapiMsg = appointment.ToMapiMessage())
            {
                mapiMsg.Save(msgPath);
            }

            // Verify that the confidentiality flag is retained after loading the MSG
            if (File.Exists(msgPath))
            {
                using (MapiMessage loadedMsg = MapiMessage.Load(msgPath))
                {
                    // The Sensitivity property reflects the confidentiality setting
                    Console.WriteLine("Sensitivity after load: " + loadedMsg.Sensitivity);
                }
            }
            else
            {
                Console.Error.WriteLine("Failed to create the MSG file.");
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine("Error: " + ex.Message);
        }
    }
}
