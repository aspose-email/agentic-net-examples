using System;
using System.IO;
using System.Net;
using Aspose.Email;
using Aspose.Email.Clients.Exchange.WebService;
using Aspose.Email.Calendar;

namespace AsposeEmailSample
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                // Configuration
                string exchangeUrl = "https://exchange.example.com/EWS/Exchange.asmx";
                string username = "user@example.com";
                string password = "password";
                string targetLocation = "Conference Room A";
                string outputFilePath = "appointments_start_times.txt";


                // Skip external calls when placeholder credentials are used
                if (exchangeUrl.Contains("example.com") || username.Contains("example.com") || password == "password")
                {
                    Console.Error.WriteLine("Placeholder credentials detected. Skipping external calls.");
                    return;
                }

                // Ensure output directory exists
                try
                {
                    string outputDirectory = Path.GetDirectoryName(outputFilePath);
                    if (!string.IsNullOrEmpty(outputDirectory) && !Directory.Exists(outputDirectory))
                    {
                        Directory.CreateDirectory(outputDirectory);
                    }
                }
                catch (Exception dirEx)
                {
                    Console.Error.WriteLine($"Directory error: {dirEx.Message}");
                    return;
                }

                // Create EWS client
                try
                {
                    using (IEWSClient client = EWSClient.GetEWSClient(exchangeUrl, new NetworkCredential(username, password)))
                    {
                        // Retrieve all appointments from the default calendar folder
                        Appointment[] appointments = client.ListAppointments();

                        // Write start times of appointments with the specified location
                        try
                        {
                            using (StreamWriter writer = new StreamWriter(outputFilePath, false))
                            {
                                foreach (Appointment appointment in appointments)
                                {
                                    if (appointment != null &&
                                        appointment.Location != null &&
                                        appointment.Location.Equals(targetLocation, StringComparison.OrdinalIgnoreCase))
                                    {
                                        writer.WriteLine(appointment.StartDate.ToString("o"));
                                    }
                                }
                            }
                        }
                        catch (Exception writeEx)
                        {
                            Console.Error.WriteLine($"File write error: {writeEx.Message}");
                            return;
                        }
                    }
                }
                catch (Exception clientEx)
                {
                    Console.Error.WriteLine($"EWS client error: {clientEx.Message}");
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
