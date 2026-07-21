using System;
using System.IO;
using Aspose.Email;
using Aspose.Email.Mapi;
using Aspose.Email.Storage.Pst;
using Aspose.Email.Tools.Search;

namespace DateRangeFilterExample
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                // Path to the PST file
                string pstFilePath = "sample.pst";

                // If the PST file does not exist, create a minimal one with a sample message
                if (!File.Exists(pstFilePath))
                {
                    using (PersonalStorage pst = PersonalStorage.Create(pstFilePath, FileFormatVersion.Unicode))
                    {
                        // Create a subfolder (Inbox) and add a sample message
                        FolderInfo inbox = pst.RootFolder.AddSubFolder("Inbox");
                        MailMessage sampleMessage = new MailMessage(
                            "sender@example.com",
                            "recipient@example.com",
                            "Sample Subject",
                            "This is a sample email body.")
                        {
                            // Set the sent date using the Date property
                            Date = new DateTime(2023, 6, 15)
                        };
                        inbox.AddMessage(MapiMessage.FromMailMessage(sampleMessage));
                    }

                    Console.WriteLine($"Created placeholder PST file at '{pstFilePath}'.");
                }

                // Define start and end dates for the filter
                DateTime startDate = new DateTime(2023, 1, 1);
                DateTime endDate = new DateTime(2023, 12, 31);

                // Build a MailQuery that selects messages with SentDate >= startDate AND SentDate < endDate
                MailQueryBuilder queryBuilder = new MailQueryBuilder();
                queryBuilder.SentDate.Since(startDate);
                queryBuilder.SentDate.Before(endDate);
                MailQuery dateRangeQuery = queryBuilder.GetQuery();

                // Load the PST file and enumerate messages matching the query
                using (PersonalStorage pstStorage = PersonalStorage.FromFile(pstFilePath))
                {
                    // Use the root folder (or any subfolder as needed)
                    FolderInfo rootFolder = pstStorage.RootFolder;

                    foreach (MessageInfo messageInfo in rootFolder.EnumerateMessages(dateRangeQuery))
                    {
                        Console.WriteLine($"Subject: {messageInfo.Subject}");
                        // SentDate property may not be available in all versions; output placeholder if not
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
