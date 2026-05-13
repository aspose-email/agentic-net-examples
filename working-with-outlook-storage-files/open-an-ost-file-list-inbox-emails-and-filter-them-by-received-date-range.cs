using System;
using System.IO;
using Aspose.Email;
using Aspose.Email.Storage.Pst;
using Aspose.Email.Mapi;

class Program
{
    static void Main()
    {
        try
        {
            // Path to the OST file
            string ostPath = "sample.ost";

            // Ensure the file exists; create a minimal placeholder if missing
            if (!File.Exists(ostPath))
            {
                try
                {
                    // Create an empty PST (usable as OST) file
                    using (PersonalStorage placeholder = PersonalStorage.Create(ostPath, FileFormatVersion.Unicode))
                    {
                        // No additional setup required
                    }
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to create placeholder OST file: {ex.Message}");
                    return;
                }
            }

            // Define the date range for filtering
            DateTime startDate = new DateTime(2023, 1, 1);
            DateTime endDate = new DateTime(2023, 12, 31);

            // Open the OST/PST file
            using (PersonalStorage pst = PersonalStorage.FromFile(ostPath))
            {
                // Get the Inbox folder
                FolderInfo inboxFolder;
                try
                {
                    inboxFolder = pst.GetPredefinedFolder(StandardIpmFolder.Inbox);
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to retrieve Inbox folder: {ex.Message}");
                    return;
                }

                // Enumerate messages in the Inbox
                foreach (MessageInfo messageInfo in inboxFolder.EnumerateMessages())
                {
                    // Extract the full MAPI message
                    MapiMessage mapiMessage = null;
                    try
                    {
                        mapiMessage = pst.ExtractMessage(messageInfo);
                    }
                    catch (Exception ex)
                    {
                        Console.Error.WriteLine($"Failed to extract message '{messageInfo.Subject}': {ex.Message}");
                        continue;
                    }

                    // Convert to MailMessage for easier date handling
                    MailMessage mailMessage = null;
                    try
                    {
                        mailMessage = mapiMessage.ToMailMessage(new MailConversionOptions());
                    }
                    catch (Exception ex)
                    {
                        Console.Error.WriteLine($"Failed to convert MAPI message to MailMessage: {ex.Message}");
                        mapiMessage.Dispose();
                        continue;
                    }

                    // Filter by received date
                    DateTime receivedDate = mailMessage.Date;
                    if (receivedDate >= startDate && receivedDate <= endDate)
                    {
                        Console.WriteLine($"Subject: {mailMessage.Subject}");
                        Console.WriteLine($"Received: {receivedDate}");
                        Console.WriteLine();
                    }

                    // Dispose resources
                    mailMessage.Dispose();
                    mapiMessage.Dispose();
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
