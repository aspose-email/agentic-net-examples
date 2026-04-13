using System;
using System.IO;
using Aspose.Email;
using Aspose.Email.Mapi;

namespace AsposeEmailTaskFilter
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                string msgFolder = @"C:\MsgFiles";

                if (!Directory.Exists(msgFolder))
                {
                    Console.Error.WriteLine($"Error: Directory not found – {msgFolder}");
                    return;
                }

                string[] msgFiles = Directory.GetFiles(msgFolder, "*.msg");

                foreach (string filePath in msgFiles)
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

                        Console.Error.WriteLine($"Error: File not found – {filePath}");
                        continue;
                    }

                    try
                    {
                        using (MapiMessage msg = MapiMessage.Load(filePath))
                        {
                            if (msg.SupportedType == MapiItemType.Task)
                            {
                                using (MapiTask task = (MapiTask)msg.ToMapiMessageItem())
                                {
                                    if (!string.IsNullOrEmpty(task.Subject) && task.Subject.IndexOf("Project", StringComparison.OrdinalIgnoreCase) >= 0)
                                    {
                                        Console.WriteLine($"Task found in file '{Path.GetFileName(filePath)}': {task.Subject}");
                                    }
                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.Error.WriteLine($"Error processing file {filePath}: {ex.Message}");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Unexpected error: {ex.Message}");
            }
        }
    }
}
