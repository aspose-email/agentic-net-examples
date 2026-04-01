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
            string inputMsgPath = "input.msg";
            string outputTaskPath = "task.msg";

            // Ensure input MSG exists; create a minimal placeholder if missing
            if (!File.Exists(inputMsgPath))
            {
                try
                {
                    using (MapiMessage placeholder = new MapiMessage("Placeholder Subject", "Placeholder Body", "sender@example.com", "recipient@example.com"))
                    {
                        placeholder.Save(inputMsgPath);
                    }
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Error creating placeholder MSG: {ex.Message}");
                    return;
                }
            }

            // Load the original message
            MapiMessage originalMsg;
            try
            {
                originalMsg = MapiMessage.Load(inputMsgPath);
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Error loading MSG file: {ex.Message}");
                return;
            }

            // Create a new task message based on the original
            using (MapiMessage taskMsg = new MapiMessage())
            {
                // Set task-specific properties
                taskMsg.MessageClass = "IPM.Task";
                taskMsg.Subject = $"Task: {originalMsg.Subject}";
                taskMsg.Body = originalMsg.Body;

                // Optionally copy categories if present
                if (originalMsg.Categories != null && originalMsg.Categories.Length > 0)
                {
                    taskMsg.Categories = originalMsg.Categories;
                }

                // Save the task message
                try
                {
                    taskMsg.Save(outputTaskPath);
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Error saving task MSG: {ex.Message}");
                    return;
                }
            }

            Console.WriteLine($"Task message created successfully at '{outputTaskPath}'.");
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
