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
            string folderPath = args.Length > 0 ? args[0] : "Messages";

            if (!Directory.Exists(folderPath))
            {
                Console.Error.WriteLine($"Folder not found: {folderPath}");
                return;
            }

            string[] msgFiles = Directory.GetFiles(folderPath, "*.msg");
            foreach (string msgFile in msgFiles)
            {
                ProcessMessage(msgFile);
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }

    static void ProcessMessage(string filePath)
    {
        try
        {
            if (!File.Exists(filePath))
            {
                try
                {
                    using (MapiMessage placeholder = new MapiMessage(
                        "from@example.com",
                        "to@example.com",
                        "Placeholder Subject",
                        "Placeholder body."))
                    {
                        placeholder.Save(filePath);
                    }
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Error creating placeholder MSG: {ex.Message}");
                    return;
                }

                Console.Error.WriteLine($"File not found: {filePath}");
                return;
            }

            using (MapiMessage message = MapiMessage.Load(filePath))
            {
                FollowUpOptions options = FollowUpManager.GetOptions(message);
                DateTime dueDate = options.DueDate;

                if (dueDate == default(DateTime))
                {
                    DateTime startDate = DateTime.Now;
                    DateTime defaultDueDate = startDate.AddDays(7);

                    FollowUpManager.SetFlag(message, "Follow up", startDate, defaultDueDate);
                    message.Save(filePath);
                    Console.WriteLine($"Flag added with default due date to: {Path.GetFileName(filePath)}");
                }
                else
                {
                    Console.WriteLine($"Flag already present in: {Path.GetFileName(filePath)}");
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error processing '{filePath}': {ex.Message}");
        }
    }
}
