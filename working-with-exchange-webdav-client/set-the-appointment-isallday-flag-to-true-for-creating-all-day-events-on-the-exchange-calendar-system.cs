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
            // Placeholder credentials and service URL
            string serviceUrl = "https://exchange.example.com/EWS/Exchange.asmx";
            string username = "username";
            string password = "password";

            // Skip execution when placeholders are detected
            if (serviceUrl.Contains("example.com") || username == "username" || password == "password")
            {
                Console.Error.WriteLine("Placeholder Exchange credentials detected. Skipping execution.");
                return;
            }

            // Create the EWS client
            using (IEWSClient client = EWSClient.GetEWSClient(serviceUrl, username, password))
            {
                // Prepare attendees collection
                MailAddressCollection attendees = new MailAddressCollection();
                attendees.Add(new MailAddress("attendee1@domain.com"));
                attendees.Add(new MailAddress("attendee2@domain.com"));

                // Create an all‑day appointment (start at midnight, end next day midnight)
                DateTime startDate = new DateTime(2024, 5, 1);
                DateTime endDate = startDate.AddDays(1); // All‑day spans full day

                Appointment appointment = new Appointment(
                    location: "Conference Room",
                    startDate: startDate,
                    endDate: endDate,
                    organizer: new MailAddress("organizer@domain.com"),
                    attendees: attendees);

                // Set the appointment as an all‑day event using the Flags property
                appointment.Flags = AppointmentFlags.AllDayEvent;

                // Optionally set summary and description
                appointment.Summary = "Company Holiday";
                appointment.Description = "All staff holiday - office closed.";

                // Create the appointment on the Exchange server
                string uid = client.CreateAppointment(appointment);
                Console.WriteLine($"All‑day appointment created with UID: {uid}");
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
