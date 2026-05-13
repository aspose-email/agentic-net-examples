using System;
using Aspose.Email;
using Aspose.Email.Calendar;

class Program
{
    static void Main()
    {
        try
        {
            // Prepare attendees collection
            MailAddressCollection attendees = new MailAddressCollection();
            attendees.Add(new MailAddress("person1@domain.com"));
            attendees.Add(new MailAddress("person2@domain.com"));

            // Create an appointment instance
            Appointment appointment = new Appointment(
                "Conference Room",
                new DateTime(2023, 10, 1, 9, 0, 0),
                new DateTime(2023, 10, 1, 10, 0, 0),
                new MailAddress("organizer@domain.com"),
                attendees);

            // Assign an HTML description
            appointment.HtmlDescription = "<p>This is an <b>HTML</b> description of the appointment.</p>";

            // Retrieve the HTML description
            string htmlDescription = appointment.HtmlDescription;

            Console.WriteLine("HTML Description:");
            Console.WriteLine(htmlDescription);
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
