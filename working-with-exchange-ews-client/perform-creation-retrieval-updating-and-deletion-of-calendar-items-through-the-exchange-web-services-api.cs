using System;
using System.Net;
using Aspose.Email;
using Aspose.Email.Clients.Exchange.WebService;
using Aspose.Email.Calendar;

namespace AsposeEmailEwsCalendarSample
{
    class Program
    {
        static void Main()
        {
            try
            {
                // Initialize the EWS client
                IEWSClient client = null;
                try
                {
                    NetworkCredential credential = new NetworkCredential("username", "password");
                    client = EWSClient.GetEWSClient("https://exchange.example.com/EWS/Exchange.asmx", credential);
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine("Failed to create EWS client: " + ex.Message);
                    return;
                }

                using (client)
                {
                    // Create a new appointment
                    MailAddress organizer = new MailAddress("organizer@example.com");
                    MailAddressCollection attendees = new MailAddressCollection();
                    attendees.Add(new MailAddress("attendee1@example.com"));
                    attendees.Add(new MailAddress("attendee2@example.com"));

                    Appointment newAppointment = new Appointment(
                        "Conference Room",
                        DateTime.Now.AddHours(1),
                        DateTime.Now.AddHours(2),
                        organizer,
                        attendees);
                    newAppointment.Summary = "Project Meeting";
                    newAppointment.Description = "Discuss project milestones.";

                    try
                    {
                        client.CreateAppointment(newAppointment);
                        Console.WriteLine("Appointment created successfully.");
                    }
                    catch (Exception ex)
                    {
                        Console.Error.WriteLine("Error creating appointment: " + ex.Message);
                    }

                    // List appointments in the default calendar folder
                    try
                    {
                        Appointment[] appointments = client.ListAppointments(client.MailboxInfo.CalendarUri);
                        Console.WriteLine("Current appointments in the calendar:");
                        foreach (Appointment appt in appointments)
                        {
                            Console.WriteLine($"Subject: {appt.Summary}, Start: {appt.StartDate}, Id: {appt.UniqueId}");
                        }

                        if (appointments.Length > 0)
                        {
                            // Retrieve the first appointment
                            string appointmentId = appointments[0].UniqueId;
                            Appointment fetchedAppointment = client.FetchAppointment(appointmentId);

                            // Update the appointment's subject
                            fetchedAppointment.Summary = fetchedAppointment.Summary + " (Updated)";
                            client.UpdateAppointment(fetchedAppointment);
                            Console.WriteLine("Appointment updated successfully.");

                            // Delete the appointment
                            client.DeleteItem(appointmentId, DeletionOptions.DeletePermanently);
                            Console.WriteLine("Appointment deleted successfully.");
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.Error.WriteLine("Error processing appointments: " + ex.Message);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine("Unhandled exception: " + ex.Message);
            }
        }
    }
}
