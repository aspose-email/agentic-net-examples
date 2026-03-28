using System;
using System.IO;
using System.Net;
using Aspose.Email;
using Aspose.Email.Calendar;
using Aspose.Email.Mapi;
using Aspose.Email.Clients.Exchange.Dav;

class Program
{
    static void Main()
    {
        try
        {
            // Define file paths
            string icsPath = "sample.ics";
            string msgPath = "output.msg";

            // Ensure the directory for the MSG file exists
            string msgDir = Path.GetDirectoryName(Path.GetFullPath(msgPath));
            if (!Directory.Exists(msgDir))
            {
                try
                {
                    Directory.CreateDirectory(msgDir);
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to create directory '{msgDir}': {ex.Message}");
                    return;
                }
            }

            // Ensure the ICS file exists; create a minimal placeholder if missing
            if (!File.Exists(icsPath))
            {
                try
                {
                    // Create a simple appointment and save as .ics
                    Appointment placeholder = new Appointment(
                        "Placeholder Meeting",
                        new DateTime(2025, 1, 1, 9, 0, 0),
                        new DateTime(2025, 1, 1, 10, 0, 0),
                        new MailAddress("organizer@example.com"),
                        new MailAddressCollection { new MailAddress("attendee@example.com") });
                    placeholder.Save(icsPath);
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to create placeholder .ics file: {ex.Message}");
                    return;
                }
            }

            // Load the appointment from the .ics file
            Appointment appointment;
            try
            {
                appointment = Appointment.Load(icsPath);
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Failed to load appointment from '{icsPath}': {ex.Message}");
                return;
            }

            // Convert the appointment to a MAPI message
            MapiMessage mapiMessage;
            try
            {
                mapiMessage = appointment.ToMapiMessage();
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Failed to convert appointment to MAPI message: {ex.Message}");
                return;
            }

            // Save the MAPI message as MSG
            try
            {
                mapiMessage.Save(msgPath);
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Failed to save MSG file '{msgPath}': {ex.Message}");
                return;
            }

            // Convert the MAPI message to a MailMessage for sending
            MailMessage mailMessage;
            try
            {
                mailMessage = mapiMessage.ToMailMessage(new MailConversionOptions());
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Failed to convert MAPI message to MailMessage: {ex.Message}");
                return;
            }

            // Prepare Exchange WebDAV client (SMTP/Exchange) credentials
            string exchangeUri = "https://exchange.example.com/ews/exchange.asmx";
            string username = "user@example.com";
            string password = "password";

            // Send the message via Exchange client
            try
            {
                using (ExchangeClient client = new ExchangeClient(exchangeUri, new NetworkCredential(username, password)))
                {
                    client.Send(mailMessage);
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Failed to send email via Exchange client: {ex.Message}");
                return;
            }

            // Clean up disposable objects
            mailMessage.Dispose();
            mapiMessage.Dispose();
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
