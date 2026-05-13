using Aspose.Email;
using System;
using System.IO;
using Aspose.Email.Calendar;
using Aspose.Email.Mapi;

class Program
{
    static void Main()
    {
        try
        {
            string outputPath = "Task.msg";
            string outputDirectory = Path.GetDirectoryName(outputPath);
            if (!string.IsNullOrEmpty(outputDirectory) && !Directory.Exists(outputDirectory))
            {
                Directory.CreateDirectory(outputDirectory);
            }

            using (Aspose.Email.Calendar.Task outlookTask = new Aspose.Email.Calendar.Task())
            {
                outlookTask.Subject = "Sample Task";
                outlookTask.Body = "Complete the report.";
                outlookTask.StartDate = DateTime.Now;
                outlookTask.DueDate = DateTime.Now.AddDays(3);
                outlookTask.Save(outputPath, TaskSaveFormat.Msg);
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine(ex.Message);
        }
    }
}
