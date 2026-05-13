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
            string inputPath = "note.msg";
            string outputPath = "note_updated.msg";

            // Guard file existence
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

            try
            {
                // Load the MSG note
                using (MapiMessage message = MapiMessage.Load(inputPath))
                {
                    // Replace line breaks with HTML <br> tags
                    string originalBody = message.Body ?? string.Empty;
                    string updatedBody = originalBody
                        .Replace("\r\n", "<br>")
                        .Replace("\n", "<br>")
                        .Replace("\r", "<br>");
                    message.Body = updatedBody;

                    // Save the modified message back to MSG
                    message.Save(outputPath);
                    Console.WriteLine($"Message saved to {outputPath}");
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Error processing message: {ex.Message}");
                return;
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
