using System;
using System.Net;
using Aspose.Email;
using Aspose.Email.Clients.Exchange.WebService;
using Aspose.Email.Calendar;
using Aspose.Email.Calendar.Recurrences;

class Program
{
    static void Main()
    {
        try
        {
            // Initialize EWS client
            string mailboxUri = "https://exchange.example.com/EWS/Exchange.asmx";

            // Skip external calls when placeholder credentials are used
            if (mailboxUri.Contains("example.com"))
            {
                Console.Error.WriteLine("Placeholder credentials detected. Skipping external calls.");
                return;
            }

            ICredentials credentials = new NetworkCredential("username", "password");
            using (IEWSClient client = EWSClient.GetEWSClient(mailboxUri, credentials))
            {
                // Folder containing the appointment (default calendar folder)
                string calendarFolderUri = client.CurrentCalendarFolderUri;

                // Identifier of the recurring appointment to update
                string appointmentId = "AAMkAGI2..."; // replace with actual appointment URI

                // Fetch the existing appointment
                Appointment appointment = client.FetchAppointment(appointmentId, calendarFolderUri);
                if (appointment == null)
                {
                    Console.Error.WriteLine("Appointment not found.");
                    return;
                }

                // New start and end times for the series
                DateTime newSeriesStart = new DateTime(2024, 5, 1, 9, 0, 0);
                DateTime newSeriesEnd   = new DateTime(2024, 5, 1, 10, 0, 0);

                // Update the series start/end times
                appointment.StartDate = newSeriesStart;
                appointment.EndDate   = newSeriesEnd;

                // Adjust recurrence pattern if present
                if (appointment.Recurrence is DailyRecurrencePattern dailyPattern)
                {
                    // Preserve existing exception dates (no changes needed)
                    // Update the recurrence end date if required
                    dailyPattern.EndDate = newSeriesEnd.AddMonths(1); // example adjustment
                }

                // Save the changes back to the server
                client.UpdateAppointment(appointment, calendarFolderUri);
                Console.WriteLine("Recurring appointment series updated successfully.");
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
