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
            // Placeholder credentials – skip real network call if they are not set.
            string mailboxUri = "https://example.com/EWS/Exchange.asmx";
            string username = "user@example.com";
            string password = "password";

            bool usePlaceholder = string.IsNullOrWhiteSpace(username) ||
                                   username.Contains("example.com") ||
                                   string.IsNullOrWhiteSpace(password);

            // Ensure map image exists (create minimal PNG if missing).
            string mapImagePath = "map.png";
            try
            {
                if (!File.Exists(mapImagePath))
                {
                    // 1x1 pixel transparent PNG.
                    byte[] pngBytes = Convert.FromBase64String(
                        "iVBORw0KGgoAAAANSUhEUgAAAAEAAAABCAQAAAC1HAwCAAAAC0lEQVR42mP8/x8AAwMCAO+XK2cAAAAASUVORK5CYII=");
                    File.WriteAllBytes(mapImagePath, pngBytes);
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Failed to prepare map image: {ex.Message}");
                return;
            }

            if (usePlaceholder)
            {
                Console.WriteLine("Placeholder credentials detected – skipping EWS operation.");
                return;
            }

            // Create EWS client.
            IEWSClient client = null;
            try
            {
                client = EWSClient.GetEWSClient(mailboxUri, username, password);
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Failed to create EWS client: {ex.Message}");
                return;
            }

            using (client)
            {
                // Prepare attendees.
                MailAddressCollection attendees = new MailAddressCollection();
                attendees.Add(new MailAddress("attendee1@domain.com"));
                attendees.Add(new MailAddress("attendee2@domain.com"));

                // Create appointment.
                Appointment meeting = new Appointment(
                    "Project Kick‑off",
                    new DateTime(2026, 5, 1, 10, 0, 0),
                    new DateTime(2026, 5, 1, 11, 0, 0),
                    new MailAddress(username),
                    attendees);

                                meeting.Summary = "Meeting Summary";
meeting.Location = "Conference Room A";
                meeting.Description = "Discuss project goals and milestones.";
                meeting.HtmlDescription = "<p>Discuss project goals and milestones.</p>";

                // Embed map image as attachment.
                try
                {
                    meeting.Attachments.Add(new Attachment(mapImagePath));
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to add attachment: {ex.Message}");
                    return;
                }

                // Send the meeting request.
                try
                {
                    string appointmentId = client.CreateAppointment(meeting);
                    Console.WriteLine($"Meeting request sent. Appointment ID: {appointmentId}");
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to create appointment: {ex.Message}");
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
