using System;
using System.IO;
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
            // EWS client configuration
            string mailboxUri = "https://exchange.example.com/EWS/Exchange.asmx";
            string username = "user@example.com";
            string password = "password";


            // Skip external calls when placeholder credentials are used
            if (mailboxUri.Contains("example.com") || username.Contains("example.com") || password == "password")
            {
                Console.Error.WriteLine("Placeholder credentials detected. Skipping external calls.");
                return;
            }

            // Create the EWS client
            using (IEWSClient client = EWSClient.GetEWSClient(mailboxUri, username, password))
            {
                // Prepare attendees
                MailAddressCollection attendees = new MailAddressCollection();
                attendees.Add(new MailAddress("person1@external.com"));
                attendees.Add(new MailAddress("person2@external.com"));

                // Create the appointment
                DateTime start = new DateTime(2026, 5, 20, 10, 0, 0);
                DateTime end = start.AddHours(2);
                MailAddress organizer = new MailAddress("organizer@example.com");
                Appointment appointment = new Appointment(
                    "Project Kickoff",
                    "Discuss project goals and timelines.",
                    "Conference Room A",
                    start,
                    end,
                    organizer,
                    attendees);

                // Path for the iCalendar file
                string icsPath = Path.Combine(Environment.CurrentDirectory, "meeting.ics");

                // Ensure the directory exists
                try
                {
                    string dir = Path.GetDirectoryName(icsPath);
                    if (!string.IsNullOrEmpty(dir) && !Directory.Exists(dir))
                    {
                        Directory.CreateDirectory(dir);
                    }

                    // Save the appointment as iCalendar
                    appointment.Save(icsPath);
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to create iCalendar file: {ex.Message}");
                    return;
                }

                // Convert appointment to a MailMessage (meeting request)
                MailMessage message = appointment.ToMailMessage();

                // Attach the iCalendar file
                try
                {
                    if (!File.Exists(icsPath))
                    {
                        Console.Error.WriteLine("iCalendar file was not found.");
                        return;
                    }

                    using (FileStream icsStream = File.OpenRead(icsPath))
                    {
                        Attachment calendarAttachment = new Attachment(icsStream, "meeting.ics", "text/calendar");
                        message.Attachments.Add(calendarAttachment);
                    }
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to attach iCalendar file: {ex.Message}");
                    return;
                }

                // Set sender and recipients explicitly
                message.From = organizer;
                foreach (MailAddress addr in attendees)
                {
                    message.To.Add(addr);
                }

                // Send the meeting request via EWS
                try
                {
                    client.Send(message);
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to send meeting request: {ex.Message}");
                    return;
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
