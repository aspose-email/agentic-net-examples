using System;
using System.IO;
using Aspose.Email;
using Aspose.Email.Mapi;
using Aspose.Email.Mime;

class Program
{
    static void Main()
    {
        try
        {
            // Define file paths
            string msgFilePath = "task.msg";
            string mhtFilePath = "task.mht";

            // Ensure the output directory exists
            string outputDirectory = Path.GetDirectoryName(Path.GetFullPath(mhtFilePath));
            if (!Directory.Exists(outputDirectory))
            {
                Directory.CreateDirectory(outputDirectory);
            }

            // Create a sample MapiTask and populate fields
            using (MapiTask mapiTask = new MapiTask())
            {
                mapiTask.Subject = "Project Plan";
                mapiTask.Body = "Complete the project milestones.";
                mapiTask.StartDate = DateTime.Now;
                mapiTask.DueDate = DateTime.Now.AddDays(7);
                mapiTask.PercentComplete = 25;
                mapiTask.Priority = MapiTaskPriority.Normal;          // enum value
                mapiTask.Status = MapiTaskStatus.InProgress;          // enum value
                mapiTask.ActualEffort = 120;
                mapiTask.EstimatedEffort = 480;
                mapiTask.Billing = "Client XYZ";
                mapiTask.Categories = new[] { "Planning", "Internal" }; // string array

                // Save the task as MSG (required for MHT conversion)
                try
                {
                    mapiTask.Save(msgFilePath, TaskSaveFormat.Msg);
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Error saving MSG file: {ex.Message}");
                    return;
                }

                // Load the MSG as a MailMessage to enable MHT conversion
                MailMessage mailMessage;
                try
                {
                    mailMessage = MailMessage.Load(msgFilePath);
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Error loading MSG as MailMessage: {ex.Message}");
                    return;
                }

                using (mailMessage)
                {
                    // Configure MHT save options (default options are sufficient)
                    MhtSaveOptions mhtOptions = new MhtSaveOptions();

                    // Save as MHT
                    try
                    {
                        mailMessage.Save(mhtFilePath, mhtOptions);
                        Console.WriteLine($"Task exported to MHT successfully: {mhtFilePath}");
                    }
                    catch (Exception ex)
                    {
                        Console.Error.WriteLine($"Error saving MHT file: {ex.Message}");
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
