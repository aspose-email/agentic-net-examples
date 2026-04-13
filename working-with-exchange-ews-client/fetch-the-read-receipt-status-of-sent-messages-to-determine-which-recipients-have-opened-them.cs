using System;
using System.Net;
using Aspose.Email;
using Aspose.Email.Clients.Exchange.WebService;

class Program
{
    static void Main()
    {
        try
        {
            // Connection parameters (replace with real values)
            string mailboxUri = "https://mail.example.com/EWS/Exchange.asmx";
            string username = "user@example.com";
            string password = "password";

            // The Message-ID of the sent email to track (replace with real value)
            string messageId = "<unique-message-id@example.com>";


            // Skip external calls when placeholder credentials are used
            if (mailboxUri.Contains("example.com") || username.Contains("example.com") || password == "password" || messageId.Contains("example.com"))
            {
                Console.Error.WriteLine("Placeholder credentials detected. Skipping external calls.");
                return;
            }

            // Create EWS client
            using (IEWSClient client = EWSClient.GetEWSClient(mailboxUri, username, password))
            {
                // Find the tracking report ID for the specified message
                FindMessageTrackingReportOptions findOptions = new FindMessageTrackingReportOptions("Self", "example.com");
                findOptions.MessageId = messageId;

                MessageTrackingReportInfo[] foundReports = client.FindMessageTrackingReport(findOptions);
                if (foundReports == null || foundReports.Length == 0)
                {
                    Console.WriteLine("No tracking report found for the specified message.");
                    return;
                }

                string reportId = foundReports[0].MessageTrackingReportId;

                // Retrieve the detailed tracking report
                GetMessageTrackingReportOptions getOptions = new GetMessageTrackingReportOptions("Self", reportId);
                MessageTrackingReport report = client.GetMessageTrackingReport(getOptions);
                if (report == null)
                {
                    Console.WriteLine("Failed to retrieve the tracking report.");
                    return;
                }

                // Output read receipt / recipient events
                Console.WriteLine($"Subject: {report.Subject}");
                Console.WriteLine("Recipient tracking events:");
                foreach (RecipientTrackingEvent ev in report.RecipientTrackingEvents)
                {
                    Console.WriteLine($"Recipient: {ev.Recipient}");
                    Console.WriteLine($"Event: {ev.EventDescription}");
                    Console.WriteLine($"Date: {ev.Date}");
                    Console.WriteLine($"Delivery Status: {ev.DeliveryStatus}");
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
