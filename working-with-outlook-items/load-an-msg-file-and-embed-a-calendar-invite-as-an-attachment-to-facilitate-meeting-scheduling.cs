using Aspose.Email.Clients;
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
            string inputPath = "input.msg";
            string outputPath = "output.msg";

            if (!File.Exists(inputPath))
            {
                try
                {
                    using (MailMessage placeholder = new MailMessage(
                        "sender@example.com",
                        "recipient@example.com",
                        "Placeholder Subject",
                        "Placeholder body."))
                    {
                        placeholder.Save(inputPath, new MsgSaveOptions(MailMessageSaveType.OutlookMessageFormat));
                    }
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Error creating placeholder MSG: {ex.Message}");
                    return;
                }

                Console.Error.WriteLine($"Input file not found: {inputPath}");
                return;
            }

            using (MailMessage message = MailMessage.Load(inputPath))
            {
                // Attendees for the meeting
                MailAddressCollection attendees = new MailAddressCollection();
                attendees.Add(new MailAddress("person1@domain.com"));
                attendees.Add(new MailAddress("person2@domain.com"));

                // Create the appointment
                Appointment appointment = new Appointment(
                    "Meeting Room",
                    new DateTime(2024, 5, 20, 10, 0, 0),
                    new DateTime(2024, 5, 20, 11, 0, 0),
                    new MailAddress("organizer@domain.com"),
                    attendees);
                appointment.Summary = "Project Meeting";
                appointment.Description = "Discuss project milestones.";

                // Convert appointment to a mail message and attach it
                using (MailMessage calendarMessage = appointment.ToMailMessage())
                {
                    using (MemoryStream ms = new MemoryStream())
                    {
                        calendarMessage.Save(ms, SaveOptions.DefaultEml);
                        ms.Position = 0;
                        Attachment calendarAttachment = new Attachment(ms, "invite.eml", "message/rfc822");
                        message.Attachments.Add(calendarAttachment);
                    }
                }

                // Save the modified message
                message.Save(outputPath, SaveOptions.DefaultMsgUnicode);
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine(ex.Message);
        }
    }
}
