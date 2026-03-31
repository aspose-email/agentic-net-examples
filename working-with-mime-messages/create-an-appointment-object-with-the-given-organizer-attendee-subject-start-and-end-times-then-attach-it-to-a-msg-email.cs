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
            // Output MSG file path
            string outputMsgPath = "appointment.msg";

            // Ensure the output directory exists
            string outputDir = Path.GetDirectoryName(outputMsgPath);
            if (!string.IsNullOrEmpty(outputDir) && !Directory.Exists(outputDir))
            {
                Directory.CreateDirectory(outputDir);
            }

            // Prepare attendees
            MailAddressCollection attendees = new MailAddressCollection();
            attendees.Add(new MailAddress("attendee@example.com"));

            // Create the appointment
            Appointment appointment = new Appointment(
                "Conference Room",
                new DateTime(2023, 10, 1, 9, 0, 0),
                new DateTime(2023, 10, 1, 10, 0, 0),
                new MailAddress("organizer@example.com"),
                attendees);
            appointment.Summary = "Project Meeting";
            appointment.Description = "Discuss project milestones.";

            // Convert appointment to a MAPI message and save as MSG
            using (MapiMessage mapiMessage = appointment.ToMapiMessage())
            {
                try
                {
                    mapiMessage.Save(outputMsgPath);
                    Console.WriteLine($"MSG file saved to {outputMsgPath}");
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to save MSG: {ex.Message}");
                    return;
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
