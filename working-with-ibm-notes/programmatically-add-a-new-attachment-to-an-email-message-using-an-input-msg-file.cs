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

            // Ensure the input MSG file exists; create a minimal placeholder if it does not.
            if (!File.Exists(inputPath))
            {
                try
                {
                    using (MapiMessage placeholder = new MapiMessage(
                        "sender@example.com",
                        "receiver@example.com",
                        "Placeholder subject",
                        "Placeholder body"))
                    {
                        placeholder.Save(inputPath);
                        Console.WriteLine($"Created placeholder MSG at {inputPath}");
                    }
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Error creating placeholder MSG: {ex.Message}");
                    return;
                }
            }

            // Load the existing MSG file.
            MapiMessage message;
            try
            {
                message = MapiMessage.Load(inputPath);
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Error loading MSG file: {ex.Message}");
                return;
            }

            using (message)
            {
                // Prepare attachment data.
                string attachmentName = "NewAttachment.txt";
                byte[] attachmentData = System.Text.Encoding.UTF8.GetBytes("This is the content of the new attachment.");

                // Add the attachment to the message.
                try
                {
                    message.Attachments.Add(attachmentName, attachmentData);
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Error adding attachment: {ex.Message}");
                    return;
                }

                // Ensure the output directory exists.
                try
                {
                    string outputDir = Path.GetDirectoryName(outputPath);
                    if (!string.IsNullOrEmpty(outputDir) && !Directory.Exists(outputDir))
                    {
                        Directory.CreateDirectory(outputDir);
                    }
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Error ensuring output directory: {ex.Message}");
                    return;
                }

                // Save the modified message.
                try
                {
                    message.Save(outputPath);
                    Console.WriteLine($"Message saved with new attachment to {outputPath}");
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Error saving MSG file: {ex.Message}");
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
