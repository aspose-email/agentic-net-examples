using System;
using Aspose.Email;
using Aspose.Email.Clients.Google;
using Aspose.Email.Calendar;

namespace GmailCalendarSample
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                // Replace the placeholders with actual credentials.
                string clientId = "YOUR_CLIENT_ID";
                string clientSecret = "YOUR_CLIENT_SECRET";
                string refreshToken = "YOUR_REFRESH_TOKEN";
                string defaultEmail = "user@example.com";

                // Create the Gmail client.
                IGmailClient gmailClient = GmailClient.GetInstance(clientId, clientSecret, refreshToken, defaultEmail);
                using (gmailClient)
                {
                    // -------------------- Create a new calendar --------------------
                    Aspose.Email.Clients.Google.Calendar newCalendar = new Aspose.Email.Clients.Google.Calendar("Sample Calendar");
                    string calendarId = gmailClient.CreateCalendar(newCalendar);
                    Console.WriteLine("Created calendar with ID: " + calendarId);

                    // -------------------- Create an appointment --------------------
                    MailAddress organizer = new MailAddress(defaultEmail);
                    MailAddressCollection attendees = new MailAddressCollection();
                    Appointment appointment = new Appointment(
                        "Team Meeting",
                        DateTime.Now.AddHours(1),
                        DateTime.Now.AddHours(2),
                        organizer,
                        attendees);
                    appointment.Summary = "Team Meeting";
                    appointment.Description = "Discuss project status.";

                    Appointment createdAppointment = gmailClient.CreateAppointment(calendarId, appointment);
                    Console.WriteLine("Created appointment with ID: " + createdAppointment.UniqueId);

                    // -------------------- List appointments --------------------
                    Appointment[] appointments = gmailClient.ListAppointments(calendarId);
                    Console.WriteLine("Appointments in calendar:");
                    foreach (Appointment appt in appointments)
                    {
                        Console.WriteLine(" - ID: " + appt.UniqueId + ", Summary: " + appt.Summary);
                    }

                    // -------------------- Update the appointment --------------------
                    string appointmentId = createdAppointment.UniqueId;
                    createdAppointment.Summary = "Updated Team Meeting";
                    Appointment updatedAppointment = gmailClient.UpdateAppointment(calendarId, createdAppointment);
                    Console.WriteLine("Updated appointment ID: " + updatedAppointment.UniqueId + ", New Summary: " + updatedAppointment.Summary);

                    // -------------------- Delete the appointment --------------------
                    gmailClient.DeleteAppointment(calendarId, appointmentId);
                    Console.WriteLine("Deleted appointment with ID: " + appointmentId);

                    // -------------------- Delete the calendar --------------------
                    gmailClient.DeleteCalendar(calendarId);
                    Console.WriteLine("Deleted calendar with ID: " + calendarId);
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine("Error: " + ex.Message);
            }
        }
    }
}