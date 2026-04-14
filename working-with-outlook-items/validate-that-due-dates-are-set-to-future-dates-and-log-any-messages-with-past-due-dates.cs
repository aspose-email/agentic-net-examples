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
            List<Aspose.Email.Calendar.Task> tasks = new List<Aspose.Email.Calendar.Task>();

            // Example task with a future due date
            Aspose.Email.Calendar.Task futureTask = new Aspose.Email.Calendar.Task();
            futureTask.Subject = "Future Task";
            futureTask.DueDate = DateTime.Now.AddDays(5);
            tasks.Add(futureTask);

            // Example task with a past due date
            Aspose.Email.Calendar.Task pastTask = new Aspose.Email.Calendar.Task();
            pastTask.Subject = "Past Task";
            pastTask.DueDate = DateTime.Now.AddDays(-2);
            tasks.Add(pastTask);

            // Validate each task's due date
            foreach (Aspose.Email.Calendar.Task task in tasks)
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
