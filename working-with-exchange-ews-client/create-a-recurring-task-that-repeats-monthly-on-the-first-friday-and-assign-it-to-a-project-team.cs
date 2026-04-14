using System;
using System.Net;
using Aspose.Email;
using Aspose.Email.Clients.Exchange.WebService;
using Aspose.Email.Calendar.Recurrences;

namespace AsposeEmailEwsTaskExample
{
    class Program
    {
        static void Main()
        {
            try
            {
                // Initialize EWS client
                IEWSClient client;
                try
                {
                    string mailboxUri = "https://exchange.example.com/EWS/Exchange.asmx";
                    string username = "username";
                    string password = "password";


                    // Skip external calls when placeholder credentials are used
                    if (mailboxUri.Contains("example.com") || username == "username" || password == "password")
                    {
                        Console.Error.WriteLine("Placeholder credentials detected. Skipping external calls.");
                        return;
                    }

                    client = EWSClient.GetEWSClient(mailboxUri, new NetworkCredential(username, password));
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine("Failed to connect to Exchange: " + ex.Message);
                    return;
                }

                using (client)
                {
                    // Create a new task
                    ExchangeTask task = new ExchangeTask();
                    task.Subject = "Monthly Project Team Task";
                    task.Body = "This task recurs on the first Friday of each month and is assigned to the project team.";
                    task.StartDate = DateTime.Today;
                    task.DueDate = DateTime.Today.AddDays(7);

                    // Assign the task to the project team
                    task.Attendees = new MailAddressCollection();
                    task.Attendees.Add(new MailAddress("member1@domain.com"));
                    task.Attendees.Add(new MailAddress("member2@domain.com"));
                    task.Attendees.Add(new MailAddress("member3@domain.com"));

                    // Define recurrence: first Friday of every month
                    MonthlyRecurrencePattern recurrence = new MonthlyRecurrencePattern(
                        DayPosition.First,   // first occurrence in the month
                        CalendarDay.Friday,  // Friday
                        1);                  // repeat every 1 month
                    task.RecurrencePattern = recurrence;

                    // Create the task on the server
                    try
                    {
                        string taskUri = client.CreateTask(task);
                        Console.WriteLine("Aspose.Email.Calendar.Task created successfully. URI: " + taskUri);
                    }
                    catch (Exception ex)
                    {
                        Console.Error.WriteLine("Failed to create task: " + ex.Message);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine("Unexpected error: " + ex.Message);
            }
        }
    }
}
