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
            // Exchange server connection details
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
                // Required attendees
                MailAddressCollection requiredAttendees = new MailAddressCollection();
                requiredAttendees.Add(new MailAddress("required1@example.com"));
                requiredAttendees.Add(new MailAddress("required2@example.com"));

                // Optional attendees
                MailAddressCollection optionalAttendees = new MailAddressCollection();
                optionalAttendees.Add(new MailAddress("optional1@example.com"));
                optionalAttendees.Add(new MailAddress("optional2@example.com"));

                // Create the appointment
                Appointment appointment = new Appointment(
                    "Conference Room",
                    "Project Kickoff",
                    "Discuss project scope. Please respond within 48 hours.",
                    DateTime.Now.AddDays(1).AddHours(9),
                    DateTime.Now.AddDays(1).AddHours(10),
                    new MailAddress(username),
                    requiredAttendees);

                                appointment.Summary = "Meeting Summary";
// Add optional attendees
                foreach (MailAddress opt in optionalAttendees)
                {
                    appointment.OptionalAttendees.Add(opt);
                }

                // Create the appointment on the server (invitations are sent automatically)
                string uid = client.CreateAppointment(appointment);
                Console.WriteLine("Appointment created with UID: " + uid);
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine("Error: " + ex.Message);
        }
    }
}
