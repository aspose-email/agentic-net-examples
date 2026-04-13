using System;
using System.IO;
using System.Net;
using Aspose.Email;
using Aspose.Email.Clients.Exchange.WebService;
using Aspose.Email.Calendar;

class Program
{
    static void Main()
    {
        try
        {
            // Define server connection parameters
            string ewsUrl = "https://exchange.example.com/EWS/Exchange.asmx";
            ICredentials credentials = new NetworkCredential("username", "password");

            // Create EWS client
            using (IEWSClient client = EWSClient.GetEWSClient(ewsUrl, credentials))
            {
                try
                {
                    // Retrieve tasks from the default tasks folder
                    TaskCollection tasks = client.ListTasks();

                    // Prepare output file path
                    string outputPath = "tasks_ids.txt";

                    // Skip external calls when placeholder credentials are used
                    if (ewsUrl.Contains("example.com"))
                    {
                        Console.Error.WriteLine("Placeholder credentials detected. Skipping external calls.");
                        return;
                    }

                    string outputDir = Path.GetDirectoryName(outputPath);
                    if (!string.IsNullOrEmpty(outputDir) && !Directory.Exists(outputDir))
                    {
                        Directory.CreateDirectory(outputDir);
                    }

                    // Write unique identifiers to the file
                    try
                    {
                        using (StreamWriter writer = new StreamWriter(outputPath))
                        {
                            foreach (Task task in tasks)
                            {
                                writer.WriteLine(task.UniqueId);
                            }
                        }
                    }
                    catch (Exception ioEx)
                    {
                        Console.Error.WriteLine($"File I/O error: {ioEx.Message}");
                        return;
                    }
                }
                catch (Exception clientEx)
                {
                    Console.Error.WriteLine($"EWS operation error: {clientEx.Message}");
                    return;
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
