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
            string pstPath = "sample.pst";
            string csvPath = "messages.csv";

            // Ensure PST file exists; create a minimal one if missing
            if (!File.Exists(pstPath))
            {
                try
                {
                    // Create an empty Unicode PST file
                    PersonalStorage.Create(pstPath, FileFormatVersion.Unicode);
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to create placeholder PST: {ex.Message}");
                    return;
                }
            }

            // Prepare CSV file (overwrite if exists)
            try
            {
                using (StreamWriter csvWriter = new StreamWriter(csvPath, false))
                {
                    // Write CSV header
                    csvWriter.WriteLine("MessageId,Subject,SenderEmail");

                    // Open PST and process messages
                    using (PersonalStorage pst = PersonalStorage.FromFile(pstPath))
                    {
                        // Get the Inbox folder (standard IPM folder)
                        FolderInfo inboxFolder = pst.GetPredefinedFolder(StandardIpmFolder.Inbox);
                        foreach (MessageInfo messageInfo in inboxFolder.EnumerateMessages())
                        {
                            using (MapiMessage msg = pst.ExtractMessage(messageInfo))
                            {
                                string messageId = msg.InternetMessageId ?? string.Empty;
                                string subject = msg.Subject ?? string.Empty;
                                string senderEmail = msg.SenderEmailAddress ?? string.Empty;

                                // Escape commas in fields
                                messageId = $"\"{messageId.Replace("\"", "\"\"")}\"";
                                subject = $"\"{subject.Replace("\"", "\"\"")}\"";
                                senderEmail = $"\"{senderEmail.Replace("\"", "\"\"")}\"";

                                csvWriter.WriteLine($"{messageId},{subject},{senderEmail}");
                            }
                        }
                    }
                }

                Console.WriteLine($"CSV file generated at: {csvPath}");
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"File operation error: {ex.Message}");
                return;
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
