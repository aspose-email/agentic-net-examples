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
            string inputPath = "input.msg";
            string outputPath = "output.msg";

            // Verify that the input MSG file exists
            if (!File.Exists(inputPath))
            {
                try
                {
                    using (MapiMessage placeholder = new MapiMessage(
                        "from@example.com",
                        "to@example.com",
                        "Placeholder Subject",
                        "Placeholder body."))
                    {
                        placeholder.Save(inputPath);
                    }
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Error creating placeholder MSG: {ex.Message}");
                    return;
                }

                Console.Error.WriteLine($"Input file '{inputPath}' does not exist.");
                return;
            }

            // Ensure the output directory exists
            string outputDirectory = Path.GetDirectoryName(outputPath);
            if (!string.IsNullOrEmpty(outputDirectory) && !Directory.Exists(outputDirectory))
            {
                try
                {
                    Directory.CreateDirectory(outputDirectory);
                }
                catch (Exception dirEx)
                {
                    Console.Error.WriteLine($"Failed to create output directory: {dirEx.Message}");
                    return;
                }
            }

            // Remove all attachments from the MSG file
            MapiAttachmentCollection removedAttachments;
            try
            {
                removedAttachments = MapiMessage.RemoveAttachments(inputPath);
            }
            catch (Exception removeEx)
            {
                Console.Error.WriteLine($"Failed to remove attachments: {removeEx.Message}");
                return;
            }

            // List removed attachments (optional)
            foreach (MapiAttachment attachment in removedAttachments)
            {
                Console.WriteLine($"Removed attachment: {attachment.FileName}");
            }

            // Load the modified message and save it as a new MSG file
            using (MapiMessage message = MapiMessage.Load(inputPath))
            {
                try
                {
                    message.Save(outputPath);
                }
                catch (Exception saveEx)
                {
                    Console.Error.WriteLine($"Failed to save message: {saveEx.Message}");
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
