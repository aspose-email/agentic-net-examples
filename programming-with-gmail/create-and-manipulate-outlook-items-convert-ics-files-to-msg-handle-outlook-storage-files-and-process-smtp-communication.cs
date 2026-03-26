using System;
using System.IO;
using System.Net;
using Aspose.Email;
using Aspose.Email.Clients.Smtp;
using Aspose.Email.Storage.Pst;
using Aspose.Email.Calendar;
using Aspose.Email.Mapi;

class Program
{
    static void Main()
    {
        try
        {
            // Define file and SMTP parameters
            string icsPath = "sample.ics";
            string pstPath = "sample.pst";
            string appointmentMsgPath = "appointment.msg";
            string smtpHost = "smtp.example.com";
            int smtpPort = 25;
            string smtpUser = "user@example.com";
            string smtpPass = "password";

            // -------------------------------------------------
            // Convert .ics to .msg
            // -------------------------------------------------
            Appointment appointment = null;
            try
            {
                if (!File.Exists(icsPath))
                {
                    Console.Error.WriteLine($"Error: File not found – {icsPath}");
                    return;
                }

                appointment = Appointment.Load(icsPath);
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Error loading .ics file: {ex.Message}");
                return;
            }

            try
            {
                MapiMessage appointmentMessage = appointment.ToMapiMessage();
                appointmentMessage.Save(appointmentMsgPath);
                Console.WriteLine($"Appointment saved as MSG: {appointmentMsgPath}");
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Error saving appointment MSG: {ex.Message}");
                return;
            }

            // -------------------------------------------------
            // Process PST storage: extract each message as .msg
            // -------------------------------------------------
            if (!File.Exists(pstPath))
            {
                Console.Error.WriteLine($"Error: File not found – {pstPath}");
                return;
            }

            try
            {
                using (PersonalStorage pst = PersonalStorage.FromFile(pstPath))
                {
                    foreach (FolderInfo folderInfo in pst.RootFolder.GetSubFolders())
                    {
                        Console.WriteLine($"Processing folder: {folderInfo.DisplayName}");
                        foreach (MessageInfo messageInfo in folderInfo.EnumerateMessages())
                        {
                            try
                            {
                                MapiMessage msg = pst.ExtractMessage(messageInfo);
                                // Sanitize filename
                                string safeSubject = string.IsNullOrEmpty(msg.Subject) ? "NoSubject" : msg.Subject;
                                foreach (char c in Path.GetInvalidFileNameChars())
                                {
                                    safeSubject = safeSubject.Replace(c, '_');
                                }
                                string msgFileName = $"{safeSubject}.msg";
                                msg.Save(msgFileName);
                                Console.WriteLine($"Saved message: {msgFileName}");
                            }
                            catch (Exception ex)
                            {
                                Console.Error.WriteLine($"Error extracting/saving message: {ex.Message}");
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Error processing PST file: {ex.Message}");
                return;
            }

            // -------------------------------------------------
            // Send an email via SMTP
            // -------------------------------------------------
            try
            {
                using (MailMessage mail = new MailMessage())
                {
                    mail.From = new MailAddress(smtpUser);
                    mail.To.Add(new MailAddress("recipient@example.com"));
                    mail.Subject = "Test Email from Aspose.Email";
                    mail.Body = "This is a test email sent using Aspose.Email SMTP client.";

                    using (SmtpClient smtp = new SmtpClient(smtpHost, smtpPort, smtpUser, smtpPass))
                    {
                        smtp.Send(mail);
                        Console.WriteLine("Email sent successfully.");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"SMTP error: {ex.Message}");
                return;
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}