using System;
using Aspose.Email;
using Aspose.Email.Calendar;
using Aspose.Email.Clients;
using Aspose.Email.Clients.Google;

public class Program
{
    public static void Main()
    {
        try
        {
            // Initialize Gmail client with dummy credentials
            Aspose.Email.Clients.Google.IGmailClient gmailClient = GmailClient.GetInstance(
                "clientId",
                "clientSecret",
                "refreshToken",
                "user@example.com");

            using (gmailClient)
            {
                // Calendar and appointment identifiers
                string calendarId = "primary";
                string appointmentId = "appointmentId";

                // Fetch the existing appointment
                Aspose.Email.Calendar.Appointment appointment = gmailClient.FetchAppointment(calendarId, appointmentId);

                // Modify appointment details
                appointment.StartDate = new DateTime(2023, 12, 25, 10, 0, 0);
                appointment.EndDate = new DateTime(2023, 12, 25, 11, 0, 0);
                appointment.Location = "Conference Room B";

                // Add a new attendee
                appointment.Attendees.Add(new Aspose.Email.MailAddress("newattendee@example.com"));

                // Update the appointment on the server
                Aspose.Email.Calendar.Appointment updatedAppointment = gmailClient.UpdateAppointment(calendarId, appointment);

                Console.WriteLine("Appointment updated. New start time: " + updatedAppointment.StartDate);
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine("Error: " + ex.Message);
        }
    }
}