using Aspose.Email;
using System;
using Aspose.Email.Mapi;

class Program
{
    static void Main()
    {
        try
        {
            // Create a task and set its estimated effort in minutes.
            MapiTask task = new MapiTask();
            task.EstimatedEffort = 960; // Example: 960 minutes (16 hours).

            // Convert minutes to days assuming an 8‑hour workday.
            double days = (double)task.EstimatedEffort / (8 * 60);
            Console.WriteLine($"Estimated effort: {task.EstimatedEffort} minutes = {days} days");
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine(ex.Message);
        }
    }
}
