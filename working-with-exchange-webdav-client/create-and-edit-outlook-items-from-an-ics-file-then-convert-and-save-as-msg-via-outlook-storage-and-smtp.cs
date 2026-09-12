using System;
using System.IO;
using Aspose.Email;
using Aspose.Email.Calendar;
using Aspose.Email.Mapi;
using Aspose.Email.Clients.Smtp;
using Aspose.Email.Mapi; // for MailConversionOptions
using Aspose.Email.Clients; // for SecurityOptions

class Program
{
    static void Main()
    {
        try
        {
            // Input and output file paths
            string icsPath = "input.ics";
            string msgPath = "output.msg";

            // Guard file existence
            if (!File.Exists(icsPath))
            {
                Console.Error.WriteLine($"Input file not found: {icsPath}");
                return;
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

            // Example edit: change the subject
            appointment.Summary = "Updated Meeting Subject";

            // Convert the appointment to a MAPI message
            MapiMessage mapMsg = appointment.ToMapiMessage();

            // Save the MAPI message as .msg
            try
            {
                // Ensure the directory for the output file exists
                string outDir = Path.GetDirectoryName(msgPath);
                if (!string.IsNullOrEmpty(outDir) && !Directory.Exists(outDir))
                {
                    Directory.CreateDirectory(outDir);
                }

                mapMsg.Save(msgPath);
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Failed to save MSG file: {ex.Message}");
                return;
            }

            // Convert MapiMessage to MailMessage for SMTP sending
            MailConversionOptions convOpts = new MailConversionOptions();
            MailMessage mailMessage = mapMsg.ToMailMessage(convOpts);

            // SMTP client configuration (replace with real credentials)
            string smtpHost = "smtp.example.com";
            int smtpPort = 587;
            string smtpUser = "user@example.com";
            string smtpPass = "password";


            // Skip external calls when placeholder credentials are used
            if (smtpHost.Contains("example.com") || smtpUser.Contains("example.com") || smtpPass == "password")
            {
                Console.Error.WriteLine("Placeholder credentials detected. Skipping external calls.");
                return;
            }

            // Send the email via SMTP
            try
            {
                using (SmtpClient smtpClient = new SmtpClient(smtpHost, smtpPort, smtpUser, smtpPass))
                {
                    smtpClient.SecurityOptions = SecurityOptions.Auto;
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
