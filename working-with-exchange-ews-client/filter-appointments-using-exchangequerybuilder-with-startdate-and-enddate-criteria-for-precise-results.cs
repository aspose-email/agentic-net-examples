using System;
using Aspose.Email;
using Aspose.Email.Calendar;
using Aspose.Email.Clients.Exchange;
using Aspose.Email.Clients.Exchange.WebService;
using Aspose.Email.Tools.Search;

class Program
{
    static void Main(string[] args)
    {
        try
        {
            // EWS connection parameters
            string mailboxUri = "https://mail.example.com/EWS/Exchange.asmx";
            string username = "user@example.com";
            string password = "password";


            // Skip external calls when placeholder credentials are used
            if (mailboxUri.Contains("example.com") || username.Contains("example.com") || password == "password")
            {
                Console.Error.WriteLine("Placeholder credentials detected. Skipping external calls.");
                return;
            }

            // Create EWS client safely
            using (IEWSClient client = EWSClient.GetEWSClient(mailboxUri, username, password))
            {
                // Define the date range for filtering
                DateTime startDate = new DateTime(2023, 1, 1);
                DateTime endDate   = new DateTime(2023, 12, 31);

                // Build the query using ExchangeQueryBuilder
                ExchangeQueryBuilder builder = new ExchangeQueryBuilder();
                builder.Appointment.Start.On(startDate);   // appointments starting on or after startDate
                builder.Appointment.End.On(endDate);       // appointments ending on or before endDate

                MailQuery query = builder.GetQuery();

                // Retrieve appointments that match the query from the default calendar folder
                Appointment[] appointments = client.ListAppointments(query);

                // Output basic information about each appointment
                foreach (Appointment appt in appointments)
                {
                    Console.WriteLine($"{appt.Summary} : {appt.StartDate:g} - {appt.EndDate:g}");
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
