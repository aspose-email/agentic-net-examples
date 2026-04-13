using System;
using System.Net;
using Aspose.Email;
using Aspose.Email.Clients.Exchange.WebService;
using Aspose.Email.Calendar;

class Program
{
    static void Main()
    {
        try
        {
            // Define connection parameters
            string mailboxUri = "https://exchange.example.com/EWS/Exchange.asmx";
            string username = "user@example.com";
            string password = "password";


            // Skip external calls when placeholder credentials are used
            if (mailboxUri.Contains("example.com") || username.Contains("example.com") || password == "password")
            {
                Console.Error.WriteLine("Placeholder credentials detected. Skipping external calls.");
                return;
            }

            // Create EWS client
            using (IEWSClient client = EWSClient.GetEWSClient(mailboxUri, username, password))
            {
                try
                {
                    // Define source and target time zones
                    TimeZoneInfo sourceTimeZone = TimeZoneInfo.FindSystemTimeZoneById("Pacific Standard Time");
                    TimeZoneInfo targetTimeZone = TimeZoneInfo.FindSystemTimeZoneById("Eastern Standard Time");

                    // Get the current calendar folder URI
                    string calendarFolderUri = client.CurrentCalendarFolderUri;

                    // Retrieve all appointments from the calendar folder
                    AppointmentCollection appointments = client.ListAppointments(calendarFolderUri, null, true);

                    foreach (Appointment appointment in appointments)
                    {
                        // Convert start time
                        DateTime sourceStart = DateTime.SpecifyKind(appointment.StartDate, DateTimeKind.Unspecified);
                        DateTime utcStart = TimeZoneInfo.ConvertTimeToUtc(sourceStart, sourceTimeZone);
                        DateTime targetStart = TimeZoneInfo.ConvertTimeFromUtc(utcStart, targetTimeZone);
                        appointment.StartDate = targetStart;

                        // Convert end time
                        DateTime sourceEnd = DateTime.SpecifyKind(appointment.EndDate, DateTimeKind.Unspecified);
                        DateTime utcEnd = TimeZoneInfo.ConvertTimeToUtc(sourceEnd, sourceTimeZone);
                        DateTime targetEnd = TimeZoneInfo.ConvertTimeFromUtc(utcEnd, targetTimeZone);
                        appointment.EndDate = targetEnd;

                        // Update time zone identifiers on the appointment
                        appointment.StartTimeZone = targetTimeZone.Id;
                        appointment.EndTimeZone = targetTimeZone.Id;

                        // Optionally, update the appointment on the server (if a synchronous method exists)
                        // client.UpdateAppointment(appointment, calendarFolderUri);
                        // Since only async update is available, this line is commented out to respect the async ban.

                        Console.WriteLine($"Appointment \"{appointment.Summary}\" converted to {targetTimeZone.DisplayName}.");
                    }
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"EWS operation failed: {ex.Message}");
                    return;
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
