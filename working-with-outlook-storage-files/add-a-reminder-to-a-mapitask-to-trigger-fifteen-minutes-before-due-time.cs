using System;
using Aspose.Email;
using Aspose.Email.Mapi;

class Program
{
    static void Main()
    {
        try
        {
            // Define start and due dates for the task
            DateTime startDate = DateTime.Now;
            DateTime dueDate = startDate.AddHours(2);

            // Create a new MapiTask instance
            using (MapiTask task = new MapiTask("Sample Task", "Task body", startDate, dueDate))
            {
                // Enable the reminder
                task.ReminderSet = true;

                // Set the reminder to trigger 15 minutes before the due date
                task.ReminderTime = task.DueDate.AddMinutes(-15);

                // Output reminder details to the console
                Console.WriteLine("Reminder set: " + task.ReminderSet);
                Console.WriteLine("Reminder will trigger at: " + task.ReminderTime);
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine(ex.Message);
        }
    }
}
