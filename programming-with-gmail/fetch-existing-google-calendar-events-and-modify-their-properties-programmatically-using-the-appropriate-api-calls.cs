using System;
using Aspose.Email;
using Aspose.Email.Calendar;
using Aspose.Email.Clients.Google;

namespace AsposeEmailExample
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                // Replace with valid OAuth 2.0 access token and default email address.
                string accessToken = "YOUR_ACCESS_TOKEN";
                string defaultEmail = "user@example.com";

                // Create Gmail client instance.
                using (IGmailClient gmailClient = GmailClient.GetInstance(accessToken, defaultEmail))
                {
                    // Retrieve all calendars.
                    Calendar[] calendars = gmailClient.ListCalendars();

                    foreach (Calendar calendar in calendars)
                    {
                        // Retrieve all appointments for the current calendar.
                        Appointment[] appointments = gmailClient.ListAppointments(calendar.Id);

                        foreach (Appointment appointment in appointments)
                        {
                            // Modify the appointment's summary.
                            appointment.Summary = appointment.Summary + " - Updated";

                            // Update the appointment on Google Calendar.
                            gmailClient.UpdateAppointment(calendar.Id, appointment);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine(ex.Message);
            }
        }
    }
}