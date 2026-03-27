using System;
using System.IO;
using Aspose.Email;
using Aspose.Email.Mapi;

namespace AsposeEmailMsgProcessor
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                if (args.Length == 0)
                {
                    Console.Error.WriteLine("Error: No input MSG file path provided.");
                    return;
                }

                string msgFilePath = args[0];

                if (!File.Exists(msgFilePath))
                {
                    Console.Error.WriteLine($"Error: File not found – {msgFilePath}");
                    return;
                }

                using (MapiMessage message = MapiMessage.Load(msgFilePath))
                {
                    Console.WriteLine($"Subject: {message.Subject}");
                    Console.WriteLine($"From: {message.SenderName}");
                    Console.WriteLine($"Body: {message.Body}");

                    foreach (MapiAttachment attachment in message.Attachments)
                    {
                        Console.WriteLine($"Attachment: {attachment.FileName}");

                        string attachmentSavePath = Path.Combine(
                            Path.GetDirectoryName(msgFilePath) ?? string.Empty,
                            attachment.FileName);

                        try
                        {
                            attachment.Save(attachmentSavePath);
                            Console.WriteLine($"Saved attachment to {attachmentSavePath}");
                        }
                        catch (Exception saveEx)
                        {
                            Console.Error.WriteLine($"Error saving attachment '{attachment.FileName}': {saveEx.Message}");
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
}
