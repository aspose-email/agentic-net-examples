using System;
using Aspose.Email;
using Aspose.Email.Calendar;
using Aspose.Email.Clients.Exchange.WebService;

class Program
{
    static void Main()
    {
        try
        {
            // Create a new Exchange task with 0% completion
            ExchangeTask task = new ExchangeTask();
            task.Subject = "Sample Task";
            task.Body = "This is a sample task created with Aspose.Email.";
            task.PercentComplete = 0;

            Console.WriteLine($"Aspose.Email.Calendar.Task created. Subject: {task.Subject}, PercentComplete: {task.PercentComplete}%");

            // Update the task to 100% completion
            task.PercentComplete = 100;

            Console.WriteLine($"Aspose.Email.Calendar.Task updated. Subject: {task.Subject}, PercentComplete: {task.PercentComplete}%");
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
