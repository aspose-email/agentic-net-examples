using Aspose.Email.Clients;
using System;
using System.IO;
using Aspose.Email;
using Aspose.Email.Clients.Smtp;
using Aspose.Email.Calendar;
using Aspose.Email.Mapi;

class Program
{
    static void Main()
    {
        try
        {
            // Define file paths
            string icsPath = "sample.ics";
            string msgPath = "output.msg";

            // Ensure the input .ics file exists; create a minimal placeholder if missing
            if (!File.Exists(icsPath))
            {
                try
                {
                    string placeholder = "BEGIN:VCALENDAR\r\nVERSION:2.0\r\nEND:VCALENDAR";
                    File.WriteAllText(icsPath, placeholder);
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
                Console.Error.WriteLine($"Failed to load appointment: {ex.Message}");
                return;
            }

            // Edit some properties of the appointment
            appointment.Summary = "Updated Meeting Subject";
            appointment.Location = "Conference Room A";

            // Convert the appointment to a MAPI message
            MapiMessage mapMessage;
            try
            {
                mapMessage = appointment.ToMapiMessage();
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Failed to convert appointment to MAPI message: {ex.Message}");
                return;
            }

            // Ensure the output directory exists
            string msgDirectory = Path.GetDirectoryName(Path.GetFullPath(msgPath));
            if (!string.IsNullOrEmpty(msgDirectory) && !Directory.Exists(msgDirectory))
            {
                try
                {
                    Directory.CreateDirectory(msgDirectory);
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to create output directory: {ex.Message}");
                    return;
                }
            }

            // Save the MAPI message as .msg
            try
            {
                using (mapMessage)
                {
                    mapMessage.Save(msgPath);
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Failed to save .msg file: {ex.Message}");
                return;
            }

            // Convert the MAPI message to a MailMessage for SMTP sending
            MailMessage mailMessage;
            try
            {
                mailMessage = mapMessage.ToMailMessage(new MailConversionOptions());
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Failed to convert MAPI message to MailMessage: {ex.Message}");
                return;
            }

            // Send the email via SMTP
            try
            {
                using (mailMessage)
                using (SmtpClient smtpClient = new SmtpClient("smtp.example.com", 587, "username", "password", SecurityOptions.Auto))
                {
                    smtpClient.Send(mailMessage);
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"SMTP send failed: {ex.Message}");
                return;
            }

            Console.WriteLine("Appointment processed, saved as MSG, and email sent successfully.");
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}