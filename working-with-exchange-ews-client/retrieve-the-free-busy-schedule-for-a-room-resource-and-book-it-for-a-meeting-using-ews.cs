using System;
using System.Collections.Specialized;
using System.Net;
using Aspose.Email;
using Aspose.Email.Clients.Exchange.WebService;
using Aspose.Email.Calendar;

namespace AsposeEmailEwsFreeBusyBooking
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                // Define Exchange server URI and credentials
                string mailboxUri = "https://exchange.example.com/EWS/Exchange.asmx";

                // Skip external calls when placeholder credentials are used
                if (mailboxUri.Contains("example.com"))
                {
                    Console.Error.WriteLine("Placeholder credentials detected. Skipping external calls.");
                    return;
                }

                NetworkCredential credentials = new NetworkCredential("username", "password", "DOMAIN");

                // Create EWS client
                using (IEWSClient client = EWSClient.GetEWSClient(mailboxUri, credentials))
                {
                    try
                    {
                        // Define the room resource to check
                        StringCollection roomAddresses = new StringCollection();
                        roomAddresses.Add("room@example.com");

                        // Define the time window for the meeting
                        DateTime meetingStart = DateTime.Now.AddHours(1);
                        DateTime meetingEnd = meetingStart.AddHours(1);
                        DateRange meetingRange = new DateRange(meetingStart, meetingEnd);

                        // Retrieve free/busy information for the room
                        var availability = client.CheckUserAvailability(roomAddresses, meetingRange);
                        // (In a real scenario, inspect 'availability' to ensure the room is free)

                        // Prepare attendees collection (room as attendee)
                        MailAddressCollection attendees = new MailAddressCollection();
                        attendees.Add(new MailAddress("room@example.com"));

                        // Create the appointment
                        Appointment appointment = new Appointment(
                            "Project Sync Meeting",
                            meetingStart,
                            meetingEnd,
                            new MailAddress("organizer@example.com"),
                            attendees);
                                                appointment.Summary = "Meeting Summary";
appointment.Location = "Conference Room 1";
                        appointment.Description = "Discuss project status and next steps.";

                        // Book the meeting (create appointment on the server)
                        string appointmentUid = client.CreateAppointment(appointment);
                        Console.WriteLine("Appointment created successfully. UID: " + appointmentUid);
                    }
                    catch (Exception ex)
                    {
                        Console.Error.WriteLine("EWS operation failed: " + ex.Message);
                        return;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine("Unexpected error: " + ex.Message);
                return;
            }
        }
    }
}
