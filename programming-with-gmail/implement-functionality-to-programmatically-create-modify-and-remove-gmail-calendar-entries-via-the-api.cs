using System;
using Aspose.Email;
using Aspose.Email.Calendar;
using Aspose.Email.Clients;
using Aspose.Email.Clients.Google;

class Program
{
    static void Main()
    {
        try
        {
            // Initialize Gmail client with dummy OAuth credentials
            IGmailClient gmailClient = GmailClient.GetInstance(
                "clientId",
                "clientSecret",
                "refreshToken",
                "user@example.com");

            // Use the client within a using block if it implements IDisposable
            using (gmailClient as IDisposable ?? new DummyDisposable())
            {
                // ---------- Create a new calendar ----------
                Calendar newCalendar = new Calendar("Sample Calendar");
                string calendarId = gmailClient.CreateCalendar(newCalendar);
                Console.WriteLine("Created calendar with ID: " + calendarId);

                // ---------- Prepare appointment details ----------
                MailAddressCollection attendees = new MailAddressCollection();
                attendees.Add(new MailAddress("person1@domain.com"));
                attendees.Add(new MailAddress("person2@domain.com"));

                MailAddress organizer = new MailAddress("organizer@example.com");
                DateTime start = DateTime.Now.AddDays(1).AddHours(10);
                DateTime end = start.AddHours(1);

                Appointment appointment = new Appointment(
                    "Conference Room",
                    start,
                    end,
                    organizer,
                    attendees);
                appointment.Summary = "Project Kickoff";
                appointment.Description = "Discuss project goals and milestones.";

                // ---------- Create the appointment ----------
                Appointment createdAppointment = gmailClient.CreateAppointment(calendarId, appointment);
                Console.WriteLine("Created appointment with UniqueId: " + createdAppointment.UniqueId);

                // ---------- Update the appointment ----------
                createdAppointment.Summary = "Updated Project Kickoff";
                Appointment updatedAppointment = gmailClient.UpdateAppointment(calendarId, createdAppointment);
                Console.WriteLine("Updated appointment summary to: " + updatedAppointment.Summary);

                // ---------- Delete the appointment ----------
                string appointmentId = updatedAppointment.UniqueId;
                gmailClient.DeleteAppointment(calendarId, appointmentId);
                Console.WriteLine("Deleted appointment with ID: " + appointmentId);

                // ---------- Delete the calendar ----------
                gmailClient.DeleteCalendar(calendarId);
                Console.WriteLine("Deleted calendar with ID: " + calendarId);
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine("Error: " + ex.Message);
        }
    }

    // Helper class to allow using pattern when the client does not implement IDisposable
    private class DummyDisposable : IDisposable
    {
        public void Dispose() { }
    }
}
