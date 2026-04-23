using System;
using System.IO;
using System.Text;
using Aspose.Email;
using Aspose.Email.Calendar;

class Program
{
    static void Main()
    {
        try
        {
            // Prepare attendees
            MailAddressCollection attendees = new MailAddressCollection();
            attendees.Add(new MailAddress("person1@domain.com"));
            attendees.Add(new MailAddress("person2@domain.com"));

            // Create an appointment (meeting request)
            Appointment appointment = new Appointment(
                "Room 112",
                new DateTime(2023, 10, 1, 13, 0, 0),
                new DateTime(2023, 10, 1, 14, 0, 0),
                new MailAddress("organizer@domain.com"),
                attendees);
            appointment.Summary = "Project Meeting";
            appointment.Description = "Discuss project status";

            // Build the email message
            using (MailMessage message = new MailMessage())
            {
                message.From = "organizer@domain.com";
                message.To.Add("receiver@domain.com");
                message.Subject = "Meeting Invitation";

                // Add the meeting request as an alternate view
                AlternateView meetingView = appointment.RequestApointment();
                message.AlternateViews.Add(meetingView);

                // Create a custom attachment from a memory stream
                byte[] data = Encoding.UTF8.GetBytes("This is the content of the custom attachment.");
                using (MemoryStream ms = new MemoryStream(data))
                {
                    Attachment customAttachment = new Attachment(ms, "application/octet-stream");
                    customAttachment.Name = "CustomFile.txt";
                    message.Attachments.Add(customAttachment);
                }

                // Save the message to disk (guarded file I/O)
                string outputPath = "MeetingWithAttachment.eml";
                string directory = Path.GetDirectoryName(Path.GetFullPath(outputPath));
                if (!Directory.Exists(directory))
                {
                    Directory.CreateDirectory(directory);
                }
                message.Save(outputPath);
                Console.WriteLine($"Message saved to {outputPath}");
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
