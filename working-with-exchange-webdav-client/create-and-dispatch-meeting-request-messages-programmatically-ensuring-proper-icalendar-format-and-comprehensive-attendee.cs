using Aspose.Email.Clients;
using System;
using System.Net;
using Aspose.Email;
using Aspose.Email.Clients.Smtp;
using Aspose.Email.Calendar;

class Program
{
    static void Main()
    {
        try
        {
            // Create a new mail message
            using (MailMessage msg = new MailMessage())
            {
                msg.From = new MailAddress("organizer@example.com");
                msg.To.Add("attendee1@example.com");
                msg.To.Add("attendee2@example.com");
                msg.Subject = "Project Kickoff Meeting";

                // Prepare attendees collection
                MailAddressCollection attendees = new MailAddressCollection();
                attendees.Add(new MailAddress("attendee1@example.com"));
                attendees.Add(new MailAddress("attendee2@example.com"));
                attendees.Add(new MailAddress("attendee3@example.com"));

                // Create the appointment (meeting request)
                Appointment meeting = new Appointment(
                    "Conference Room 1",
                    new DateTime(2024, 5, 20, 10, 0, 0),
                    new DateTime(2024, 5, 20, 11, 0, 0),
                    new MailAddress("organizer@example.com"),
                    attendees);

                meeting.Summary = "Project Kickoff";
                meeting.Description = "Discuss project goals, timeline, and responsibilities.";
                meeting.Location = "Conference Room 1";

                // Add the iCalendar meeting request as an alternate view
                msg.AlternateViews.Add(meeting.RequestApointment());

                // Send the meeting request via SMTP
                using (SmtpClient smtp = new SmtpClient("smtp.example.com", 587, "user", "password"))
                {
                    smtp.SecurityOptions = SecurityOptions.Auto;
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
