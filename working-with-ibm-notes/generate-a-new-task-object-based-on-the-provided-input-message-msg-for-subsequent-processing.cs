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
            // Path to the input MSG file
            string msgPath = "input.msg";

            // Verify that the file exists before attempting to load it
            if (!File.Exists(msgPath))
            {
                Console.Error.WriteLine($"Error: File not found – {msgPath}");
                return;
            }

            // Load the MSG file into a MapiMessage instance
            using (MapiMessage msg = MapiMessage.Load(msgPath))
            {
                // Create a new task based on the message content
                MapiTask task = new MapiTask();

                // Transfer basic properties from the message to the task
                task.Subject = msg.Subject;
                task.Body = msg.Body;

                // Set example dates for the task
                task.StartDate = DateTime.Now;
                task.DueDate = DateTime.Now.AddDays(7);

                // Output the created task details
                Console.WriteLine("Task created from message:");
                Console.WriteLine($"Subject: {task.Subject}");
                Console.WriteLine($"Body: {task.Body}");
                Console.WriteLine($"Start Date: {task.StartDate}");
                Console.WriteLine($"Due Date: {task.DueDate}");
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
