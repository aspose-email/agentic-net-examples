using System;
using System.IO;
using Aspose.Email;
using Aspose.Email.Calendar;
using Aspose.Email.Mapi;
using Aspose.Email.Storage.Pst;
using Aspose.Email.Clients.Smtp;

class Program
{
    static void Main()
    {
        try
        {
            // Define file paths
            string icsPath = "sample.ics";
            string msgPath = "output.msg";
            string pstPath = "sample.pst";

            // Ensure .ics file exists; create minimal placeholder if missing
            if (!File.Exists(icsPath))
            {
                try
                {
                    File.WriteAllText(icsPath, "BEGIN:VCALENDAR\r\nVERSION:2.0\r\nEND:VCALENDAR");
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Error creating placeholder .ics file: {ex.Message}");
                    return;
                }
            }

            // Load appointment from .ics
            Appointment appointment;
            try
            {
                appointment = Appointment.Load(icsPath);
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Error loading .ics file: {ex.Message}");
                return;
            }

            // Convert appointment to MAPI message
            MapiMessage mapiMessage;
            try
            {
                mapiMessage = appointment.ToMapiMessage();
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Error converting appointment to MAPI message: {ex.Message}");
                return;
            }

            // Save MAPI message as .msg
            try
            {
                using (mapiMessage)
                {
                    mapiMessage.Save(msgPath);
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Error saving .msg file: {ex.Message}");
                return;
            }

            // Ensure PST file exists; create minimal placeholder if missing
            if (!File.Exists(pstPath))
            {
                try
                {
                    using (PersonalStorage pst = PersonalStorage.Create(pstPath, FileFormatVersion.Unicode))
                    {
                        // Create an Inbox folder
                        pst.RootFolder.AddSubFolder("Inbox");
                    }
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Error creating placeholder PST file: {ex.Message}");
                    return;
                }
            }

            // Add the MAPI message to the PST Inbox folder
            try
            {
                using (PersonalStorage pst = PersonalStorage.FromFile(pstPath))
                {
                    FolderInfo inbox;
                    // Try to get existing Inbox; create if it does not exist
                    try
                    {
                        inbox = pst.RootFolder.GetSubFolder("Inbox");
                    }
                    catch
                    {
                        inbox = pst.RootFolder.AddSubFolder("Inbox");
                    }

                    // Add the message
                    inbox.AddMessage(mapiMessage);
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Error adding message to PST: {ex.Message}");
                return;
            }

            // Convert MAPI message back to MailMessage for SMTP sending
            MailMessage mailMessage;
            try
            {
                mailMessage = mapiMessage.ToMailMessage(new MailConversionOptions());
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Error converting MAPI to MailMessage: {ex.Message}");
                return;
            }

            // SMTP send (placeholder credentials – skip actual send)
            string smtpHost = "smtp.example.com";
            int smtpPort = 25;
            string smtpUser = "username";
            string smtpPass = "password";

            // Detect placeholder values and skip sending
            if (smtpHost.Contains("example.com") || smtpUser == "username")
            {
                Console.WriteLine("Skipping SMTP send due to placeholder credentials/host.");
                return;
            }

            try
            {
                using (SmtpClient client = new SmtpClient(smtpHost, smtpPort, smtpUser, smtpPass))
                {
                    client.Send(mailMessage);
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"SMTP send failed: {ex.Message}");
                return;
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
