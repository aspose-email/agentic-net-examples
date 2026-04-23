using System;
using System.IO;
using Aspose.Email;
using Aspose.Email.Clients;
using Aspose.Email.Clients.Google;
using Aspose.Email.Clients.Smtp;
using Aspose.Email.Calendar;

class Program
{
    static void Main()
    {
        try
        {
            // Placeholder credentials – replace with real values or skip execution.
            string clientId = "your_client_id";
            string clientSecret = "your_client_secret";
            string refreshToken = "your_refresh_token";
            string defaultEmail = "your_email@gmail.com";
            string recipientEmail = "recipient@example.com";

            // Detect placeholder values and exit gracefully.
            if (clientId.StartsWith("your_") ||
                clientSecret.StartsWith("your_") ||
                refreshToken.StartsWith("your_") ||
                defaultEmail.StartsWith("your_"))
            {
                Console.Error.WriteLine("Please provide valid Google OAuth credentials. Skipping execution.");
                return;
            }

            // Obtain OAuth access token.
            TokenProvider tokenProvider = TokenProvider.Google.GetInstance(clientId, clientSecret, refreshToken);
            OAuthToken oauthToken;
            try
            {
                oauthToken = tokenProvider.GetAccessToken();
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Failed to acquire access token: {ex.Message}");
                return;
            }

            // Prepare the iCalendar appointment.
            MailAddress organizer = new MailAddress(defaultEmail);
            MailAddressCollection attendees = new MailAddressCollection();
            attendees.Add(new MailAddress(recipientEmail));

            Appointment appointment = new Appointment(
                "Team Meeting",
                DateTime.Now.AddHours(1),
                DateTime.Now.AddHours(2),
                organizer,
                attendees);
            appointment.Summary = "Project Sync";
            appointment.Description = "Discuss project updates and next steps.";

            // Save the appointment to an .ics file.
            string icsPath = "invitation.ics";
            try
            {
                // Ensure the directory for the .ics file exists.
                string icsDirectory = Path.GetDirectoryName(Path.GetFullPath(icsPath));
                if (!Directory.Exists(icsDirectory))
                {
                    Directory.CreateDirectory(icsDirectory);
                }

                appointment.Save(icsPath);
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Failed to create iCalendar file: {ex.Message}");
                return;
            }

            // Compose the email message.
            using (MailMessage message = new MailMessage())
            {
                message.From = organizer;
                message.To.Add(recipientEmail);
                message.Subject = "Invitation: Project Sync";
                message.Body = "Please find the meeting invitation attached.";

                // Attach the .ics file.
                try
                {
                    if (!File.Exists(icsPath))
                    {
                        Console.Error.WriteLine("iCalendar file not found.");
                        return;
                    }

                    Attachment icsAttachment = new Attachment(icsPath);
                    message.Attachments.Add(icsAttachment);
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to attach iCalendar file: {ex.Message}");
                    return;
                }

                // Send the email via Gmail SMTP using OAuth2.
                using (SmtpClient smtpClient = new SmtpClient(
                    "smtp.gmail.com",
                    587,
                    defaultEmail,
                    oauthToken.Token,
                    true,
                    SecurityOptions.Auto))
                {
                    try
                    {
                        smtpClient.Send(message);
                        Console.WriteLine("Invitation email sent successfully.");
                    }
                    catch (Exception ex)
                    {
                        Console.Error.WriteLine($"Failed to send email: {ex.Message}");
                    }
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
