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
            string msgPath = "input.msg";

            if (!File.Exists(msgPath))
{
    try
    {
        MailMessage placeholderMsg = new MailMessage("sender@example.com", "recipient@example.com", "Placeholder", "This is a placeholder MSG.");
        placeholderMsg.Save(msgPath, SaveOptions.DefaultMsgUnicode);
    }
    catch (Exception ex)
    {
        Console.Error.WriteLine($"Failed to create placeholder MSG: {ex.Message}");
        return;
    }
}


            using (MapiMessage msg = MapiMessage.Load(msgPath))
            {
                // Create a new task based on the message content
                using (MapiTask task = new MapiTask())
                {
                    task.Subject = msg.Subject;
                    task.Body = msg.Body;

                    // Example: set task start and due dates to now and tomorrow
                    task.StartDate = DateTime.Now;
                    task.DueDate = DateTime.Now.AddDays(1);

                    Console.WriteLine("Task created successfully:");
                    Console.WriteLine($"Subject: {task.Subject}");
                    Console.WriteLine($"Body: {task.Body}");
                    Console.WriteLine($"Start Date: {task.StartDate}");
                    Console.WriteLine($"Due Date: {task.DueDate}");
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
