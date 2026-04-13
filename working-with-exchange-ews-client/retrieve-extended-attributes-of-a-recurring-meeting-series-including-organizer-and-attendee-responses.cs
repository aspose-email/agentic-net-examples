using System;
using System.Net;
using Aspose.Email;
using Aspose.Email.Calendar;
using Aspose.Email.Calendar.Recurrences;
using Aspose.Email.Clients.Exchange.WebService;

class Program
{
    static void Main(string[] args)
    {
        try
        {
            // Exchange server connection settings
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
                // Retrieve all appointments from the default calendar folder
                AppointmentCollection appointments = client.ListAppointments();

                foreach (Appointment appointment in appointments)
                {
                    // Process only recurring meetings
                    if (appointment.Recurrence != null)
                    {
                        Console.WriteLine("Subject: " + appointment.Summary);
                        Console.WriteLine("Organizer: " + (appointment.Organizer != null ? appointment.Organizer.Address : "N/A"));
                        Console.WriteLine("Start: " + appointment.StartDate);
                        Console.WriteLine("Recurrence Pattern Type: " + appointment.Recurrence.GetType().Name);

                        // List attendees
                        foreach (MailAddress attendee in appointment.Attendees)
                        {
                            Console.WriteLine("Attendee: " + attendee.Address);
                            // Aspose.Email does not expose attendee response status directly in Appointment
                        }

                        Console.WriteLine();
                    }
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine(ex.Message);
        }
    }
}
