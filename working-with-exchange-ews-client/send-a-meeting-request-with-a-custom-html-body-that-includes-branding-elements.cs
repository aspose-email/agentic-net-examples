using System;
using System.Net;
using Aspose.Email;
using Aspose.Email.Clients.Exchange.WebService;
using Aspose.Email.Calendar;

class Program
{
    static void Main()
    {
        try
        {
            // EWS client credentials
            string mailboxUri = "https://exchange.example.com/EWS/Exchange.asmx";
            string username = "user@example.com";
            string password = "password";


            // Skip external calls when placeholder credentials are used
            if (mailboxUri.Contains("example.com") || username.Contains("example.com") || password == "password")
            {
                Console.Error.WriteLine("Placeholder credentials detected. Skipping external calls.");
                return;
            }

            // Create and use the EWS client
            using (IEWSClient client = EWSClient.GetEWSClient(mailboxUri, username, password))
            {
                // Prepare attendees
                MailAddressCollection attendees = new MailAddressCollection();
                attendees.Add(new MailAddress("attendee1@example.com"));
                attendees.Add(new MailAddress("attendee2@example.com"));

                // Define appointment details
                DateTime start = new DateTime(2026, 5, 20, 10, 0, 0);
                DateTime end = new DateTime(2026, 5, 20, 11, 0, 0);
                MailAddress organizer = new MailAddress("organizer@example.com");

                Appointment appointment = new Appointment(
                    "Project Kickoff Meeting",          // subject
                    "Conference Room A",                // location
                    "Initial project kickoff discussion.", // description (plain text)
                    start,
                    end,
                    organizer,
                    attendees);

                // Custom HTML body with branding
                appointment.HtmlDescription = @"
<html>
<body>
    <h1 style='color:#2E86C1;'>Project Kickoff Meeting</h1>
    <p>Please join us for the kickoff.</p>
    <img src='https://example.com/logo.png' alt='Company Logo' style='width:150px;'/>
</body>
</html>";

                // Create the meeting request message
                using (MailMessage message = new MailMessage())
                {
                    message.From = organizer;
                    // Add at least one recipient to satisfy the MailMessage requirements
                    message.To.Add(new MailAddress("attendee1@example.com"));
                    message.Subject = "Invitation: " + appointment.Summary;

                    // Attach the appointment as an alternate view (meeting request)
                    message.AddAlternateView(appointment.RequestApointment());

                    // Send the meeting request via EWS
                    try
                    {
                        client.Send(message);
                        Console.WriteLine("Meeting request sent successfully.");
                    }
                    catch (Exception ex)
                    {
                        Console.Error.WriteLine("Error sending meeting request: " + ex.Message);
                        return;
                    }
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine("Unhandled exception: " + ex.Message);
        }
    }
}
