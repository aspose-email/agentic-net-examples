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
            // Input MSG file path
            string inputMessagePath = "message.msg";
            // Output directory for attachments
            string outputDirectory = "Attachments";

            // Verify input file exists
            if (!File.Exists(inputMessagePath))
            {
                try
                {
                    using (MapiMessage placeholder = new MapiMessage(
                        "from@example.com",
                        "to@example.com",
                        "Placeholder Subject",
                        "Placeholder body."))
                    {
                        placeholder.Save(inputMessagePath);
                    }
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Error creating placeholder MSG: {ex.Message}");
                    return;
                }

                Console.Error.WriteLine($"Input file not found: {inputMessagePath}");
                return;
            }

            // Ensure output directory exists
            try
            {
                if (!Directory.Exists(outputDirectory))
                {
                    Directory.CreateDirectory(outputDirectory);
                }
            }
            catch (Exception dirEx)
            {
                Console.Error.WriteLine($"Failed to create output directory: {dirEx.Message}");
                return;
            }

            // Load the MSG file
            using (MapiMessage message = MapiMessage.Load(inputMessagePath))
            {
                // Iterate through attachments
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
                        Console.Error.WriteLine($"Failed to save attachment '{attachment.FileName}': {attEx.Message}");
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
