using System;
using System.IO;
using Aspose.Email;
using Aspose.Email.Calendar;
using Aspose.Email.Clients;
using Aspose.Email.Clients.Smtp;
using Aspose.Email.Mapi;

class Program
{
    static void Main()
    {
        try
        {
            // Author note: This sample demonstrates creating an appointment from an .ics file,
            // converting it to a .msg file, loading a .msg file, and sending the message via SMTP.

            // Define file paths
            string icsPath = "meeting.ics";
            string msgOutputPath = "meeting.msg";
            
            string outputDir = Path.GetDirectoryName(msgOutputPath);
            if (!string.IsNullOrEmpty(outputDir) && !Directory.Exists(outputDir))
            {
                Directory.CreateDirectory(outputDir);
            }
string msgInputPath = "sample.msg";

            // Ensure the .ics file exists; create a minimal placeholder if missing
            if (!File.Exists(icsPath))
            {
                try
                {
                    string minimalIcs = "BEGIN:VCALENDAR\r\nVERSION:2.0\r\nBEGIN:VEVENT\r\nDTSTART:20240101T090000Z\r\nDTEND:20240101T100000Z\r\nSUMMARY:Sample Meeting\r\nEND:VEVENT\r\nEND:VCALENDAR";
                    File.WriteAllText(icsPath, minimalIcs);
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
                Console.Error.WriteLine($"Failed to load appointment from .ics: {ex.Message}");
                return;
            }

            // Convert the appointment to a MailMessage
            MailMessage mailMessageFromIcs;
            try
            {
                mailMessageFromIcs = appointment.ToMailMessage();
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Failed to convert appointment to MailMessage: {ex.Message}");
                return;
            }

            // Save the MailMessage as a .msg file
            try
            {
                mailMessageFromIcs.Save(msgOutputPath, SaveOptions.DefaultMsg);
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Failed to save .msg file: {ex.Message}");
                return;
            }

            // Load an existing .msg file if it exists
            if (File.Exists(msgInputPath))
            {
                MapiMessage mapMsg;
                try
                {
                    mapMsg = MapiMessage.Load(msgInputPath);
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to load .msg file: {ex.Message}");
                    mapMsg = null;
                }

                if (mapMsg != null)
                {
                    MailMessage mailMessageFromMsg;
                    try
                    {
                        mailMessageFromMsg = mapMsg.ToMailMessage(new MailConversionOptions());
                    }
                    catch (Exception ex)
                    {
                        Console.Error.WriteLine($"Failed to convert .msg to MailMessage: {ex.Message}");
                        mailMessageFromMsg = null;
                    }

                    if (mailMessageFromMsg != null)
                    {
                        // Prepare SMTP client settings
                        string smtpHost = "smtp.example.com";
                        int smtpPort = 587;
                        string smtpUser = "user@example.com";
                        string smtpPass = "password";

                        // Send the message via SMTP
                        try
                        {
                            using (SmtpClient smtpClient = new SmtpClient(smtpHost, smtpPort, smtpUser, smtpPass))
                            {
                                smtpClient.SecurityOptions = SecurityOptions.Auto;
                                smtpClient.Send(mailMessageFromMsg);
                            }
                        }
                        catch (Exception ex)
                        {
                            Console.Error.WriteLine($"SMTP send failed: {ex.Message}");
                            // Continue without rethrowing
                        }
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
