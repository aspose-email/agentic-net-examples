using System;
using Aspose.Email;
using Aspose.Email.Calendar;
using Aspose.Email.Clients.Exchange.WebService;

class Program
{
    static void Main()
    {
        try
        {
            // Placeholder credentials – replace with real values to run
            string host = "https://ews.example.com/EWS/Exchange.asmx";
            string username = "your_username";
            string password = "your_password";

            if (host.Contains("example.com") || username.StartsWith("your_") || password.StartsWith("your_"))
            {
                Console.Error.WriteLine("Exchange client credentials are placeholders. Skipping execution.");
                return;
            }

            // Initialize Exchange Web Services client
            using (IEWSClient ewsClient = EWSClient.GetEWSClient(host, username, password))
            {
                // Identifier of the appointment to update
                string appointmentId = "existing_appointment_id";

                // Create an appointment instance with required fields
                MailAddress organizer = new MailAddress("organizer@example.com");
                MailAddressCollection attendees = new MailAddressCollection();
                attendees.Add(new MailAddress("attendee1@example.com"));
                DateTime start = DateTime.Now.AddDays(1);
                DateTime end = start.AddHours(1);
                Appointment appointment = new Appointment("Updated Meeting", start, end, organizer, attendees);

                // Set the unique identifier of the existing appointment
                appointment.UniqueId = appointmentId;

                // Modify the location (or other details) as needed
                appointment.Location = "New Conference Room";

                // Update the appointment on Exchange
                ewsClient.UpdateAppointment(appointment);
                Console.WriteLine("Appointment updated. New location: " + appointment.Location);
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine("Error: " + ex.Message);
        }
    }
}
