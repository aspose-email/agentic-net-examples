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
            string inputPath = @"c:\input\source.msg";
            string outputPath = @"c:\output\note.msg";

            // Verify input file exists
            if (!File.Exists(inputPath))
            {
                Console.Error.WriteLine($"Error: Input file not found – {inputPath}");
                return;
            }

            // Ensure output directory exists
            string outputDir = Path.GetDirectoryName(outputPath);
            if (!Directory.Exists(outputDir))
            {
                try
                {
                    Directory.CreateDirectory(outputDir);
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Error: Unable to create output directory – {ex.Message}");
                    return;
                }
            }

            // Load the MSG file into a MapiMessage
            using (MapiMessage sourceMessage = MapiMessage.Load(inputPath))
            {
                // Preserve content and metadata
                Console.WriteLine("Subject: " + sourceMessage.Subject);
                Console.WriteLine("From: " + sourceMessage.SenderName);
                Console.WriteLine("Body: " + sourceMessage.Body);

                // Create a new Notes document (IPM.StickyNote) using the same content
                using (MapiMessage noteMessage = new MapiMessage())
                {
                    noteMessage.Subject = sourceMessage.Subject;
                    noteMessage.Body = sourceMessage.Body;
                    noteMessage.SenderName = sourceMessage.SenderName;
                    noteMessage.SenderEmailAddress = sourceMessage.SenderEmailAddress;
                    noteMessage.MessageClass = "IPM.StickyNote";

                    // Copy attachments if any
                    foreach (MapiAttachment attachment in sourceMessage.Attachments)
                    {
                        // Preserve attachment by adding it to the new message
                        noteMessage.Attachments.Add(attachment);
                    }

                    // Save the new Notes document
                    noteMessage.Save(outputPath);
                }
            }

            Console.WriteLine("Notes document created successfully at: " + outputPath);
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
