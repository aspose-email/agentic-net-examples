using Aspose.Email;
using System;
using System.Collections.Generic;
using Aspose.Email.Calendar;

class Program
{
    static void Main()
    {
        try
        {
            // Prepare a collection of tasks for validation
            List<Task> tasks = new List<Task>();

            // Example task with a future due date
            Task futureTask = new Task();
            futureTask.Subject = "Future Task";
            futureTask.DueDate = DateTime.Now.AddDays(5);
            tasks.Add(futureTask);

            // Example task with a past due date
            Task pastTask = new Task();
            pastTask.Subject = "Past Task";
            pastTask.DueDate = DateTime.Now.AddDays(-2);
            tasks.Add(pastTask);

            // Validate each task's due date
            foreach (Task task in tasks)
            {
                using (task)
                {
                    if (task.DueDate < DateTime.Now)
                    {
                        Console.WriteLine($"Task '{task.Subject}' has a past due date: {task.DueDate}");
                    }
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine(ex.Message);
        }
    }
}
