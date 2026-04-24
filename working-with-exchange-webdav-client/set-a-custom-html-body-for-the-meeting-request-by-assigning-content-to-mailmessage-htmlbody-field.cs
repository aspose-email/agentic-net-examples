using System;
using Aspose.Email;
using Aspose.Email.Calendar;

class Program
{
    static void Main()
    {
        try
        {
            // Prepare attendees list
            MailAddressCollection attendees = new MailAddressCollection();
            attendees.Add(new MailAddress("person1@domain.com"));
            attendees.Add(new MailAddress("person2@domain.com"));

            // Create an appointment (meeting request)
            Appointment appointment = new Appointment(
                "Conference Room",
                new DateTime(2023, 12, 15, 10, 0, 0),
                new DateTime(2023, 12, 15, 11, 0, 0),
                new MailAddress("organizer@domain.com"),
                attendees);
            appointment.Summary = "Project Kickoff";
            appointment.Description = "Discuss project goals.";

            // Convert the appointment to a MailMessage
            using (MailMessage message = appointment.ToMailMessage())
            {
                // Set a custom HTML body for the meeting request
                message.HtmlBody = "<h1>Project Kickoff Meeting</h1><p>Please join us at 10:00 AM in Conference Room.</p>";

                // Additional required fields
                message.Subject = "Meeting Invitation: Project Kickoff";
                message.From = new MailAddress("organizer@domain.com");
                message.To.Add(new MailAddress("person1@domain.com"));
                message.To.Add(new MailAddress("person2@domain.com"));

                // Demonstrate that the HTML body is set
                Console.WriteLine("Custom HTML Body:");
                Console.WriteLine(message.HtmlBody);
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine(ex.Message);
        }
    }
}
