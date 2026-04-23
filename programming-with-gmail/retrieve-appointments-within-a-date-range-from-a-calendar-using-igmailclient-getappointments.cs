using System;
using Aspose.Email;
using Aspose.Email.Clients.Google;
using Aspose.Email.Calendar;

namespace GmailAppointmentsSample
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                // Placeholder credentials – replace with real values before running.
                string accessToken = "YOUR_ACCESS_TOKEN";
                string defaultEmail = "user@example.com";

                // Guard against placeholder credentials to avoid live network calls.
                if (accessToken == "YOUR_ACCESS_TOKEN")
                {
                    Console.Error.WriteLine("Access token not provided. Update the placeholder with a valid OAuth 2.0 token.");
                    return;
                }

                // Calendar identifier – "primary" refers to the default calendar.
                string calendarId = "primary";

                // Define the date range for filtering appointments.
                DateTime rangeStart = new DateTime(2023, 1, 1);
                DateTime rangeEnd   = new DateTime(2023, 12, 31);

                // Create the Gmail client instance.
                using (IGmailClient gmailClient = GmailClient.GetInstance(accessToken, defaultEmail))
                {
                    try
                    {
                        // Retrieve all appointments from the specified calendar.
                        Appointment[] allAppointments = gmailClient.ListAppointments(calendarId);

                        // Filter appointments that fall within the desired date range.
                        foreach (Appointment appointment in allAppointments)
                        {
                            if (appointment.StartDate >= rangeStart && appointment.EndDate <= rangeEnd)
                            {
                                Console.WriteLine("Subject: {0}", appointment.Summary);
                                Console.WriteLine("Start : {0}", appointment.StartDate);
                                Console.WriteLine("End   : {0}", appointment.EndDate);
                                Console.WriteLine("Location: {0}", appointment.Location);
                                Console.WriteLine(new string('-', 40));
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.Error.WriteLine("Error while retrieving appointments: " + ex.Message);
                        return;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine("Unexpected error: " + ex.Message);
            }
        }
    }
}
