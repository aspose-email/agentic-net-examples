using System;
using System.IO;
using Aspose.Email;
using Aspose.Email.Storage.Mbox;

class Program
{
    static void Main(string[] args)
    {
        try
        {
            // Path to the MBOX file
            string mboxPath = "archive.mbox";

            // Verify that the MBOX file exists
            if (!File.Exists(mboxPath))
            {
                Console.Error.WriteLine($"MBOX file not found: {mboxPath}");
                return;
            }

            // List of message IDs to retrieve
            string[] messageIds = new string[] { "id1", "id2" };

            // Create the MBOX reader
            using (MboxStorageReader mbox = MboxStorageReader.CreateReader(mboxPath, new MboxLoadOptions()))
            {
                foreach (string id in messageIds)
                {
                    try
                    {
                        // Extract the message by its unique identifier
                        MailMessage message = mbox.ExtractMessage(id, new EmlLoadOptions());

                        // Ensure the MailMessage is disposed after processing
                        using (message)
                        {
                            // Example processing: output the subject
                            Console.WriteLine($"Subject: {message.Subject}");

                            // Save the extracted message to an .eml file
                            string safeSubject = string.IsNullOrEmpty(message.Subject) ? "NoSubject" : message.Subject;
                            string fileName = $"{safeSubject}_{id}.eml";

                            try
                            {
                                message.Save(fileName, SaveOptions.DefaultEml);
                            }
                            catch (Exception saveEx)
                            {
                                Console.Error.WriteLine($"Failed to save message {id}: {saveEx.Message}");
                            }
                        }
                    }
                    catch (Exception extractEx)
                    {
                        Console.Error.WriteLine($"Failed to extract message with ID {id}: {extractEx.Message}");
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