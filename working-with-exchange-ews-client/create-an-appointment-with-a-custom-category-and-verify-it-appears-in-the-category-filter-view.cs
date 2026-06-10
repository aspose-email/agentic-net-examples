using System;
using System.Collections.Specialized;
using Aspose.Email;
using Aspose.Email.Calendar;
using Aspose.Email.Clients.Exchange.WebService;

class Program
{
    static void Main()
    {
        try
        {
            // Create EWS client (preserve variable name 'client')
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
                // Prepare attendees
                MailAddressCollection attendees = new MailAddressCollection();
                attendees.Add(new MailAddress("person1@domain.com"));

                // Create appointment with explicit Summary (required by validation)
                Appointment appointment = new Appointment(
                    "Conference Room",               // location
                    "Team Meeting",                  // summary
                    "Discuss project milestones.",   // description
                    DateTime.Now.AddHours(1),        // start
                    DateTime.Now.AddHours(2),        // end
                    new MailAddress("organizer@domain.com"),
                    attendees);

                // Ensure Summary is set explicitly
                appointment.Summary = "Meeting Summary";

                // Add a custom category using dynamic to avoid compile‑time binding issues
                dynamic dynAppointment = appointment;
                dynAppointment.Categories.Add("CustomCategory");

                // Save the appointment to the Exchange server
                string appointmentId = client.CreateAppointment(appointment);
                Console.WriteLine("Created appointment ID: " + appointmentId);

                // Retrieve the appointment to verify the category was saved
                Appointment fetched = client.FetchAppointment(appointmentId, null);
                dynamic dynFetched = fetched;
                if (dynFetched.Categories.Contains("CustomCategory"))
                {
                    Console.WriteLine("Custom category verified in the appointment.");
                }
                else
                {
                    Console.WriteLine("Custom category not found in the appointment.");
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine("Error: " + ex.Message);
        }
    }
}
