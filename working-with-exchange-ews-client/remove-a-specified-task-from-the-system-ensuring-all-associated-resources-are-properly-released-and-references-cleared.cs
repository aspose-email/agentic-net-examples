using System;
using System.Net;
using Aspose.Email;
using Aspose.Email.Clients;
using Aspose.Email.Clients.Exchange.WebService;

class Program
{
    // Author: Aspose.Email example for deleting a task via EWS
    static void Main(string[] args)
    {
        try
        {
            // Validate input task URI
            if (args.Length == 0 || string.IsNullOrWhiteSpace(args[0]))
            {
                Console.Error.WriteLine("Please provide the task URI as the first argument.");
                return;
            }

            string taskUri = args[0];

            // EWS service endpoint and credentials
            string ewsUrl = "https://example.com/EWS/Exchange.asmx";

            // Skip external calls when placeholder credentials are used
            if (ewsUrl.Contains("example.com"))
            {
                Console.Error.WriteLine("Placeholder credentials detected. Skipping external calls.");
                return;
            }

            ICredentials credentials = new NetworkCredential("username", "password");

            // Create EWS client (implements IEWSClient and IDisposable)
            using (IEWSClient client = EWSClient.GetEWSClient(ewsUrl, credentials))
            {
                try
                {
                    // Delete the task permanently using synchronous method
                    client.DeleteItem(taskUri, DeletionOptions.DeletePermanently);
                    Console.WriteLine("Task deleted successfully.");
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Error deleting task: {ex.Message}");
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
