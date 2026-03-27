using System;
using System.Net;
using Aspose.Email;
using Aspose.Email.Calendar;
using Aspose.Email.Clients.Exchange.WebService;
using Aspose.Email.Tools.Search;

class Program
{
    static void Main()
    {
        var credential = new System.Net.NetworkCredential("username", "password", "domain");

        try
        {
            // Initialize EWS client
            string mailboxUri = "https://exchange.example.com/EWS/Exchange.asmx";
            string username = "user@example.com";
            string password = "password";
            NetworkCredential credentials = new NetworkCredential(username, password);

            using (IEWSClient client = EWSClient.GetEWSClient(mailboxUri, credentials))
            {
                // Create a new appointment
                Appointment newAppointment = new Appointment(
                    "Room 101",
                    new DateTime(2023, 12, 1, 10, 0, 0),
                    new DateTime(2023, 12, 1, 11, 0, 0),
                    new MailAddress("organizer@example.com"),
                    new MailAddressCollection { new MailAddress("attendee1@example.com") }
                );
                newAppointment.Summary = "Project Meeting";
                newAppointment.Description = "Discuss project milestones.";

                // CreateAppointment returns the UID of the created item
                string appointmentUid = client.CreateAppointment(newAppointment);
                Console.WriteLine("Created appointment UID: " + appointmentUid);

                // Retrieve the appointment by UID
                Appointment fetchedAppointment = client.FetchAppointment(appointmentUid);
                Console.WriteLine("Fetched appointment subject: " + fetchedAppointment.Summary);

                // Update the appointment (e.g., change the subject)
                fetchedAppointment.Summary = "Updated Project Meeting";
                client.UpdateAppointment(fetchedAppointment);
                Console.WriteLine("Appointment updated.");

                // Delete the appointment using generic DeleteItem method
                client.DeleteItem(appointmentUid, DeletionOptions.DeletePermanently);
                Console.WriteLine("Appointment deleted.");
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine("Error: " + ex.Message);
        }
    }
}