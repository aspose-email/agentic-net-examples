using System;
using System.IO;
using System.Net;
using Aspose.Email;
using Aspose.Email.Clients.Exchange.WebService;
using Aspose.Email.Calendar;

namespace MeetingRequestExample
{
    class Program
    {
        static void Main()
        {
            try
            {
                // Create EWS client
                IEWSClient client = null;
                try
                {
                    string mailboxUri = "https://exchange.example.com/EWS/Exchange.asmx";
                    string username = "username";
                    string password = "password";

                    client = EWSClient.GetEWSClient(mailboxUri, username, password);
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to create EWS client: {ex.Message}");
                    return;
                }

                // Prepare attendees
                MailAddressCollection attendees = new MailAddressCollection();
                attendees.Add(new MailAddress("attendee1@example.com"));
                attendees.Add(new MailAddress("attendee2@example.com"));

                // Create appointment
                Appointment appointment = new Appointment(
                    "Team Meeting",
                    new DateTime(2023, 12, 15, 10, 0, 0),
                    new DateTime(2023, 12, 15, 11, 0, 0),
                    new MailAddress("organizer@example.com"),
                    attendees);

                appointment.Location = "Conference Room A";
                appointment.Summary = "Quarterly Review";

                // Custom HTML body with branding and map placeholder
                appointment.HtmlDescription = "<html><body>" +
                                              "<h1 style='color:#2E86C1;'>Company Quarterly Review</h1>" +
                                              "<p>Please join the meeting at the designated time.</p>" +
                                              "<img src='cid:MapImage' alt='Map' />" +
                                              "</body></html>";

                // Ensure map image exists (create minimal placeholder if missing)
                string mapImagePath = "map.png";
                if (!File.Exists(mapImagePath))
                {
                    try
                    {
                        using (FileStream fs = File.Create(mapImagePath))
                        {
                            // Minimal PNG header (placeholder image)
                            byte[] pngHeader = new byte[] { 0x89, 0x50, 0x4E, 0x47, 0x0D, 0x0A, 0x1A, 0x0A };
                            fs.Write(pngHeader, 0, pngHeader.Length);
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.Error.WriteLine($"Failed to create placeholder map image: {ex.Message}");
                        client?.Dispose();
                        return;
                    }
                }

                // Attach map image with Content-ID matching the HTML reference
                try
                {
                    Attachment mapAttachment = new Attachment(mapImagePath);
                    mapAttachment.ContentId = "MapImage";
                    appointment.Attachments.Add(mapAttachment);
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to attach map image: {ex.Message}");
                    client?.Dispose();
                    return;
                }

                // Create the appointment on the server (sends meeting invitations)
                try
                {
                    string appointmentId = client.CreateAppointment(appointment);
                    Console.WriteLine($"Appointment created with ID: {appointmentId}");
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to create appointment: {ex.Message}");
                }
                finally
                {
                    client?.Dispose();
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Unexpected error: {ex.Message}");
            }
        }
    }
}
