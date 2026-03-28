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
            string msgPath = Console.ReadLine();

            if (string.IsNullOrWhiteSpace(msgPath))
            {
                Console.Error.WriteLine("Error: No path provided.");
                return;
            }

            if (!File.Exists(msgPath))
            {
                Console.Error.WriteLine($"Error: File not found – {msgPath}");
                return;
            }

            try
            {
                using (MapiMessage msg = MapiMessage.Load(msgPath))
                {
                    Console.WriteLine($"Subject: {msg.Subject}");
                    Console.WriteLine($"From: {msg.SenderName} <{msg.SenderEmailAddress}>");
                    Console.WriteLine("Body:");
                    Console.WriteLine(msg.Body);
                    Console.WriteLine();

                    if (msg.Attachments != null && msg.Attachments.Count > 0)
                    {
                        Console.WriteLine("Attachments:");
                        foreach (MapiAttachment attachment in msg.Attachments)
                        {
                            Console.WriteLine($"- {attachment.FileName}");
                            // Save attachment to the same directory as the MSG file
                            string attachmentPath = Path.Combine(Path.GetDirectoryName(msgPath) ?? string.Empty, attachment.FileName);
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
