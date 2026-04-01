using System;
using System.IO;
using System.Net;
using Aspose.Email;
using Aspose.Email.Calendar;
using Aspose.Email.Mapi;
using Aspose.Email.Clients.Smtp;
using Aspose.Email.Clients.Exchange.Dav;

class Program
{
    static void Main(string[] args)
    {
        try
        {
            // Input and output paths
            string icsPath = "appointment.ics";
            string msgPath = "appointment.msg";

            // Ensure the input .ics file exists; create a minimal placeholder if missing
            if (!File.Exists(icsPath))
            {
                try
                {
                    File.WriteAllText(icsPath, "BEGIN:VCALENDAR\r\nVERSION:2.0\r\nEND:VCALENDAR");
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

            // Edit the appointment (example modification)
            appointment.Summary = "Updated Summary";
            appointment.Description = "Edited description via Aspose.Email.";

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

            // Ensure output directory exists
            try
            {
                string outputDir = Path.GetDirectoryName(msgPath);
                if (!string.IsNullOrEmpty(outputDir) && !Directory.Exists(outputDir))
                {
                    Directory.CreateDirectory(outputDir);
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Failed to prepare output directory: {ex.Message}");
                return;
            }

            // Save the MAPI message as .msg
            try
            {
                using (mapiMessage)
                {
                    mapiMessage.Save(msgPath);
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Failed to save MSG file: {ex.Message}");
                return;
            }

            // Convert MAPI message to MailMessage for SMTP sending
            MailMessage mailMessage;
            try
            {
                mailMessage = mapiMessage.ToMailMessage(new MailConversionOptions());
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Failed to convert MAPI to MailMessage: {ex.Message}");
                return;
            }

            // SMTP client configuration (placeholders)
            string smtpHost = "smtp.example.com";
            int smtpPort = 25;
            string smtpUser = "user";
            string smtpPass = "password";

            // Skip real SMTP send when placeholders are detected
            if (smtpHost.Contains("example.com"))
            {
                Console.WriteLine("Placeholder SMTP configuration detected; skipping send.");
            }
            else
            {
                using (SmtpClient smtpClient = new SmtpClient(smtpHost, smtpPort, smtpUser, smtpPass))
                {
                    try
                    {
                        smtpClient.Send(mailMessage);
                        Console.WriteLine("Email sent via SMTP.");
                    }
                    catch (Exception ex)
                    {
                        Console.Error.WriteLine($"SMTP send failed: {ex.Message}");
                    }
                }
            }

            // Exchange client configuration for uploading the MSG (placeholders)
            string exchangeUrl = "https://exchange.example.com/EWS/Exchange.asmx";
            string exchangeUser = "user";
            string exchangePass = "password";

            // Skip real Exchange upload when placeholders are detected
            if (exchangeUrl.Contains("example.com"))
            {
                Console.WriteLine("Placeholder Exchange configuration detected; skipping upload.");
            }
            else
            {
                using (ExchangeClient client = new ExchangeClient(exchangeUrl, new NetworkCredential(exchangeUser, exchangePass)))
                {
                    try
                    {
                        // Upload the message to the Inbox folder
                        client.AppendMessage(client.MailboxInfo.InboxUri, mailMessage);
                        Console.WriteLine("Message uploaded to Exchange mailbox.");
                    }
                    catch (Exception ex)
                    {
                        Console.Error.WriteLine($"Exchange upload failed: {ex.Message}");
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
