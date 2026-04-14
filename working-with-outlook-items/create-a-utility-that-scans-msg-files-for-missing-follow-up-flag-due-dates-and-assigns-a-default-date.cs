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
                Console.Error.WriteLine($"Directory not found: {folderPath}");
                return;
            }

            string[] msgFiles = Directory.GetFiles(folderPath, "*.msg");
            foreach (string msgFile in msgFiles)
            {
                try
                {
                    if (!File.Exists(msgFile))
                    {
                try
                {
                    using (MapiMessage placeholder = new MapiMessage(
                        "from@example.com",
                        "to@example.com",
                        "Placeholder Subject",
                        "Placeholder body."))
                    {
                        placeholder.Save(msgFile);
                    }
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Error creating placeholder MSG: {ex.Message}");
                    return;
                }

                        Console.Error.WriteLine($"File not found: {msgFile}");
                        continue;
                    }

                    using (MapiMessage message = MapiMessage.Load(msgFile))
                    {
                        FollowUpOptions options = FollowUpManager.GetOptions(message);
                        DateTime dueDate = options.DueDate;

                        if (dueDate == DateTime.MinValue)
                        {
                            DateTime startDate = DateTime.Now;
                            DateTime defaultDueDate = startDate.AddDays(7);
                            FollowUpManager.SetFlag(message, "Follow up", startDate, defaultDueDate);
                            message.Save(msgFile);
                            Console.WriteLine($"Updated follow-up flag for: {msgFile}");
                        }
                        else
                        {
                            Console.WriteLine($"Follow-up already set for: {msgFile}");
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Error processing file {msgFile}: {ex.Message}");
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
