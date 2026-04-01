using System;
using System.IO;
using Aspose.Email;
using Aspose.Email.Mapi;

class Program
{
    static void Main()
    {
        try
        {
            const string msgPath = "sample.msg";
            const string attachmentsFolder = "Attachments";

            // Ensure the MSG file exists; create a minimal placeholder if missing.
            if (!File.Exists(msgPath))
            {
                using (MapiMessage placeholder = new MapiMessage("Placeholder Subject", "Placeholder Body", "sender@example.com", "recipient@example.com"))
                {
                    placeholder.Save(msgPath);
                }
            }

            // Load the MSG file.
            using (MapiMessage message = MapiMessage.Load(msgPath))
            {
                Console.WriteLine("Subject: " + message.Subject);
                Console.WriteLine("Sender: " + message.SenderEmailAddress);
                Console.WriteLine("Body: " + message.Body);

                // Process attachments, if any.
                if (message.Attachments != null && message.Attachments.Count > 0)
                {
                    Directory.CreateDirectory(attachmentsFolder);

                    foreach (MapiAttachment attachment in message.Attachments)
                    {
                        byte[] data = attachment.BinaryData;
                        if (data != null && data.Length > 0)
                        {
                            string fileName = !string.IsNullOrEmpty(attachment.LongFileName)
                                ? attachment.LongFileName
                                : (!string.IsNullOrEmpty(attachment.FileName) ? attachment.FileName : "attachment.bin");

                            string outputPath = Path.Combine(attachmentsFolder, fileName);
                            File.WriteAllBytes(outputPath, data);
                            Console.WriteLine($"Saved attachment: {outputPath}");
                        }
                    }
                }
                else
                {
                    Console.WriteLine("No attachments found.");
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine("Error: " + ex.Message);
        }
    }
}
