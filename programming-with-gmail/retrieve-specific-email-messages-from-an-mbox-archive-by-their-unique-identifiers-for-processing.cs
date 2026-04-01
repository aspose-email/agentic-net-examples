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
            // Input MBOX file path
            string mboxPath = "sample.mbox";

            // Output directory for processed messages
            string outputFolder = "output";

            // Unique identifiers of messages to retrieve (Message-Id header values)
            string[] targetIds = new string[] { "<msg1@example.com>", "<msg2@example.com>" };

            // Guard input file existence
            if (!File.Exists(mboxPath))
            {
                Console.Error.WriteLine($"MBOX file not found: {mboxPath}");
                return;
            }

            // Ensure output directory exists
            Directory.CreateDirectory(outputFolder);

            // Create the MBOX reader using the required factory method
            using (MboxStorageReader reader = MboxStorageReader.CreateReader(mboxPath, new MboxLoadOptions()))
            {
                MailMessage message;
                // Read messages sequentially
                while ((message = reader.ReadNextMessage()) != null)
                {
                    using (message)
                    {
                        // Check if the current message matches one of the requested IDs
                        if (Array.Exists(targetIds, id => id == message.MessageId))
                        {
                            // Prepare a safe file name
                            string safeSubject = string.IsNullOrEmpty(message.Subject) ? "NoSubject" : message.Subject;
                            string fileName = Path.Combine(outputFolder, $"{safeSubject}_{Guid.NewGuid()}.html");

                            try
                            {
                                // Save the message as HTML
                                HtmlSaveOptions saveOptions = new HtmlSaveOptions();
                                message.Save(fileName, saveOptions);
                                Console.WriteLine($"Saved message {message.MessageId} to {fileName}");
                            }
                            catch (Exception ex)
                            {
                                Console.Error.WriteLine($"Failed to save message {message.MessageId}: {ex.Message}");
                            }
                        }
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
