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
            // Input MSG file path
            string inputPath = "sample.msg";

            // Verify input file exists
            if (!File.Exists(inputPath))
            {
                Console.Error.WriteLine($"Input file not found: {inputPath}");
                return;
            }

            // Ensure output directory exists for any saved attachment
            string outputDir = "output";
            if (!Directory.Exists(outputDir))
                Directory.CreateDirectory(outputDir);

            // Open the MSG file with MapiMessageReader
            using (MapiMessageReader reader = new MapiMessageReader(inputPath))
            {
                // NOTE: The property 'CurrentAttachment' is not documented in the current Aspose.Email version.
                // The following line is a placeholder to illustrate the intended usage.
                // Replace it with the actual API when available.
                Attachment attachment = null; // reader.CurrentAttachment;

                // Fallback: retrieve the first attachment from the message if CurrentAttachment is unavailable
                MapiMessage message = reader.ReadMessage();
                if (message.Attachments.Count > 0)
                {
                    MapiAttachment mapiAtt = message.Attachments[0];
                    string attPath = Path.Combine(outputDir, mapiAtt.FileName);
                    mapiAtt.Save(attPath);
                    attachment = new Attachment(attPath);
                }

                if (attachment != null)
                {
                    Console.WriteLine($"Attachment retrieved: {attachment.Name}");
                }
                else
                {
                    Console.WriteLine("No attachment found in the MSG file.");
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
