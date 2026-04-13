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
            // EWS connection parameters
            string mailboxUri = "https://exchange.example.com/EWS/Exchange.asmx";
            string username = "user@example.com";
            string password = "password";


            // Skip external calls when placeholder credentials are used
            if (mailboxUri.Contains("example.com") || username.Contains("example.com") || password == "password")
            {
                Console.Error.WriteLine("Placeholder credentials detected. Skipping external calls.");
                return;
            }

            // Create EWS client with safety wrapper
            try
            {
                using (IEWSClient client = EWSClient.GetEWSClient(mailboxUri, username, password))
                {
                    // Prepare attendees
                    MailAddressCollection attendees = new MailAddressCollection();
                    attendees.Add(new MailAddress("attendee1@example.com"));

                    // Define appointment times
                    DateTime start = DateTime.Now.AddHours(1);
                    DateTime end = start.AddHours(2);

                    // Create appointment
                    Appointment appointment = new Appointment(
                        "Conference Room",
                        "Team Meeting",
                        "Discuss project status",
                        start,
                        end,
                        new MailAddress(username),
                        attendees);

                    // Add a custom reminder 30 minutes before the start time
                    AppointmentReminder customReminder = new AppointmentReminder();
                    customReminder.Trigger = new ReminderTrigger(start.AddMinutes(-30));
                    customReminder.Summary = "Reminder: Team Meeting";
                    appointment.Reminders.Add(customReminder);

                    // Create the appointment on the Exchange server
                    string uid = client.CreateAppointment(appointment);
                    Console.WriteLine("Created appointment UID: " + uid);

                    // Fetch the appointment back to verify the reminder
                    Appointment fetched = client.FetchAppointment(uid);
                    if (fetched != null && fetched.Reminders.Count > 0)
                    {
                        Console.WriteLine("Fetched reminder trigger: " + fetched.Reminders[0].Trigger);
                    }
                    else
                    {
                        Console.WriteLine("No reminders found on fetched appointment.");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine("EWS client error: " + ex.Message);
                return;
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine("Unexpected error: " + ex.Message);
        }
    }
}
