using Aspose.Email.Calendar;
using System;
using System.Collections.Generic;
using Aspose.Email;
using Aspose.Email.Clients.Google;

class Program
{
    static void Main()
    {
        try
        {
            // Placeholder OAuth credentials – replace with real values.
            string clientId = "YOUR_CLIENT_ID";
            string clientSecret = "YOUR_CLIENT_SECRET";
            string refreshToken = "YOUR_REFRESH_TOKEN";
            string defaultEmail = "user@example.com";

            // Guard against placeholder credentials to avoid real network calls.
            if (clientId.StartsWith("YOUR_") || clientSecret.StartsWith("YOUR_") || refreshToken.StartsWith("YOUR_"))
            {
                Console.Error.WriteLine("Placeholder credentials detected – skipping Gmail operations.");
                return;
            }

            // Initialize Gmail client.
            using (IGmailClient gmailClient = GmailClient.GetInstance(clientId, clientSecret, refreshToken, defaultEmail))
            {
                // Placeholder identifiers – replace with actual IDs.
                string calendarId = "primary";
                string appointmentId = "YOUR_APPOINTMENT_ID";

                // Guard against placeholder appointment ID.
                if (appointmentId.StartsWith("YOUR_"))
                {
                    Console.Error.WriteLine("Placeholder appointment ID – cannot fetch appointment.");
                    return;
                }

                // Fetch the existing appointment.
                Appointment appointment = gmailClient.FetchAppointment(calendarId, appointmentId);
                if (appointment == null)
                {
                    Console.Error.WriteLine("Failed to fetch appointment.");
                    return;
                }

                // Modify appointment properties.
                appointment.Summary = "Updated Meeting Subject";
                appointment.Description = "Updated description for the meeting.";
                appointment.StartDate = new DateTime(2026, 4, 15, 10, 0, 0);
                appointment.EndDate = new DateTime(2026, 4, 15, 11, 0, 0);

                // Update attendees.
                MailAddressCollection attendees = new MailAddressCollection
                {
                    new MailAddress("alice@example.com"),
                    new MailAddress("bob@example.com")
                };
                appointment.Attendees = attendees;

                // Update the appointment on the server.
                Appointment updated = gmailClient.UpdateAppointment(calendarId, appointment);
                if (updated != null)
                {
                    Console.WriteLine("Appointment updated successfully.");
                }
                else
                {
                    Console.Error.WriteLine("Failed to update appointment.");
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
