using System;
using System.Collections.Generic;
using System.Net;
using Aspose.Email;
using Aspose.Email.Calendar;
using Aspose.Email.Clients.Exchange.WebService;

class Program
{
    static void Main()
    {
        try
        {
            // Placeholder credentials – in real scenarios replace with actual values.
            string mailboxUri = "https://example.com/EWS/Exchange.asmx";
            string username = "user";
            string password = "password";

            // Guard against placeholder credentials to avoid live network calls.
            if (mailboxUri.Contains("example.com") || username == "user" && password == "password")
            {
                Console.Error.WriteLine("Placeholder credentials detected. Skipping network operations.");
                return;
            }

            // Create the EWS client.
            using (IEWSClient client = EWSClient.GetEWSClient(mailboxUri, username, password))
            {
                // Define the new appointment details.
                MailAddress organizer = new MailAddress("organizer@example.com");
                MailAddressCollection attendees = new MailAddressCollection
                {
                    new MailAddress("attendee1@example.com"),
                    new MailAddress("attendee2@example.com")
                };

                DateTime newStart = new DateTime(2024, 12, 15, 10, 0, 0);
                DateTime newEnd = new DateTime(2024, 12, 15, 11, 0, 0);

                Appointment newAppointment = new Appointment(
                    location: "Conference Room A",
                    summary: "Project Sync",
                    description: "Discuss project milestones.",
                    startDate: newStart,
                    endDate: newEnd,
                    organizer: organizer,
                    attendees: attendees);

                // Retrieve existing appointments.
                Appointment[] existingAppointments = client.ListAppointments();

                // Check for overlapping appointments.
                bool hasConflict = false;
                foreach (Appointment existing in existingAppointments)
                {
                    // Ensure existing appointment has valid dates.
                    if (existing.StartDate < newEnd && existing.EndDate > newStart)
                    {
                        hasConflict = true;
                        Console.WriteLine($"Conflict detected with appointment: {existing.Summary} ({existing.StartDate} - {existing.EndDate})");
                        break;
                    }
                }

                if (hasConflict)
                {
                    Console.WriteLine("New appointment conflicts with an existing one. Creation aborted.");
                }
                else
                {
                    // No conflict – create the appointment on the server.
                    string uid = client.CreateAppointment(newAppointment);
                    Console.WriteLine($"Appointment created successfully. UID: {uid}");
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
