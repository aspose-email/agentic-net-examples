using System;
using Aspose.Email;
using Aspose.Email.Calendar;
using Aspose.Email.Calendar.Recurrences;
using Aspose.Email.Clients.Smtp;

class Program
{
    static void Main()
    {
        try
        {
            // Placeholder SMTP settings
            string smtpHost = "smtp.example.com";
            int smtpPort = 25;
            string smtpUser = "user@example.com";
            string smtpPass = "password";

            // Guard against placeholder credentials
            if (smtpHost.Contains("example.com"))
            {
                Console.Error.WriteLine("Placeholder SMTP configuration detected. Skipping send operation.");
                return;
            }

            // Attendees collection
            MailAddressCollection attendees = new MailAddressCollection();
            attendees.Add(new MailAddress("person1@domain.com"));
            attendees.Add(new MailAddress("person2@domain.com"));
            attendees.Add(new MailAddress("person3@domain.com"));

            // Create the appointment
            Appointment appointment = new Appointment(
                location: "Room 112",
                summary: "Release Meeting",
                description: "Discuss the next release",
                startDate: new DateTime(2026, 4, 1, 13, 0, 0),
                endDate: new DateTime(2026, 4, 1, 14, 0, 0),
                organizer: new MailAddress("organizer@domain.com"),
                attendees: attendees);

            // Set a daily recurrence ending on a specific date (no OccurrenceCount property)
            DailyRecurrencePattern recurrence = new DailyRecurrencePattern(5, 1);
            recurrence.EndDate = new DateTime(2026, 4, 5);
            appointment.Recurrence = recurrence;

            // Build the meeting request message
            MailMessage message = new MailMessage();
            message.From = new MailAddress("organizer@domain.com");
            foreach (MailAddress addr in attendees)
            {
                message.To.Add(addr);
            }
            message.Subject = appointment.Summary;
            message.AlternateViews.Add(appointment.RequestApointment());

            // Send the meeting request via SMTP
            using (SmtpClient client = new SmtpClient(smtpHost, smtpPort, smtpUser, smtpPass))
            {
                client.Send(message);
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine(ex.Message);
        }
    }
}
