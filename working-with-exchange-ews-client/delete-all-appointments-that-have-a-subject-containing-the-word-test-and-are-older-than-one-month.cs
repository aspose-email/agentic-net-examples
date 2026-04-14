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
            string mailboxUri = "https://mail.example.com/EWS/Exchange.asmx";

            // Skip external calls when placeholder credentials are used
            if (mailboxUri.Contains("example.com"))
            {
                Console.Error.WriteLine("Placeholder credentials detected. Skipping external calls.");
                return;
            }

            NetworkCredential credentials = new NetworkCredential("username", "password");

            using (IEWSClient client = EWSClient.GetEWSClient(mailboxUri, credentials))
            {
                // Retrieve all appointments from the default calendar folder
                Appointment[] appointments = client.ListAppointments();

                DateTime cutoffDate = DateTime.Now.AddMonths(-1);

                foreach (Appointment appointment in appointments)
                {
                    // Filter by subject (Summary) containing "Test" and older than one month
                    if (appointment.Summary != null &&
                        appointment.Summary.IndexOf("Test", StringComparison.OrdinalIgnoreCase) >= 0 &&
                        appointment.StartDate < cutoffDate)
                    {
                        try
                        {
                            // Delete the appointment
                            client.CancelAppointment(appointment);
                            Console.WriteLine($"Deleted appointment: {appointment.Summary}");
                        }
                        catch (Exception ex)
                        {
                            Console.Error.WriteLine($"Failed to delete appointment '{appointment.Summary}': {ex.Message}");
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
