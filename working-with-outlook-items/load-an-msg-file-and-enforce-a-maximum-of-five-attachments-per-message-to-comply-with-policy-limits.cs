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
            const string inputPath = "input.msg";
            const string outputPath = "output.msg";

            // Verify input file exists
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

                Console.Error.WriteLine($"Input file not found: {inputPath}");
                return;
            }

            // Load the MSG file
            using (MapiMessage message = MapiMessage.Load(inputPath))
            {
                int attachmentCount = message.Attachments.Count;

                if (attachmentCount > 5)
                {
                    Console.Error.WriteLine($"Message has {attachmentCount} attachments, which exceeds the allowed maximum of 5.");
                    // Optionally, remove excess attachments (keep first five)
                    for (int i = attachmentCount - 1; i >= 5; i--)
                    {
                        // Remove extra attachments
                        message.Attachments.RemoveAt(i);
                    }

                    // Save the trimmed message
                    try
                    {
                        message.Save(outputPath);
                        Console.WriteLine($"Message saved with trimmed attachments to: {outputPath}");
                    }
                    catch (Exception ex)
                    {
                        Console.Error.WriteLine($"Failed to save message: {ex.Message}");
                    }
                }
                else
                {
                    // Save the original message unchanged
                    try
                    {
                        message.Save(outputPath);
                        Console.WriteLine($"Message saved to: {outputPath}");
                    }
                    catch (Exception ex)
                    {
                        Console.Error.WriteLine($"Failed to save message: {ex.Message}");
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
