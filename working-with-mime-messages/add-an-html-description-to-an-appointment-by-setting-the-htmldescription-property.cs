using System;
using Aspose.Email;
using Aspose.Email.Calendar;

class Program
{
    static void Main()
    {
        try
        {
            // Prepare attendees
            MailAddressCollection attendees = new MailAddressCollection();
            attendees.Add(new MailAddress("person1@domain.com"));
            attendees.Add(new MailAddress("person2@domain.com"));

            // Create an appointment
            Appointment appointment = new Appointment(
                "Conference Room",
                new DateTime(2023, 10, 20, 10, 0, 0),
                new DateTime(2023, 10, 20, 11, 0, 0),
                new MailAddress("organizer@domain.com"),
                attendees);

            // Add HTML description
            appointment.HtmlDescription = "<p>This is an <b>HTML</b> description of the meeting.</p>";

            // Output the HTML description to verify
            Console.WriteLine("HTML Description set:");
            Console.WriteLine(appointment.HtmlDescription);
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine(ex.Message);
        }
    }
}
