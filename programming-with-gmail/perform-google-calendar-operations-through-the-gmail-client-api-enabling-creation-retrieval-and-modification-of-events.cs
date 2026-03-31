using System;
using Aspose.Email;
using Aspose.Email.Clients;
using Aspose.Email.Clients.Google;
using Aspose.Email.Calendar;

namespace GoogleCalendarSample
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                // Placeholder credentials
                string clientId = "clientId";
                string clientSecret = "clientSecret";
                string refreshToken = "refreshToken";
                string defaultEmail = "user@example.com";

                // Skip execution if placeholders are used
                if (clientId == "clientId" || clientSecret == "clientSecret" || refreshToken == "refreshToken")
                {
                    Console.WriteLine("Placeholder credentials detected. Skipping Google Calendar operations.");
                    return;
                }

                // Create Gmail client
                try
                {
                    using (IGmailClient gmailClient = GmailClient.GetInstance(clientId, clientSecret, refreshToken, defaultEmail))
                    {
                        // List calendars
                        Aspose.Email.Clients.Google.Calendar[] calendars = gmailClient.ListCalendars();
                        if (calendars == null || calendars.Length == 0)
                        {
                            Console.WriteLine("No calendars found.");
                            return;
                        }

                        // Use the first calendar
                        string calendarId = calendars[0].Id;

                        // Create attendees collection
                        MailAddressCollection attendees = new MailAddressCollection();
                        attendees.Add(new MailAddress("person1@example.com"));
                        attendees.Add(new MailAddress("person2@example.com"));

                        // Create an appointment
                        Appointment appointment = new Appointment(
                            "Team Meeting",
                            new DateTime(2024, 5, 1, 10, 0, 0),
                            new DateTime(2024, 5, 1, 11, 0, 0),
                            new MailAddress(defaultEmail),
                            attendees);
                        appointment.Location = "Conference Room";
                        appointment.Description = "Discuss project status.";

                        // Create the appointment in the calendar
                        Appointment createdAppointment = gmailClient.CreateAppointment(calendarId, appointment);
                        Console.WriteLine($"Created appointment with ID: {createdAppointment.UniqueId}");

                        // Retrieve appointments
                        Appointment[] appointments = gmailClient.ListAppointments(calendarId);
                        Console.WriteLine($"Total appointments in calendar: {appointments.Length}");

                        // Modify the appointment (e.g., change location)
                        createdAppointment.Location = "Updated Conference Room";
                        Appointment updatedAppointment = gmailClient.UpdateAppointment(calendarId, createdAppointment);
                        Console.WriteLine($"Updated appointment location to: {updatedAppointment.Location}");
                    }
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Error during Gmail client operations: {ex.Message}");
                    return;
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Unexpected error: {ex.Message}");
            }
        }
    }
}
