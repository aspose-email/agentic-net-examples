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
                    Console.Error.WriteLine($"Error: File not found – {msgFilePath}");
                    return;
                }

                using (MapiMessage mapiMessage = MapiMessage.Load(msgFilePath))
                {
                    if (mapiMessage.SupportedType == MapiItemType.Task)
                    {
                        using (MapiTask task = (MapiTask)mapiMessage.ToMapiMessageItem())
                        {
                            Console.WriteLine($"Subject: {task.Subject}");
                            Console.WriteLine($"Due Date: {task.DueDate}");
                            // The task object is now instantiated in memory and can be used further.
                        }
                    }
                    else
                    {
                        Console.WriteLine("The MSG file does not contain a task.");
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
