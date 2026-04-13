using System;
using Aspose.Email;
using Aspose.Email.Clients.Exchange.WebService;
using Aspose.Email.Calendar;

class Program
{
    static void Main()
    {
        try
        {
            // Initialize EWS client
            string mailboxUri = "https://exchange.example.com/EWS/Exchange.asmx";
            string username = "username";
            string password = "password";


            // Skip external calls when placeholder credentials are used
            if (mailboxUri.Contains("example.com") || username == "username" || password == "password")
            {
                Console.Error.WriteLine("Placeholder credentials detected. Skipping external calls.");
                return;
            }

            using (IEWSClient client = EWSClient.GetEWSClient(mailboxUri, username, password))
            {
                // Folder URI for the calendar (default calendar folder)
                string folderUri = client.MailboxInfo.CalendarUri;

                // Unique identifier of the appointment to be updated
                string appointmentId = "AAMkAG..."; // replace with actual appointment UID

                // Create a minimal appointment instance with required fields
                MailAddressCollection attendees = new MailAddressCollection();
                Appointment appointment = new Appointment(
                    "Conference Room",
                    DateTime.Now.AddHours(1),
                    DateTime.Now.AddHours(2),
                    new MailAddress("organizer@example.com"),
                    attendees);

                // Set the unique ID so the server knows which item to update
                appointment.UniqueId = appointmentId;

                // Update the HTML body to include a hyperlink to the agenda document
                appointment.HtmlDescription = "<a href=\"https://example.com/agenda.pdf\">Meeting Agenda</a>";

                // Send the update to the server
                client.UpdateAppointment(appointment, folderUri);
                Console.WriteLine("Appointment updated successfully.");
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
