using System;
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
            string username = "username";
            string password = "password";

            // Skip external calls when placeholder credentials are used
            if (mailboxUri.Contains("example.com") || username == "username" || password == "password")
            {
                Console.Error.WriteLine("Placeholder credentials detected. Skipping external calls.");
                return;
            }

            using (IEWSClient client = EWSClient.GetEWSClient(mailboxUri, username, password))
            {
                // Retrieve all appointments from the default calendar folder
                Appointment[] appointments = client.ListAppointments();

                DateTime cutoffDate = DateTime.Now.AddMonths(-6);

                foreach (Appointment appointment in appointments)
                {
                    // Check if the appointment is cancelled and older than six months
                    bool isCancelled = appointment.Status == AppointmentStatus.Cancelled;
                    bool isOld = appointment.StartDate < cutoffDate;

                    if (isCancelled && isOld)
                    {
                        // Cancel the appointment on the server
                        client.CancelAppointment(appointment);
                        Console.WriteLine($"Cancelled appointment: {appointment.Summary} (Start: {appointment.StartDate})");
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
