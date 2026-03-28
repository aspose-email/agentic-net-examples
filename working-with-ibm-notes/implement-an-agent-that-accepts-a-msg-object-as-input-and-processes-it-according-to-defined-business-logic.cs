using System;
using System.IO;
using Aspose.Email;
using Aspose.Email.Mapi;

class Program
{
    static void Main(string[] args)
    {
        try
        {
            if (args.Length == 0)
            {
                Console.Error.WriteLine("Error: No input file path provided.");
                return;
            }

            string msgPath = args[0];

            if (!File.Exists(msgPath))
            {
                Console.Error.WriteLine($"Error: File not found – {msgPath}");
                return;
            }

            // Load the MSG file
            using (MapiMessage message = MapiMessage.Load(msgPath))
            {
                Console.WriteLine($"Subject: {message.Subject}");
                Console.WriteLine($"From: {message.SenderName} <{message.SenderEmailAddress}>");
                Console.WriteLine($"Body: {message.Body}");

                // Determine output directory for attachments
                string outputDirectory = Path.GetDirectoryName(msgPath);
                if (string.IsNullOrEmpty(outputDirectory))
                {
                    outputDirectory = ".";
                }

                if (!Directory.Exists(outputDirectory))
                {
                    try
                    {
                        Directory.CreateDirectory(outputDirectory);
                    }
                    catch (Exception dirEx)
                    {
                        Console.Error.WriteLine($"Error creating directory {outputDirectory}: {dirEx.Message}");
                        return;
                    }
                }

                // Process attachments
                foreach (MapiAttachment attachment in message.Attachments)
                {
                    string attachmentPath = Path.Combine(outputDirectory, attachment.FileName);
                    try
                    {
                        attachment.Save(attachmentPath);
                        Console.WriteLine($"Saved attachment: {attachment.FileName}");
                    }
                    catch (Exception attEx)
                    {
                        Console.Error.WriteLine($"Error saving attachment {attachment.FileName}: {attEx.Message}");
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
