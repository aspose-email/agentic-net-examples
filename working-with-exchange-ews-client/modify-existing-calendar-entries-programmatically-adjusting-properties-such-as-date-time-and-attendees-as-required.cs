using System;
using Aspose.Email;
using Aspose.Email.Clients;
using Aspose.Email.Clients.Google;
using Aspose.Email.Calendar;

namespace AsposeEmailSample
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                // Initialize Gmail client with placeholder credentials
                try
                {
                    IGmailClient gmailClient = GmailClient.GetInstance(
                        "clientId",
                        "clientSecret",
                        "refreshToken",
                        "user@example.com");

                    using (gmailClient)
                    {
                        // Identify the calendar and the appointment to modify
                        string calendarId = "primary"; // typically "primary" for the main calendar
                        string appointmentId = "appointmentId"; // replace with actual appointment ID

                        // Fetch the existing appointment
                        Appointment appointment = gmailClient.FetchAppointment(calendarId, appointmentId);

                        // Modify appointment details
                        appointment.StartDate = new DateTime(2023, 12, 25, 10, 0, 0);
                        appointment.EndDate = new DateTime(2023, 12, 25, 11, 0, 0);
                        appointment.Summary = "Updated Meeting Summary";
                        appointment.Description = "Updated description for the meeting.";

                        // Update attendees
                        MailAddressCollection newAttendees = new MailAddressCollection();
                        newAttendees.Add(new MailAddress("newperson@example.com"));
                        appointment.Attendees = newAttendees;

                        // Send the updated appointment back to the server
                        Appointment updatedAppointment = gmailClient.UpdateAppointment(calendarId, appointment);
                        Console.WriteLine("Appointment updated successfully. New Summary: " + updatedAppointment.Summary);
                    }
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine("Error during Gmail client operation: " + ex.Message);
                    return;
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine("Unexpected error: " + ex.Message);
            }
        }
    }
}
