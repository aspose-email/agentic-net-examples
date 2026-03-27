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
            Console.WriteLine("Enter the full path to the MSG file:");
            string inputPath = Console.ReadLine();

            if (string.IsNullOrWhiteSpace(inputPath))
            {
                Console.Error.WriteLine("Error: No path provided.");
                return;
            }

            if (!File.Exists(inputPath))
            {
                Console.Error.WriteLine($"Error: File not found – {inputPath}");
                return;
            }

            try
            {
                using (MapiMessage message = MapiMessage.Load(inputPath))
                {
                    Console.WriteLine($"Subject: {message.Subject}");
                    Console.WriteLine($"From: {message.SenderName}");
                    Console.WriteLine("Body:");
                    Console.WriteLine(message.Body);
                    Console.WriteLine();

                    if (message.Attachments != null && message.Attachments.Count > 0)
                    {
                        Console.WriteLine("Attachments:");
                        foreach (MapiAttachment attachment in message.Attachments)
                        {
                            Console.WriteLine($"- FileName: {attachment.FileName}");
                            // Save attachment to the same directory as the MSG file
                            string attachmentPath = Path.Combine(Path.GetDirectoryName(inputPath) ?? string.Empty, attachment.FileName);
                            try
                            {
                                attachment.Save(attachmentPath);
                                Console.WriteLine($"  Saved to: {attachmentPath}");
                            }
                            catch (Exception ex)
                            {
                                Console.Error.WriteLine($"  Failed to save attachment '{attachment.FileName}': {ex.Message}");
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
                Console.Error.WriteLine($"Error processing MSG file: {ex.Message}");
                return;
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
