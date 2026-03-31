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
            // Initialize the EWS client with placeholder credentials.
            string serviceUrl = "https://exchange.example.com/EWS/Exchange.asmx";
            string username = "user@example.com";
            string password = "password";

            using (IEWSClient client = EWSClient.GetEWSClient(serviceUrl, username, password))
            {
                // -------------------- Create a calendar appointment --------------------
                Appointment newAppointment = new Appointment(
                    "Conference Room 1",
                    new DateTime(2023, 12, 25, 10, 0, 0),
                    new DateTime(2023, 12, 25, 11, 0, 0),
                    new MailAddress("organizer@example.com"),
                    new MailAddressCollection { new MailAddress("attendee1@example.com") }
                );
                newAppointment.Summary = "Christmas Planning Meeting";
                newAppointment.Description = "Discuss plans for the holiday season.";

                client.CreateAppointment(newAppointment);
                Console.WriteLine("Appointment created.");

                // -------------------- Retrieve the created appointment --------------------
                // The UniqueId property holds the identifier of the item on the server.
                string appointmentUri = newAppointment.UniqueId;
                Appointment fetchedAppointment = client.FetchAppointment(appointmentUri);
                Console.WriteLine($"Fetched appointment: {fetchedAppointment.Summary} at {fetchedAppointment.StartDate}");

                // -------------------- Update the appointment --------------------
                fetchedAppointment.Location = "Conference Room 2";
                client.UpdateAppointment(fetchedAppointment);
                Console.WriteLine("Appointment updated with new location.");

                // -------------------- Delete the appointment --------------------
                client.DeleteItem(appointmentUri, CalendarDeletionOptions.DeletePermanently);
                Console.WriteLine("Appointment deleted permanently.");
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
