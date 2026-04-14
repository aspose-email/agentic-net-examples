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

            // Ensure output directory exists
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

            // Load the MSG file and scan attachments
            try
            {
                using (MapiMessage message = MapiMessage.Load(inputPath))
                {
                    bool hasProhibitedAttachment = false;
                    foreach (MapiAttachment attachment in message.Attachments)
                    {
                        // Example prohibited content: executable files
                        if (attachment.FileName != null &&
                            attachment.FileName.EndsWith(".exe", StringComparison.OrdinalIgnoreCase))
                        {
                            Console.Error.WriteLine($"Prohibited attachment detected: {attachment.FileName}");
                            hasProhibitedAttachment = true;
                            // Optionally, you could remove the attachment here if needed
                        }
                    }

                    if (hasProhibitedAttachment)
                    {
                        Console.Error.WriteLine("Message contains prohibited attachments. Save operation aborted.");
                        return;
                    }

                    // Save the message after successful validation
                    try
                    {
                        message.Save(outputPath);
                        Console.WriteLine($"Message saved successfully to {outputPath}");
                    }
                    catch (Exception saveEx)
                    {
                        Console.Error.WriteLine($"Failed to save message: {saveEx.Message}");
                    }
                }
            }
            catch (Exception loadEx)
            {
                Console.Error.WriteLine($"Failed to load MSG file: {loadEx.Message}");
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
