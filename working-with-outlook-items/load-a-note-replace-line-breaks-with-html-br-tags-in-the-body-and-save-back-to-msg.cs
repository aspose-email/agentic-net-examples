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

            // Ensure the input file exists; create a minimal placeholder if it does not.
            if (!File.Exists(inputPath))
            {
                MapiMessage placeholder = new MapiMessage(
                    "placeholder@example.com",
                    "placeholder@example.com",
                    "Placeholder",
                    "This is a placeholder message."
                );

                try
                {
                    placeholder.Save(inputPath);
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to create placeholder MSG: {ex.Message}");
                    return;
                }
                finally
                {
                    placeholder.Dispose();
                }
            }

            // Load the MSG file.
            try
            {
                using (MapiMessage message = MapiMessage.Load(inputPath))
                {
                    // Replace line breaks with HTML <br> tags.
                    string originalBody = message.Body ?? string.Empty;
                    string updatedBody = originalBody
                        .Replace("\r\n", "<br>")
                        .Replace("\n", "<br>")
                        .Replace("\r", "<br>");
                    message.Body = updatedBody;

                    // Save the updated message.
                    try
                    {
                        message.Save(outputPath);
                    }
                    catch (Exception ex)
                    {
                        Console.Error.WriteLine($"Failed to save updated MSG: {ex.Message}");
                        return;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Failed to load MSG: {ex.Message}");
                return;
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
