using System;
using System.IO;
using Aspose.Email;
using Aspose.Email.Calendar;
using Aspose.Email.Calendar.Recurrences;
using Aspose.Email.Clients.Exchange.WebService;
using Aspose.Email.Mapi;

class Program
{
    static void Main()
    {
        try
        {
            string msgPath = "task.msg";
            string mhtPath = "task.mht";

            // Ensure output directories exist
            string msgDir = Path.GetDirectoryName(msgPath);
            if (!string.IsNullOrEmpty(msgDir) && !Directory.Exists(msgDir))
            {
                Directory.CreateDirectory(msgDir);
            }

            string mhtDir = Path.GetDirectoryName(mhtPath);
            if (!string.IsNullOrEmpty(mhtDir) && !Directory.Exists(mhtDir))
            {
                Directory.CreateDirectory(mhtDir);
            }

            // Create a weekly recurring task that occurs on Fridays only
            using (ExchangeTask task = new ExchangeTask())
            {
                task.Subject = "Weekly Report";
                task.StartDate = DateTime.Today;
                task.DueDate = DateTime.Today.AddDays(1);

                // Weekly recurrence pattern with interval of 1 week
                WeeklyRecurrencePattern recurrence = new WeeklyRecurrencePattern(task.StartDate, 1);
                task.RecurrencePattern = recurrence;

                // Save the task to MSG format
                task.Save(msgPath, TaskSaveFormat.Msg);
            }

            // Load the MSG file as a MailMessage and export it to MHT for preview
            using (MailMessage mail = MailMessage.Load(msgPath))
            {
                MhtSaveOptions mhtOptions = new MhtSaveOptions();
                mail.Save(mhtPath, mhtOptions);
            }

            Console.WriteLine("Aspose.Email.Calendar.Task exported to MHT successfully.");
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
