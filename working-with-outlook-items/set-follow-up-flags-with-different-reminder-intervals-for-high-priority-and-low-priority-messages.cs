using System;
using System.IO;
using Aspose.Email;
using Aspose.Email.Mapi;

class Program
{
    static void Main(string[] args)
    {
        try
        {
            // Ensure the output directory exists
            string outputDir = "Output";
            if (!Directory.Exists(outputDir))
            {
                Directory.CreateDirectory(outputDir);
            }

            // Create a high‑priority message
            MapiMessage highPriorityMessage = new MapiMessage(
                "high@example.com",
                "recipient@example.com",
                "High Priority",
                "This is a high priority message."
            );

            // Set a follow‑up flag with start and due dates (reminder interval)
            DateTime startDate = DateTime.Now;
            DateTime dueDate = startDate.AddDays(2);
            FollowUpManager.SetFlag(highPriorityMessage, "Follow up", startDate, dueDate);

            // Save the high‑priority message
            string highPriorityPath = Path.Combine(outputDir, "HighPriority.msg");
            using (highPriorityMessage)
            {
                highPriorityMessage.Save(highPriorityPath);
            }

            // Create a low‑priority message
            MapiMessage lowPriorityMessage = new MapiMessage(
                "low@example.com",
                "recipient@example.com",
                "Low Priority",
                "This is a low priority message."
            );

            // Set a simple follow‑up flag without a reminder
            FollowUpManager.SetFlag(lowPriorityMessage, "Follow up");

            // Save the low‑priority message
            string lowPriorityPath = Path.Combine(outputDir, "LowPriority.msg");
            using (lowPriorityMessage)
            {
                lowPriorityMessage.Save(lowPriorityPath);
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine(ex.Message);
        }
    }
}
