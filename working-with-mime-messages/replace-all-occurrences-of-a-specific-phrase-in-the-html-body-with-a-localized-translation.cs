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
            string phraseToReplace = "Hello";
            string localizedTranslation = "Bonjour";

            // Ensure input file exists; create a minimal placeholder if missing
            if (!File.Exists(inputPath))
            {
                try
                {
                    MapiMessage placeholder = new MapiMessage(
                        "sender@example.com",
                        "recipient@example.com",
                        "Placeholder Subject",
                        "Hello world");
                    placeholder.Save(inputPath);
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to create placeholder MSG: {ex.Message}");
                    return;
                }
            }

            // Load the MSG file, replace phrase in HTML body, and save the result
            try
            {
                using (MapiMessage msg = MapiMessage.Load(inputPath))
                {
                    string originalHtml = msg.BodyHtml ?? string.Empty;
                    string updatedHtml = originalHtml.Replace(phraseToReplace, localizedTranslation);

                    // Update the body content as HTML
                    msg.SetBodyContent(updatedHtml, BodyContentType.Html);

                    msg.Save(outputPath);
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Error processing MSG file: {ex.Message}");
                return;
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
