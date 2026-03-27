using System;
using System.Net;
using Aspose.Email;
using Aspose.Email.Clients.Exchange.WebService;
using Aspose.Email.Tools.Search;

namespace AsposeEmailEwsTaskSample
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                // EWS service URL and credentials (replace with real values)
                string ewsUrl = "https://exchange.example.com/EWS/Exchange.asmx";
                NetworkCredential credentials = new NetworkCredential("username", "password");

                // Create and connect the EWS client
                try
                {
                    using (IEWSClient client = EWSClient.GetEWSClient(ewsUrl, credentials))
                    {
                        // -------------------- Create a task --------------------
                        using (ExchangeTask newTask = new ExchangeTask())
                        {
                            newTask.Subject = "Sample Task";
                            newTask.Body = "This is a sample task created via EWS.";
                            newTask.DueDate = DateTime.Now.AddDays(7);
                            newTask.Priority = MailPriority.High; // Correct enum usage

                            string taskUri = client.CreateTask(newTask);
                            Console.WriteLine($"Created task URI: {taskUri}");

                            // -------------------- Retrieve the task --------------------
                            using (ExchangeTask fetchedTask = client.FetchTask(taskUri))
                            {
                                Console.WriteLine($"Fetched task subject: {fetchedTask.Subject}");

                                // -------------------- Update the task --------------------
                                fetchedTask.Body = "Updated body of the task.";
                                fetchedTask.Priority = MailPriority.Normal;
                                client.UpdateTask(fetchedTask);
                                Console.WriteLine("Task updated.");

                                // -------------------- Delete the task --------------------
                                client.DeleteItem(taskUri, DeletionOptions.DeletePermanently);
                                Console.WriteLine("Task deleted.");
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"EWS operation failed: {ex.Message}");
                    return;
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Unexpected error: {ex.Message}");
            }
        }
    }
}
