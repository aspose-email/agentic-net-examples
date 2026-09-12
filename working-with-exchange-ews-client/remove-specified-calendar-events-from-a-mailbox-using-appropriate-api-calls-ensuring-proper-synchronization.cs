using Aspose.Email.Clients.Exchange.WebService;
using System;
using Aspose.Email;
using Aspose.Email.Clients.Exchange;
using Aspose.Email.Calendar;

class Program
{
    static void Main()
    {
        try
        {
            // Exchange server connection settings (replace with your actual values)
            string serviceUrl = "YOUR_EWS_URL";
            string username = "YOUR_USERNAME";
            string password = "YOUR_PASSWORD";
            string domain = "YOUR_DOMAIN";

            // Initialize Exchange client
            IEWSClient client = EWSClient.GetEWSClient(serviceUrl, username, password, domain);

            // List existing appointments in the default Calendar folder (optional, for verification)
            Appointment[] existingAppointments = client.ListAppointments("Calendar");
            Console.WriteLine($"Found {existingAppointments.Length} appointments in the Calendar folder.");

            // IDs of appointments to delete (replace with actual appointment UIDs)
            string[] appointmentIdsToDelete = new string[] { "YOUR_APPOINTMENT_ID1", "YOUR_APPOINTMENT_ID2" };

            // Cancel specified appointments
            foreach (string appointmentId in appointmentIdsToDelete)
            {
                try
                {
                    // Cancel the appointment with a cancellation message
                    client.CancelAppointment(appointmentId, "Cancelled by automated process.");
                    Console.WriteLine($"Cancelled appointment '{appointmentId}'.");
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to cancel appointment '{appointmentId}': {ex.Message}");
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
