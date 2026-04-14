using System;
using System.IO;
using Aspose.Email;
using Aspose.Email.Clients.Exchange.WebService;
using Aspose.Email.Calendar;

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

            // Path to the custom form to be attached
            string formPath = "CustomForm.pdf";

            // Ensure the custom form file exists; create an empty placeholder if missing
            if (!File.Exists(formPath))
            {
                try
                {
                    File.WriteAllBytes(formPath, new byte[0]);
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to create placeholder form file: {ex.Message}");
                    return;
                }
            }

            // Create the EWS client
            using (IEWSClient client = EWSClient.GetEWSClient(mailboxUri, username, password))
            {
                // Build the meeting appointment
                MailAddress organizer = new MailAddress(username);
                MailAddressCollection attendees = new MailAddressCollection();
                attendees.Add(new MailAddress("attendee@example.com"));

                Appointment appointment = new Appointment(
                    "Project Kickoff",
                    new DateTime(2023, 12, 1, 10, 0, 0),
                    new DateTime(2023, 12, 1, 11, 0, 0),
                    organizer,
                    attendees);
                appointment.Location = "Conference Room A";
                appointment.Description = "Kickoff meeting with attached form.";

                // Convert the appointment to a MailMessage so we can add an attachment
                MailMessage meetingMessage = appointment.ToMailMessage();

                // Attach the custom form
                try
                {
                    using (FileStream fs = File.OpenRead(formPath))
                    {
                        Attachment attachment = new Attachment(fs, Path.GetFileName(formPath));
                        meetingMessage.Attachments.Add(attachment);
                    }
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to attach form: {ex.Message}");
                    return;
                }

                // Send the meeting request
                try
                {
                    client.Send(meetingMessage);
                    Console.WriteLine("Meeting request sent successfully.");
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to send meeting request: {ex.Message}");
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
