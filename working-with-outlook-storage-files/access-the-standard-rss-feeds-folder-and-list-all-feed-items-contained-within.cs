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
            const string pstPath = "sample.pst";

            // Ensure PST file exists; create a minimal one if missing
            if (!File.Exists(pstPath))
            {
                try
                {
                    PersonalStorage.Create(pstPath, FileFormatVersion.Unicode);
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to create PST file: {ex.Message}");
                    return;
                }
            }

            // Open the PST file
            using (PersonalStorage pst = PersonalStorage.FromFile(pstPath))
            {
                // Access the standard RSS Feeds folder
                FolderInfo rssFolder;
                try
                {
                    rssFolder = pst.GetPredefinedFolder(StandardIpmFolder.RssFeeds);
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to get RSS folder: {ex.Message}");
                    return;
                }

                // Enumerate all messages (feed items) in the RSS folder
                foreach (MapiMessage mapiMsg in rssFolder.EnumerateMapiMessages())
                {
                    try
                    {
                        // Convert MAPI message to MailMessage for easy access
                        MailMessage mailMsg = mapiMsg.ToMailMessage(null);
                        Console.WriteLine($"Subject: {mailMsg.Subject}");
                        Console.WriteLine($"From: {mailMsg.From}");
                        Console.WriteLine($"Date: {mailMsg.Date}");
                        Console.WriteLine($"Body Preview: {mailMsg.Body?.Substring(0, Math.Min(100, mailMsg.Body?.Length ?? 0))}");
                        Console.WriteLine(new string('-', 40));
                    }
                    catch (Exception ex)
                    {
                        Console.Error.WriteLine($"Error processing a message: {ex.Message}");
                    }
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
