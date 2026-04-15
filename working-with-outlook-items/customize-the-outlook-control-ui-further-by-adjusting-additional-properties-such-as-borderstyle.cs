using System;
using Aspose.Email;
using Aspose.Email.Clients.Smtp;
using Aspose.Email.Calendar;
using Aspose.Email.Calendar.Recurrences;

class Program
{
    static void Main()
    {
        try
        {
            // Placeholder SMTP settings – skip actual send when placeholders are detected
            string smtpHost = "smtp.example.com";
            int smtpPort = 25;
            string smtpUser = "user";
            string smtpPass = "password";

            if (smtpHost.Contains("example.com"))
            {
                Console.Error.WriteLine("Placeholder SMTP configuration detected. Skipping send operation.");
                return;
            }

            // Create a mail message
            using (MailMessage msg = new MailMessage())
            {
                // Prepare attendees
                MailAddressCollection attendees = new MailAddressCollection();
                attendees.Add(new MailAddress("person1@domain.com"));
                attendees.Add(new MailAddress("person2@domain.com"));
                attendees.Add(new MailAddress("person3@domain.com"));

                // Create an appointment using the location‑based constructor
                Appointment app = new Appointment(
                    "Room 112",
                    new DateTime(2026, 5, 20, 13, 0, 0),
                    new DateTime(2026, 5, 20, 14, 0, 0),
                    new MailAddress("organizer@domain.com"),
                    attendees);

                // Explicitly set Summary and Description as required by validation rules
                app.Summary = "Project Kickoff";
                app.Description = "Discuss project goals and timelines.";

                // Add the appointment request as an alternate view
                msg.AddAlternateView(app.RequestApointment());

                // Send the email via SMTP
                using (SmtpClient smtp = new SmtpClient(smtpHost, smtpPort, smtpUser, smtpPass))
                {
                    try
                    {
                        smtp.Send(msg);
                        Console.WriteLine("Email with appointment sent successfully.");
                    }
                    catch (Exception ex)
                    {
                        Console.Error.WriteLine($"SMTP send failed: {ex.Message}");
                    }
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
