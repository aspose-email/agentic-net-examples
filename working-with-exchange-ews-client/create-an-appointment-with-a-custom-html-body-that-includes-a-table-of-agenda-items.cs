using System;
using System.IO;
using Aspose.Email;
using Aspose.Email.Calendar;

class Program
{
    static void Main(string[] args)
    {
        try
        {
            // Prepare attendees
            MailAddressCollection attendees = new MailAddressCollection();
            attendees.Add(new MailAddress("person1@domain.com"));
            attendees.Add(new MailAddress("person2@domain.com"));
            attendees.Add(new MailAddress("person3@domain.com"));

            // Create the appointment
            Appointment appointment = new Appointment(
                "Conference Room",
                new DateTime(2023, 12, 1, 10, 0, 0),
                new DateTime(2023, 12, 1, 11, 0, 0),
                new MailAddress("organizer@domain.com"),
                attendees);

            appointment.Summary = "Project Kickoff";
            appointment.HtmlDescription = @"
<html>
<body>
<h2>Agenda</h2>
<table border='1' cellpadding='5' cellspacing='0'>
<tr><th>Time</th><th>Topic</th></tr>
<tr><td>10:00</td><td>Introduction</td></tr>
<tr><td>10:30</td><td>Requirements</td></tr>
</table>
</body>
</html>";

            // Define output file
            string filePath = "appointment.ics";

            // Ensure directory exists
            string directory = Path.GetDirectoryName(filePath);
            if (!string.IsNullOrEmpty(directory) && !Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }

            // Save the appointment with file I/O guard
            try
            {
                appointment.Save(filePath);
                Console.WriteLine($"Appointment saved to {filePath}");
            }
            catch (Exception ioEx)
            {
                Console.Error.WriteLine($"Failed to save appointment: {ioEx.Message}");
                return;
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
