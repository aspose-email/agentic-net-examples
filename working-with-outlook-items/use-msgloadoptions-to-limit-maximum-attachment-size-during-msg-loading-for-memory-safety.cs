using System;
using System.IO;
using System.Collections.Generic;
using Aspose.Email;
using Aspose.Email.Mapi;

class Program
{
    static void Main()
    {
        try
        {
            const string inputPath = "sample.msg";
            const string outputPath = "filtered.msg";
            const long maxAttachmentSize = 1 * 1024 * 1024; // 1 MB

            // Ensure the input MSG file exists; create a minimal placeholder if missing.
            if (!File.Exists(inputPath))
            {
                try
                {
                    using (MapiMessage placeholder = new MapiMessage(
                        "from@example.com",
                        "to@example.com",
                        "Placeholder Subject",
                        "Placeholder body"))
                    {
                        placeholder.Save(inputPath);
                        Console.WriteLine($"Created placeholder MSG file at '{inputPath}'.");
                    }
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Error creating placeholder MSG: {ex.Message}");
                    return;
                }
            }

            // Load the MSG file with load options.
            MsgLoadOptions loadOptions = new MsgLoadOptions();
            using (MailMessage message = MailMessage.Load(inputPath, loadOptions))
            {
                // Identify attachments exceeding the size limit.
                List<Attachment> attachmentsToRemove = new List<Attachment>();
                foreach (Attachment att in message.Attachments)
                {
                    if (att.ContentStream != null && att.ContentStream.Length > maxAttachmentSize)
                    {
                        attachmentsToRemove.Add(att);
                    }
                }

                // Remove oversized attachments.
                foreach (Attachment att in attachmentsToRemove)
                {
                    message.Attachments.Remove(att);
                }

                // Save the filtered message.
                try
                {
                    message.Save(outputPath);
                    Console.WriteLine($"Filtered message saved to '{outputPath}'.");
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Error saving filtered message: {ex.Message}");
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
