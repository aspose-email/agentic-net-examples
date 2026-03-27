using System;
using System.Net;
using Aspose.Email;
using Aspose.Email.Calendar;
using Aspose.Email.Clients.Exchange.WebService;
using Aspose.Email.Tools.Search;

public class Program
{
    public static void Main()
    {
        var credential = new System.Net.NetworkCredential("username", "password", "domain");

        try
        {
            // Replace with actual mailbox URI and credentials
            string mailboxUri = "https://exchange.example.com/EWS/Exchange.asmx";
            string username = "user@example.com";
            string password = "password";

            // Create the EWS client (service)
            using (IEWSClient service = EWSClient.GetEWSClient(mailboxUri, username, password))
            {
                // UID of the existing appointment to be updated
                string appointmentUid = "existing-appointment-uid";

                // Fetch the existing appointment
                Aspose.Email.Calendar.Appointment appointment = service.FetchAppointment(appointmentUid);

                // Update relevant fields
                appointment.Summary = "Updated Meeting Subject";
                appointment.Description = "Updated description of the meeting.";
                appointment.Location = "Conference Room B";
                appointment.StartDate = new DateTime(2024, 5, 20, 10, 0, 0);
                appointment.EndDate = new DateTime(2024, 5, 20, 11, 0, 0);

                // Update attendees
                Aspose.Email.MailAddressCollection attendees = new Aspose.Email.MailAddressCollection();
                attendees.Add(new Aspose.Email.MailAddress("alice@example.com"));
                attendees.Add(new Aspose.Email.MailAddress("bob@example.com"));
                appointment.Attendees = attendees;

                // Apply the updates to the server
                service.UpdateAppointment(appointment);

                Console.WriteLine("Appointment updated successfully.");
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}