using System;
using System.IO;
using Aspose.Email;
using Aspose.Email.Mapi;

namespace AsposeEmailTaskExample
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                string msgFilePath = "task.msg";

                if (!File.Exists(msgFilePath))
                {
                try
                {
                    using (MapiMessage placeholder = new MapiMessage(
                        "from@example.com",
                        "to@example.com",
                        "Placeholder Subject",
                        "Placeholder body."))
                    {
                        placeholder.Save(msgFilePath);
                    }
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Error creating placeholder MSG: {ex.Message}");
                    return;
                }

                    Console.Error.WriteLine($"File not found: {msgFilePath}");
                    return;
                }

                using (MapiMessage mapiMessage = MapiMessage.Load(msgFilePath))
                {
                    if (mapiMessage.SupportedType == MapiItemType.Task)
                    {
                        using (MapiTask mapiTask = (MapiTask)mapiMessage.ToMapiMessageItem())
                        {
                            Console.WriteLine($"Subject: {mapiTask.Subject}");
                            Console.WriteLine($"Due Date: {mapiTask.DueDate}");
                            Console.WriteLine($"Start Date: {mapiTask.StartDate}");
                            Console.WriteLine($"Percent Complete: {mapiTask.PercentComplete}");
                        }
                    }
                    else
                    {
                        Console.WriteLine("The loaded MSG file is not a task item.");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Error: {ex.Message}");
            }
        }
    }
}
