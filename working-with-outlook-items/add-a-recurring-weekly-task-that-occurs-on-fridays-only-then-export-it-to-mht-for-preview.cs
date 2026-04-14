using System;
using System.IO;
using Aspose.Email;
using Aspose.Email.Calendar;
using Aspose.Email.Calendar.Recurrences;
using Aspose.Email.Clients.Exchange.WebService;

class Program
{
    static void Main()
    {
        try
        {
            // Define file paths
            string msgPath = "task.msg";
            string mhtPath = "task.mht";

            // Ensure the directory for the output files exists
            try
            {
                string? directory = Path.GetDirectoryName(Path.GetFullPath(msgPath));
                if (!string.IsNullOrEmpty(directory) && !Directory.Exists(directory))
                {
                    Directory.CreateDirectory(directory);
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Failed to prepare output directory: {ex.Message}");
                return;
            }

            // Create a recurring weekly task that occurs on Fridays only
            using (ExchangeTask task = new ExchangeTask())
            {
                task.Subject = "Weekly Report";
                task.StartDate = DateTime.Today.AddHours(9);      // 9:00 AM today
                task.DueDate = DateTime.Today.AddHours(10);      // 10:00 AM today
                task.Body = "Prepare and send the weekly status report.";

                // Configure weekly recurrence on Fridays
                WeeklyRecurrencePattern recurrence = new WeeklyRecurrencePattern(DateTime.Today, 1);
                recurrence.Interval = 1;                         // Every week
                task.RecurrencePattern = recurrence;

                // Save the task to MSG format
                try
                {
                    task.Save(msgPath, Aspose.Email.Mapi.TaskSaveFormat.Msg);
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to save task to MSG: {ex.Message}");
                    return;
                }
            }

            // Load the saved MSG as a MailMessage
            if (!File.Exists(msgPath))
            {
                try
                {
                    using (MailMessage placeholder = new MailMessage(
                        "sender@example.com",
                        "recipient@example.com",
                        "Placeholder Subject",
                        "Placeholder body."))
                    {
                        placeholder.Save(msgPath, new MsgSaveOptions(MailMessageSaveType.OutlookMessageFormat));
                    }
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Error creating placeholder MSG: {ex.Message}");
                    return;
                }

                Console.Error.WriteLine("The task MSG file was not created.");
                return;
            }

            using (MailMessage mail = MailMessage.Load(msgPath))
            {
                // Prepare MHT save options to render task fields
                MhtSaveOptions mhtOptions = new MhtSaveOptions();
                mhtOptions.MhtFormatOptions = MhtFormatOptions.RenderTaskFields;

                // Save the MailMessage as MHT for preview
                try
                {
                    mail.Save(mhtPath, mhtOptions);
                    Console.WriteLine($"Task preview saved to MHT at: {Path.GetFullPath(mhtPath)}");
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to save MHT preview: {ex.Message}");
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
