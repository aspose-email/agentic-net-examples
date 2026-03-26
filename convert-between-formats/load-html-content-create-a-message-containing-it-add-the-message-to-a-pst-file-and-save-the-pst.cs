using System;
using System.IO;
using Aspose.Email.Storage.Pst;
using Aspose.Email.Mapi;

class Program
{
    static void Main()
    {
        try
        {
            // Define file paths
            string htmlFilePath = "message.html";
            string pstFilePath = "output.pst";

            // Ensure HTML file exists; create a minimal placeholder if missing
            if (!File.Exists(htmlFilePath))
            {
                try
                {
                    File.WriteAllText(htmlFilePath, "<html><body><p>Placeholder content</p></body></html>");
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to create placeholder HTML file: {ex.Message}");
                    return;
                }
            }

            // Ensure PST file exists; create a new PST if missing
            if (!File.Exists(pstFilePath))
            {
                try
                {
                    using (PersonalStorage pstCreator = PersonalStorage.Create(pstFilePath, FileFormatVersion.Unicode))
                    {
                        // Create a default Inbox folder
                        pstCreator.CreatePredefinedFolder("Inbox", StandardIpmFolder.Inbox);
                    }
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to create PST file: {ex.Message}");
                    return;
                }
            }

            // Load HTML content
            string htmlContent;
            try
            {
                htmlContent = File.ReadAllText(htmlFilePath);
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Failed to read HTML file: {ex.Message}");
                return;
            }

            // Create a MapiMessage from the HTML content
            MapiMessage mapiMessage;
            try
            {
                mapiMessage = new MapiMessage(
                    "sender@example.com",
                    "recipient@example.com",
                    "Sample Subject",
                    htmlContent,
                    OutlookMessageFormat.Unicode);
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Failed to create MapiMessage: {ex.Message}");
                return;
            }

            // Add the message to the PST file
            try
            {
                using (PersonalStorage pst = PersonalStorage.FromFile(pstFilePath))
                {
                    // Retrieve the Inbox folder (or create if not present)
                    FolderInfo inboxFolder;
                    try
                    {
                        inboxFolder = pst.GetPredefinedFolder(StandardIpmFolder.Inbox);
                    }
                    catch
                    {
                        // Fallback: create Inbox folder manually
                        pst.CreatePredefinedFolder("Inbox", StandardIpmFolder.Inbox);
                        inboxFolder = pst.GetPredefinedFolder(StandardIpmFolder.Inbox);
                    }

                    // Add the message
                    string entryId = inboxFolder.AddMessage(mapiMessage);
                    Console.WriteLine($"Message added to PST. EntryId: {entryId}");
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Failed to add message to PST: {ex.Message}");
                return;
            }
            finally
            {
                // Dispose the MapiMessage explicitly
                if (mapiMessage != null)
                {
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