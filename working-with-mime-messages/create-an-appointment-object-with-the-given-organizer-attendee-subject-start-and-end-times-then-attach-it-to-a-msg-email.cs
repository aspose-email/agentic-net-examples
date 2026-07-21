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
            // Output MSG file path
            string outputPath = "AppointmentMessage.msg";

            // Ensure the output directory exists
            string directory = Path.GetDirectoryName(outputPath);
            if (!string.IsNullOrEmpty(directory) && !Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }

            // Prepare attendees
            MailAddressCollection attendees = new MailAddressCollection();
            attendees.Add(new MailAddress("attendee1@domain.com"));
            attendees.Add(new MailAddress("attendee2@domain.com"));

            // Create the appointment
            Appointment appointment = new Appointment(
                "Conference Room",
                new DateTime(2023, 12, 15, 10, 0, 0),
                new DateTime(2023, 12, 15, 11, 0, 0),
                new MailAddress("organizer@domain.com"),
                attendees);
            appointment.Summary = "Project Kickoff";
            appointment.Description = "Discuss project goals and timeline.";

            // Build the email message and attach the appointment
            using (MailMessage message = new MailMessage())
            {
                message.From = new MailAddress("organizer@domain.com");
                message.To.Add(new MailAddress("recipient@domain.com"));
                message.Subject = "Meeting Invitation";
                message.Body = "Please find the meeting invitation attached.";

                // Attach the calendar as an alternate view
                message.AddAlternateView(appointment.RequestApointment());

                // Save the message as a MSG file
                message.Save(outputPath);
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
