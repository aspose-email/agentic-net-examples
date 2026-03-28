using System;
using System.IO;
using Aspose.Email;
using Aspose.Email.Clients.Smtp;
using Aspose.Email.Storage.Pst;
using Aspose.Email.Mapi;
using Aspose.Email.Calendar;

class Program
{
    static void Main()
    {
        try
        {
            // File paths
            string icsPath = "sample.ics";
            string msgPath = "sample.msg";
            string pstPath = "sample.pst";

            // Ensure the .ics file exists; create a minimal placeholder if missing
            if (!File.Exists(icsPath))
            {
                string minimalIcs = "BEGIN:VCALENDAR\r\nVERSION:2.0\r\nEND:VCALENDAR";
                File.WriteAllText(icsPath, minimalIcs);
            }

            // Load the .ics file as an Appointment
            Appointment appointment = Appointment.Load(icsPath);

            // Convert the Appointment to a MailMessage (iCalendar -> MIME)
            using (MailMessage icsMail = appointment.ToMailMessage())
            {
                // Attach the original .ics file to the email
                using (Attachment icsAttachment = new Attachment(icsPath))
                {
                    icsMail.Attachments.Add(icsAttachment);

                    // Convert the MailMessage to a MAPI message and save as .msg
                    using (MapiMessage mapiMsg = MapiMessage.FromMailMessage(icsMail))
                    {
                        mapiMsg.Save(msgPath);
                    }
                }

                // Ensure the PST file exists; create an empty PST with an Inbox folder if missing
                if (!File.Exists(pstPath))
                {
                    using (PersonalStorage pstCreate = PersonalStorage.Create(pstPath, FileFormatVersion.Unicode))
                    {
                        pstCreate.RootFolder.AddSubFolder("Inbox");
                    }
                }

                // Open the PST and add the .msg as a message in the Inbox folder
                using (PersonalStorage pst = PersonalStorage.FromFile(pstPath))
                {
                    FolderInfo inbox = pst.RootFolder.GetSubFolder("Inbox");
                    using (MapiMessage msg = MapiMessage.Load(msgPath))
                    {
                        inbox.AddMessage(msg);
                    }
                }

                // Send the email via SMTP
                using (SmtpClient smtp = new SmtpClient("smtp.example.com", 25, "username", "password"))
                {
                    smtp.Send(icsMail);
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
