using System;
using Aspose.Email;
using Aspose.Email.Mapi;
using Aspose.Email.Clients.Exchange.WebService;

class Program
{
    static void Main()
    {
        try
        {
            // Placeholder credentials and endpoint
            string ewsUrl = "https://ews.example.com/EWS/Exchange.asmx";
            string username = "user@example.com";
            string password = "password";
            string domain = "";

            // Skip external call when using placeholder values
            if (ewsUrl.Contains("example.com"))
            {
                Console.Error.WriteLine("Placeholder EWS endpoint detected. Skipping external operations.");
                return;
            }

            // Create EWS client
            using (IEWSClient client = EWSClient.GetEWSClient(ewsUrl, username, password, domain))
            {
                // Create a new MAPI message
                using (MapiMessage message = new MapiMessage(
                    "sender@example.com",
                    "recipient@example.com",
                    "Project Update",
                    "Please review the attached project update."))
                {
                    // Add "Feedback" voting button
                    FollowUpManager.AddVotingButton(message, "Feedback");

                    // Set follow‑up flag with a one‑week due date
                    DateTime startDate = DateTime.Now;
                    DateTime dueDate = startDate.AddDays(7);
                    FollowUpManager.SetFlag(message, "Please provide feedback", startDate, dueDate);

                    // Create the item in the mailbox
                    client.CreateItem(message);
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
