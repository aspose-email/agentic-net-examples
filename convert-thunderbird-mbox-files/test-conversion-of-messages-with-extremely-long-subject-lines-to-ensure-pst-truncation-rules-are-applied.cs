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
            string pstPath = "LongSubjectTest.pst";

            // Ensure the PST file exists; create a new one if it does not.
            if (!File.Exists(pstPath))
            {
                try
                {
                    using (PersonalStorage pstCreate = PersonalStorage.Create(pstPath, FileFormatVersion.Unicode))
                    {
                        // Create the default Inbox folder.
                        pstCreate.CreatePredefinedFolder("Inbox", StandardIpmFolder.Inbox);
                    }
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to create PST file: {ex.Message}");
                    return;
                }
            }

            // Open the PST file.
            try
            {
                using (PersonalStorage pst = PersonalStorage.FromFile(pstPath))
                {
                    // Get the Inbox folder (creates it if missing).
                    FolderInfo inboxFolder;
                    try
                    {
                        inboxFolder = pst.GetPredefinedFolder(StandardIpmFolder.Inbox);
                    }
                    catch (Exception ex)
                    {
                        Console.Error.WriteLine($"Failed to get Inbox folder: {ex.Message}");
                        return;
                    }

                    // Construct an extremely long subject (e.g., 5000 characters).
                    string longSubject = new string('A', 5000);
                    string fromAddress = "sender@example.com";
                    string toAddress = "recipient@example.com";
                    string bodyText = "This is a test message with an extremely long subject line.";

                    // Create a MAPI message with the long subject.
                    MapiMessage message = new MapiMessage(fromAddress, toAddress, longSubject, bodyText);

                    // Add the message to the Inbox folder.
                    string entryId;
                    try
                    {
                        entryId = inboxFolder.AddMessage(message);
                    }
                    catch (Exception ex)
                    {
                        Console.Error.WriteLine($"Failed to add message to PST: {ex.Message}");
                        return;
                    }

                    // Extract the message back from the PST using the returned EntryId.
                    MapiMessage extractedMessage;
                    try
                    {
                        extractedMessage = pst.ExtractMessage(entryId);
                    }
                    catch (Exception ex)
                    {
                        Console.Error.WriteLine($"Failed to extract message from PST: {ex.Message}");
                        return;
                    }

                    // Output the original and extracted subject lengths.
                    Console.WriteLine($"Original subject length: {longSubject.Length}");
                    Console.WriteLine($"Extracted subject length: {extractedMessage.Subject?.Length ?? 0}");

                    // Optionally, display a truncated preview of the extracted subject.
                    if (extractedMessage.Subject != null && extractedMessage.Subject.Length > 100)
                    {
                        Console.WriteLine($"Extracted subject preview (first 100 chars): {extractedMessage.Subject.Substring(0, 100)}");
                    }
                    else
                    {
                        Console.WriteLine($"Extracted subject: {extractedMessage.Subject}");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Failed to process PST file: {ex.Message}");
                return;
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
