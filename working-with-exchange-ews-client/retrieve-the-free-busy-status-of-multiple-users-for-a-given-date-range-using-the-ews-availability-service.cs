using System;
using System.Collections.Specialized;
using Aspose.Email;
using Aspose.Email.Clients.Exchange.WebService;

namespace AsposeEmailEwsAvailabilitySample
{
    class Program
    {
        static void Main()
        {
            try
            {
                // Mailbox URI and credentials (replace with real values)
                string mailboxUri = "https://exchange.example.com/EWS/Exchange.asmx";
                string username = "user@example.com";
                string password = "password";


                // Skip external calls when placeholder credentials are used
                if (mailboxUri.Contains("example.com") || username.Contains("example.com") || password == "password")
                {
                    Console.Error.WriteLine("Placeholder credentials detected. Skipping external calls.");
                    return;
                }

                // Create EWS client
                using (IEWSClient client = EWSClient.GetEWSClient(mailboxUri, username, password))
                {
                    // List of users to query
                    StringCollection users = new StringCollection();
                    users.Add("alice@example.com");
                    users.Add("bob@example.com");

                    // Define the time window (e.g., today)
                    DateTime startDate = DateTime.Today;
                    DateTime endDate = startDate.AddDays(1);
                    DateRange timeWindow = new DateRange(startDate, endDate);

                    // Retrieve availability for the users
                    ExchangeUserAvailabilityCollection availabilityCollection = client.CheckUserAvailability(users, timeWindow);

                    // Iterate through each user's availability information
                    foreach (ExchangeUserAvailability availability in availabilityCollection)
                    {
                        Console.WriteLine($"User: {availability.User}");

                        // Working hours collection for the user
                        foreach (ExchangeUserWorkingHours workingHours in availability.WorkingHours)
                        {
                            Console.WriteLine($"  Day: {workingHours.DayOfWeek}");
                            Console.WriteLine($"    Start: {workingHours.StartTime}");
                            Console.WriteLine($"    End: {workingHours.EndTime}");
                        }

                        // Example: Get working hours for a specific date
                        DateRange dayHours = availability.GetWorkingHours(startDate);
                        Console.WriteLine($"  Working hours on {startDate:d}: {dayHours.StartTime} - {dayHours.EndTime}");
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
