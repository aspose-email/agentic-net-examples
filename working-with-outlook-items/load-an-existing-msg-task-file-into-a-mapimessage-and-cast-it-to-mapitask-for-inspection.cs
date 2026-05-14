using System;
using System.IO;
using Aspose.Email;
using Aspose.Email.Mapi;

class Program
{
    static void Main()
    {
        try
        {
            // Path to the MSG task file
            string msgFilePath = "task.msg";

            // Verify that the file exists before attempting to load it
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

                Console.Error.WriteLine("File not found: " + msgFilePath);
                return;
            }

            // Load the MSG file into a MapiMessage inside a using block for proper disposal
            using (MapiMessage msg = MapiMessage.Load(msgFilePath))
            {
                // Ensure the loaded message represents a task
                if (msg.SupportedType == MapiItemType.Task)
                {
                    // Convert the MapiMessage to a MapiTask and dispose it after use
                    using (MapiTask task = (MapiTask)msg.ToMapiMessageItem())
                    {
                        // Display selected task properties
                        Console.WriteLine("Subject: " + task.Subject);
                        Console.WriteLine("Due Date: " + task.DueDate);
                        Console.WriteLine("Start Date: " + task.StartDate);
                        Console.WriteLine("Percent Complete: " + task.PercentComplete);
                        Console.WriteLine("Status: " + task.Status);
                    }
                }
                else
                {
                    Console.WriteLine("The specified MSG file does not contain a task.");
                }
            }
        }
        catch (Exception ex)
        {
            // Output any unexpected errors without crashing the application
            Console.Error.WriteLine("Error: " + ex.Message);
        }
    }
}
