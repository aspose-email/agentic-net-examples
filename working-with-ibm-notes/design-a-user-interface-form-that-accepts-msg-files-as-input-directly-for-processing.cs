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
            Console.Write("Enter the full path to the MSG file: ");
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

            // Load the MSG file safely
            MapiMessage msg;
            try
            {
                msg = MapiMessage.Load(msgPath);
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Error loading MSG file: {ex.Message}");
                return;
            }

            using (msg)
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
                        string savePath = Path.Combine(Path.GetDirectoryName(msgPath) ?? "", attachment.FileName);
                        try
                        {
                            attachment.Save(savePath);
                            Console.WriteLine($"  Saved to: {savePath}");
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
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
