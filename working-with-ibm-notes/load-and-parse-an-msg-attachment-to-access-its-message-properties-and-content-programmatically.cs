using System;
using System.IO;
using Aspose.Email;
using Aspose.Email.Mapi;

namespace AsposeEmailExample
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                // Path to the MSG file (attachment)
                string msgPath = "sample.msg";

                // Verify that the file exists before attempting to load it
                if (!File.Exists(msgPath))
                {
                    Console.Error.WriteLine($"Error: File not found – {msgPath}");
                    return;
                }

                // Load the MSG file into a MapiMessage instance
                using (MapiMessage msg = MapiMessage.Load(msgPath))
                {
                    // Access basic message properties
                    Console.WriteLine($"Subject: {msg.Subject}");
                    Console.WriteLine($"From: {msg.SenderName}");
                    Console.WriteLine($"Body: {msg.Body}");

                    // Iterate through attachments, display their names and save them to disk
                    foreach (MapiAttachment attachment in msg.Attachments)
                    {
                        Console.WriteLine($"Attachment: {attachment.FileName}");

                        // Determine a safe file name for saving the attachment
                        string attachmentPath = attachment.FileName;
                        if (File.Exists(attachmentPath))
                        {
                            int duplicateIndex = 1;
                            string baseName = Path.GetFileNameWithoutExtension(attachmentPath);
                            string extension = Path.GetExtension(attachmentPath);
                            while (File.Exists($"{baseName}_{duplicateIndex}{extension}"))
                            {
                                duplicateIndex++;
                            }
                            attachmentPath = $"{baseName}_{duplicateIndex}{extension}";
                        }

                        // Save the attachment
                        attachment.Save(attachmentPath);
                        Console.WriteLine($"Saved to: {attachmentPath}");
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
