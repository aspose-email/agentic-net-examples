using System;
using System.IO;
using Aspose.Email;
using Aspose.Email.Calendar;
using Aspose.Email.Clients.Smtp;

class Program
{
    static void Main()
    {
        try
        {
            // Prepare participants
            MailAddressCollection attendees = new MailAddressCollection();
            attendees.Add(new MailAddress("person1@domain.com"));
            attendees.Add(new MailAddress("person2@domain.com"));
            attendees.Add(new MailAddress("person3@domain.com"));

            // Organizer address
            MailAddress organizer = new MailAddress("organizer@domain.com");

            // Create the appointment (meeting request)
            Appointment meeting = new Appointment(
                location: "Conference Room 1",
                summary: "Project Kick‑off",
                description: "Discuss project goals and timeline.",
                startDate: new DateTime(2024, 10, 15, 10, 0, 0),
                endDate: new DateTime(2024, 10, 15, 11, 0, 0),
                organizer: organizer,
                attendees: attendees);

            // ---- Audio reminder (if supported) ----
            // The Appointment class in this version does not expose a direct Reminder property.
            // If a Reminder property becomes available, you could assign an AppointmentReminder
            // and attach an audio file (e.g., WAV) to it.
            // Example (commented out because the API is not present):
            // meeting.Reminder = new AppointmentReminder();
            // meeting.Reminder.Attachments.Add(new ReminderAttachment("reminder.wav"));
            // ------------------------------------------------

            // Prepare the email message
            MailMessage msg = new MailMessage();
            msg.From = organizer;
            foreach (MailAddress attendee in attendees)
            {
                msg.To.Add(attendee);
            }
            msg.Subject = "Meeting Invitation: Project Kick‑off";
            msg.Body = "Please find the meeting invitation attached.";

            // Add the meeting request as an alternate view
            msg.AddAlternateView(meeting.RequestApointment());

            // Attach agenda PDF (guard file I/O)
            string agendaPath = "agenda.pdf";
            if (!File.Exists(agendaPath))
            {
                // Create a minimal placeholder PDF if missing
                File.WriteAllBytes(agendaPath, new byte[] { 0x25, 0x50, 0x44, 0x46, 0x2D, 0x31, 0x2E, 0x34, 0x0A, 0x25, 0xE2, 0xE3, 0xCF, 0xD3, 0x0A, 0x31, 0x20, 0x30, 0x20, 0x6F, 0x62, 0x6A, 0x0A, 0x3C, 0x3C, 0x2F, 0x54, 0x79, 0x70, 0x65, 0x2F, 0x43, 0x61, 0x74, 0x61, 0x6C, 0x6F, 0x67, 0x3E, 0x3E, 0x0A, 0x65, 0x6E, 0x64, 0x6F, 0x62, 0x6A, 0x0A });
            }
            msg.AddAttachment(new Attachment(agendaPath));

            // SMTP client configuration (placeholder guard)
            string smtpHost = "smtp.server.com";
            int smtpPort = 25;
            string smtpUser = "user";
            string smtpPass = "password";

            bool isPlaceholder = smtpHost.Contains("smtp.server.com") ||
                                 smtpUser.Equals("user", StringComparison.OrdinalIgnoreCase) ||
                                 smtpPass.Equals("password", StringComparison.OrdinalIgnoreCase);

            if (isPlaceholder)
            {
                Console.Error.WriteLine("SMTP credentials are placeholders; skipping send operation.");
                return;
            }

            // Send the email (client connection safety)
            using (SmtpClient smtp = new SmtpClient(smtpHost, smtpPort, smtpUser, smtpPass))
            {
                try
                {
                    smtp.Send(msg);
                    Console.WriteLine("Meeting invitation sent successfully.");
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to send email: {ex.Message}");
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
