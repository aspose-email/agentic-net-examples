using System;
using Aspose.Email;
using Aspose.Email.Clients.Smtp;
using Aspose.Email.Calendar;

namespace MeetingRequestSample
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                // Prepare the email message.
                using (MailMessage msg = new MailMessage())
                {
                    // Set the organizer as the sender.
                    MailAddress organizer = new MailAddress("organizer@domain.com");
                    msg.From = organizer;

                    // Define attendees.
                    MailAddressCollection attendees = new MailAddressCollection
                    {
                        new MailAddress("person1@domain.com"),
                        new MailAddress("person2@domain.com"),
                        new MailAddress("person3@domain.com")
                    };

                    // Add attendees to the message recipients.
                    foreach (MailAddress attendee in attendees)
                    {
                        msg.To.Add(attendee);
                    }

                    // Create the appointment (meeting request).
                    Appointment app = new Appointment(
                        "Room 112",
                        new DateTime(2026, 10, 1, 13, 0, 0),
                        new DateTime(2026, 10, 1, 14, 0, 0),
                        organizer,
                        attendees)
                    {
                        Summary = "Release Meeting",
                        Description = "Discuss the next release"
                    };

                    // Attach the iCalendar representation to the email.
                    msg.AlternateViews.Add(app.RequestApointment());

                    // SMTP configuration (placeholders).
                    string smtpHost = "smtp.server.com";
                    int smtpPort = 25;
                    string smtpUser = "user";
                    string smtpPass = "password";

                    // Guard: skip actual sending when placeholders are detected.
                    bool placeholdersDetected = smtpHost.Contains("smtp.server.com") ||
                                                smtpUser.Equals("user", StringComparison.OrdinalIgnoreCase) ||
                                                smtpPass.Equals("password", StringComparison.OrdinalIgnoreCase);

                    if (placeholdersDetected)
                    {
                        Console.WriteLine("Placeholder SMTP credentials detected. Skipping send operation.");
                        Console.WriteLine("Meeting request prepared successfully.");
                    }
                    else
                    {
                        using (SmtpClient smtp = new SmtpClient(smtpHost, smtpPort, smtpUser, smtpPass))
                        {
                            smtp.Send(msg);
                            Console.WriteLine("Meeting request sent successfully.");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine("Unexpected error: " + ex.Message);
            }
        }
    }
}
