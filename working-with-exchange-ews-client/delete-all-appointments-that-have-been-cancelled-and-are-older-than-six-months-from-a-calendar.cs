using System;
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
            // Initialize EWS client
            string mailboxUri = "https://exchange.example.com/EWS/Exchange.asmx";

            // Skip external calls when placeholder credentials are used
            if (mailboxUri.Contains("example.com"))
            {
                Console.Error.WriteLine("Placeholder credentials detected. Skipping external calls.");
                return;
            }

            NetworkCredential credentials = new NetworkCredential("username", "password");
            using (IEWSClient client = EWSClient.GetEWSClient(mailboxUri, credentials))
            {
                // Get the default calendar folder URI
                string calendarFolderUri = client.CurrentCalendarFolderUri;

                // Retrieve all appointments from the calendar
                Appointment[] appointments = client.ListAppointments(calendarFolderUri, true);

                DateTime cutoffDate = DateTime.Now.AddMonths(-6);

                foreach (Appointment appointment in appointments)
                {
                    // Check if the appointment is cancelled and older than six months
                    if (appointment.Status == AppointmentStatus.Cancelled && appointment.StartDate < cutoffDate)
                    {
                        try
                        {
                            // Delete the cancelled appointment
                            client.DeleteItem(appointment.UniqueId, new DeletionOptions(DeletionType.MoveToDeletedItems));
                        }
                        catch (Exception ex)
                        {
                            Console.Error.WriteLine($"Failed to delete appointment {appointment.UniqueId}: {ex.Message}");
                        }
                    }
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
