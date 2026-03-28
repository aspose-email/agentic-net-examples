using System;
using System.IO;
using Aspose.Email;
using Aspose.Email.Calendar;
using Aspose.Email.Mime;

class Program
{
    static void Main()
    {
        try
        {
            // Define output MSG file path
            string outputPath = Path.Combine(Directory.GetCurrentDirectory(), "AppointmentEmail.msg");

            // Ensure the directory exists
            string outputDir = Path.GetDirectoryName(outputPath);
            if (!Directory.Exists(outputDir))
            {
                Directory.CreateDirectory(outputDir);
            }

            // Create attendees collection
            MailAddressCollection attendees = new MailAddressCollection();
            attendees.Add(new MailAddress("attendee@example.com"));

            // Create the appointment
            Appointment appointment = new Appointment(
                location: "Conference Room",
                startDate: new DateTime(2024, 5, 20, 10, 0, 0),
                endDate: new DateTime(2024, 5, 20, 11, 0, 0),
                organizer: new MailAddress("organizer@example.com"),
                attendees: attendees);

            appointment.Summary = "Project Kickoff";
            appointment.Description = "Discuss project goals and timelines.";

            // Create a mail message and attach the appointment as an alternate view
            using (MailMessage msg = new MailMessage())
            {
                msg.From = new MailAddress("organizer@example.com");
                msg.To.Add("attendee@example.com");
                msg.Subject = "Meeting Invitation";

                // Attach the appointment
                msg.AddAlternateView(appointment.RequestApointment());

                // Save the message as MSG
                msg.Save(outputPath, SaveOptions.DefaultMsgUnicode);
            }

            Console.WriteLine("MSG file created at: " + outputPath);
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine("Error: " + ex.Message);
        }
    }
}
