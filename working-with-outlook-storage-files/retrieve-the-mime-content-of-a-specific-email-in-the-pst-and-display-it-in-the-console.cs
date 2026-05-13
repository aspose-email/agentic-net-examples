using System;
using System.IO;
using Aspose.Email;
using Aspose.Email.Mapi;
using Aspose.Email.Storage.Pst;

class Program
{
    static void Main()
    {
        try
        {
            // Path to the PST file (placeholder if not present)
            string pstFilePath = "sample.pst";

            // Ensure a PST file exists; create a minimal placeholder if missing
            if (!File.Exists(pstFilePath))
            {
                using (PersonalStorage pst = PersonalStorage.Create(pstFilePath, FileFormatVersion.Unicode))
                {
                    // Create a simple Inbox folder
                    pst.RootFolder.AddSubFolder("Inbox");
                }

                Console.WriteLine($"Placeholder PST created at '{pstFilePath}'. Add messages and rerun the program.");
                return;
            }

            // Subject of the email to retrieve
            string targetSubject = "Target Email Subject";

            // Open the PST file
            using (PersonalStorage pst = PersonalStorage.FromFile(pstFilePath))
            {
                // Get the Inbox folder (created earlier)
                FolderInfo inboxFolder = pst.RootFolder.GetSubFolder("Inbox");

                // Enumerate messages in the Inbox
                foreach (MessageInfo messageInfo in inboxFolder.EnumerateMessages())
                {
                    // Check if this is the message we are looking for
                    if (string.Equals(messageInfo.Subject, targetSubject, StringComparison.OrdinalIgnoreCase))
                    {
                        // Extract the full message as a MapiMessage
                        MapiMessage mapiMessage = pst.ExtractMessage(messageInfo);

                        // Display MIME-like content: headers and body
                        Console.WriteLine("=== MIME Content ===");
                        Console.WriteLine("Headers:");
                        Console.WriteLine(mapiMessage.TransportMessageHeaders ?? "(no headers)");
                        Console.WriteLine();
                        Console.WriteLine("Body:");
                        Console.WriteLine(mapiMessage.Body ?? "(no body)");
                        Console.WriteLine("====================");

                        // Message found; exit the loop
                        break;
                    }
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
