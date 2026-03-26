using System;
using Aspose.Email;
using Aspose.Email.Clients.Google;
using Aspose.Email.Calendar;
using System.Collections.Generic;

namespace GmailCalendarSample
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                // Initialize Gmail client with placeholder credentials
                string clientId = "YOUR_CLIENT_ID";
                string clientSecret = "YOUR_CLIENT_SECRET";
                string refreshToken = "YOUR_REFRESH_TOKEN";
                string userEmail = "user@example.com";

                using (IGmailClient gmailClient = GmailClient.GetInstance(clientId, clientSecret, refreshToken, userEmail))
                {
                    // Ensure we have at least one calendar; create one if none exist
                    Calendar[] calendars = gmailClient.ListCalendars();
                    string calendarId;
                    if (calendars != null && calendars.Length > 0)
                    {
                        calendarId = calendars[0].Id;
                    }
                    else
                    {
                        Calendar newCalendar = new Calendar("Sample Calendar");
                        calendarId = gmailClient.CreateCalendar(newCalendar);
                    }

                    // Prepare attendees for the appointment
                    MailAddressCollection attendees = new MailAddressCollection();
                    attendees.Add(new MailAddress("attendee1@example.com"));
                    attendees.Add(new MailAddress("attendee2@example.com"));

                    // Create a new appointment
                    Appointment newAppointment = new Appointment(
                        "Team Sync",
                        new DateTime(2024, 5, 20, 10, 0, 0),
                        new DateTime(2024, 5, 20, 11, 0, 0),
                        new MailAddress(userEmail),
                        attendees);
                    newAppointment.Description = "Weekly team synchronization meeting.";
                    newAppointment.Location = "Conference Room A";

                    // Create the appointment in the selected calendar
                    Appointment createdAppointment = gmailClient.CreateAppointment(calendarId, newAppointment);
                    Console.WriteLine("Created Appointment ID: " + createdAppointment.UniqueId);

                    // Retrieve the appointment back from Google Calendar
                    Appointment fetchedAppointment = gmailClient.FetchAppointment(calendarId, createdAppointment.UniqueId);
                    Console.WriteLine("Fetched Appointment Summary: " + fetchedAppointment.Summary);
                    Console.WriteLine("Fetched Appointment Start: " + fetchedAppointment.StartDate);
                    Console.WriteLine("Fetched Appointment End: " + fetchedAppointment.EndDate);

                    // Modify the appointment (e.g., change the summary)
                    fetchedAppointment.Summary = "Team Sync - Updated";
                    Appointment updatedAppointment = gmailClient.UpdateAppointment(calendarId, fetchedAppointment);
                    Console.WriteLine("Updated Appointment Summary: " + updatedAppointment.Summary);
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine("Error: " + ex.Message);
                return;
            }
        }
    }
}