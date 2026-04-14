using System;
using System.Collections.Generic;
using System.Net;
using Aspose.Email;
using Aspose.Email.Calendar;
using Aspose.Email.Clients.Exchange.WebService;

class Program
{
    static void Main()
    {
        try
        {
            // Initialize EWS client (replace with actual server URI and credentials)
            string mailboxUri = "https://exchange.example.com/EWS/Exchange.asmx";

            // Skip external calls when placeholder credentials are used
            if (mailboxUri.Contains("example.com"))
            {
                Console.Error.WriteLine("Placeholder credentials detected. Skipping external calls.");
                return;
            }

            ICredentials credentials = new NetworkCredential("username", "password");

            using (IEWSClient client = EWSClient.GetEWSClient(mailboxUri, credentials))
            {
                // Specify the target calendar folder URI (replace with actual folder URI if needed)
                string calendarFolderUri = client.CurrentCalendarFolderUri;

                // Retrieve all appointments from the specified calendar folder
                Appointment[] appointments = client.ListAppointments(calendarFolderUri);

                // Determine cutoff date (appointments older than 5 years)
                DateTime cutoffDate = DateTime.Now.AddYears(-5);

                // Collect URIs (UniqueId) of appointments to delete
                List<string> itemsToDelete = new List<string>();
                foreach (Appointment appt in appointments)
                {
                    if (appt.StartDate < cutoffDate)
                    {
                        // Use UniqueId as the item identifier for deletion
                        if (!string.IsNullOrEmpty(appt.UniqueId))
                        {
                            itemsToDelete.Add(appt.UniqueId);
                        }
                    }
                }

                // Perform batch deletion if there are items to remove
                if (itemsToDelete.Count > 0)
                {
                    DeletionOptions deleteOptions = new DeletionOptions(DeletionType.MoveToDeletedItems);
                    client.DeleteItems(itemsToDelete, deleteOptions);
                    Console.WriteLine($"{itemsToDelete.Count} appointment(s) older than five years were deleted.");
                }
                else
                {
                    Console.WriteLine("No appointments older than five years were found.");
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
