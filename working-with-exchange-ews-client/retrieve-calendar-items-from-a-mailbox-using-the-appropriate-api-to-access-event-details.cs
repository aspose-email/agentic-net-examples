using System;
using System.Net;
using Aspose.Email;
using Aspose.Email.Calendar;
using Aspose.Email.Clients.Exchange.WebService;

namespace AsposeEmailExample
{
    class Program
    {
        static void Main()
        {
            try
            {
                // Prepare credentials and service URL (replace with real values)
                NetworkCredential credentials = new NetworkCredential("username", "password");
                string serviceUrl = "https://exchange.example.com/EWS/Exchange.asmx";

                // Create and use the EWS client
                using (IEWSClient client = EWSClient.GetEWSClient(serviceUrl, credentials))
                {
                    // Retrieve appointments from the default calendar folder
                    Appointment[] appointments = client.ListAppointments();

                    // Output basic details of each appointment
                    foreach (Appointment appointment in appointments)
                    {
                        Console.WriteLine($"Subject : {appointment.Summary}");
                        Console.WriteLine($"Start   : {appointment.StartDate}");
                        Console.WriteLine($"End     : {appointment.EndDate}");
                        Console.WriteLine($"Location: {appointment.Location}");
                        Console.WriteLine(new string('-', 40));
                    }
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Error: {ex.Message}");
            }
        }
    }
}
