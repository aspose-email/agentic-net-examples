using Aspose.Email;
using System;
using System.IO;
using Aspose.Email.Mapi;

// Author: Aspose.Email example - load a MSG file and extract its task information
class Program
{
    static void Main()
    {
        // Path to the input MSG file containing a task
        string inputPath = "task.msg";

        // Ensure the file exists before attempting to load it
        if (!File.Exists(inputPath))
        {
                try
                {
                    using (MapiMessage placeholder = new MapiMessage(
                        "from@example.com",
                        "to@example.com",
                        "Placeholder Subject",
                        "Placeholder body."))
                    {
                        placeholder.Save(inputPath);
                    }
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Error creating placeholder MSG: {ex.Message}");
                    return;
                }

            Console.Error.WriteLine($"Input file not found: {inputPath}");
            return;
        }

        try
        {
            // Load the MSG file as a generic MAPI message
            MapiMessage msg = MapiMessage.Load(inputPath);

            // Verify that the message actually represents a task
            if (msg.SupportedType == MapiItemType.Task)
            {
                // Convert the generic message to a strongly‑typed MapiTask
                MapiTask task = (MapiTask)msg.ToMapiMessageItem();

                // Example processing: output key task properties
                Console.WriteLine($"Subject   : {task.Subject}");
                Console.WriteLine($"StartDate : {task.StartDate}");
                Console.WriteLine($"DueDate   : {task.DueDate}");
            }
            else
            {
                Console.Error.WriteLine("The loaded MSG file does not contain a task item.");
            }
        }
        catch (Exception ex)
        {
            // Report any errors without throwing
            Console.Error.WriteLine($"Error processing the MSG file: {ex.Message}");
        }
    }
}
