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
            // Create EWS client (replace with actual server URL and credentials)
            string mailboxUri = "https://exchange.example.com/EWS/Exchange.asmx";
            string username = "username";
            string password = "password";

            using (IEWSClient client = EWSClient.GetEWSClient(mailboxUri, username, password))
            {
                // Define the folder URI for the calendar (usually the default calendar folder)
                string calendarFolderUri = client.MailboxInfo.CalendarUri;

                // Existing appointment unique identifier (replace with actual UID)
                string existingAppointmentUid = "AAMkAD..."; // placeholder UID

                // Load the existing appointment (minimal representation)
                // For demonstration, create a new Appointment instance and set its UniqueId.
                // In a real scenario, you would fetch the appointment from the server.
                Appointment appointment = new Appointment(
                    "Meeting Subject",
                    DateTime.Now.AddHours(1),          // original start (will be updated)
                    DateTime.Now.AddHours(2),          // original end
                    new MailAddress("organizer@domain.com"),
                    new MailAddressCollection()
                );
                appointment.UniqueId = existingAppointmentUid;

                // Update the start time of the appointment
                appointment.StartDate = new DateTime(2023, 12, 25, 10, 0, 0);
                appointment.EndDate = new DateTime(2023, 12, 25, 11, 0, 0);

                // Add a new attendee
                appointment.Attendees.Add(new MailAddress("new.attendee@domain.com"));

                // Update the appointment on the server
                client.UpdateAppointment(appointment, calendarFolderUri);
                Console.WriteLine("Appointment updated successfully.");
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
