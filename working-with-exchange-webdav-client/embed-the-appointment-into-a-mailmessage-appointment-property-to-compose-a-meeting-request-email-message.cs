using System;
using Aspose.Email;
using Aspose.Email.Calendar;
using Aspose.Email.Clients.Smtp;

class Program
{
    static void Main()
    {
        try
        {
            // Prepare placeholder SMTP settings
            string smtpHost = "smtp.example.com";
            int smtpPort = 25;
            string smtpUser = "user";
            string smtpPass = "password";

            // Guard against using placeholder credentials/hosts
            if (smtpHost.Contains("example.com"))
            {
                Console.Error.WriteLine("Placeholder SMTP settings detected. Skipping send operation.");
                return;
            }

            // Create a new mail message
            using (MailMessage msg = new MailMessage())
            {
                msg.From = new MailAddress("organizer@domain.com");
                msg.To.Add(new MailAddress("attendee1@domain.com"));
                msg.Subject = "Meeting Request";
                msg.Body = "Please find the meeting details attached.";

                // Prepare attendees collection
                MailAddressCollection attendees = new MailAddressCollection();
                attendees.Add(new MailAddress("person1@domain.com"));
                attendees.Add(new MailAddress("person2@domain.com"));
                attendees.Add(new MailAddress("person3@domain.com"));

                // Create an appointment
                Appointment app = new Appointment(
                    "Conference Room 112",
                    new DateTime(2024, 5, 20, 13, 0, 0),
                    new DateTime(2024, 5, 20, 14, 0, 0),
                    new MailAddress("organizer@domain.com"),
                    attendees);
                app.Summary = "Release Meeting";
                app.Description = "Discuss the upcoming release.";

                // Add the appointment as an alternate view (meeting request)
                using (AlternateView calendarView = app.RequestApointment())
                {
                    msg.AddAlternateView(calendarView);
                }

                // Send the email using SMTP client
                using (SmtpClient smtp = new SmtpClient(smtpHost, smtpPort, smtpUser, smtpPass))
                {
                    smtp.Send(msg);
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
