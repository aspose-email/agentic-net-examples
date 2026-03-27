using System;
using System.IO;
using Aspose.Email;
using Aspose.Email.Calendar;
using Aspose.Email.Mapi;
using Aspose.Email.Clients;

class Program
{
    static void Main()
    {
        try
        {
            string icsPath = "sample.ics";
            string msgPath = "updated.msg";

            // Ensure the input file exists; create a minimal placeholder if missing
            if (!File.Exists(icsPath))
            {
                try
                {
                    File.WriteAllText(icsPath, "BEGIN:VCALENDAR\r\nVERSION:2.0\r\nEND:VCALENDAR");
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Error creating placeholder iCalendar file: {ex.Message}");
                    return;
                }
            }

            // Load the iCalendar file
            Appointment appointment;
            try
            {
                appointment = Appointment.Load(icsPath);
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Error loading iCalendar file: {ex.Message}");
                return;
            }

            // Modify the appointment as required (example: change the summary)
            appointment.Summary = "Updated Summary";

            // Convert the appointment to a MailMessage
            using (MailMessage mailMessage = appointment.ToMailMessage())
            {
                // Prepare MSG save options (Unicode format)
                MsgSaveOptions saveOptions = new MsgSaveOptions(MailMessageSaveType.OutlookMessageFormatUnicode);

                // Save the MailMessage as MSG
                try
                {
                    mailMessage.Save(msgPath, saveOptions);
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Error saving MSG file: {ex.Message}");
                    return;
                }
            }

            Console.WriteLine("iCalendar processed and saved as MSG successfully.");
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}