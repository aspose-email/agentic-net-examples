using Aspose.Email;
using System;
using System.IO;
using Aspose.Email.Mapi;

class Program
{
    static void Main()
    {
        try
        {
            // Author note: Example to extract all attachments from an Outlook MSG file.
            string msgPath = @"c:\outlookmessage.msg";

            // Verify the MSG file exists before attempting to load it.
            if (!File.Exists(msgPath))
            {
                try
                {
                    using (MapiMessage placeholder = new MapiMessage(
                        "from@example.com",
                        "to@example.com",
                        "Placeholder Subject",
                        "Placeholder body."))
                    {
                        placeholder.Save(msgPath);
                    }
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Error creating placeholder MSG: {ex.Message}");
                    return;
                }

                Console.Error.WriteLine($"Input file not found: {msgPath}");
                return;
            }

            // Load the MSG file.
            MapiMessage msg = MapiMessage.Load(msgPath);

            // Prepare output directory for extracted attachments.
            string outputDir = Path.Combine(Path.GetDirectoryName(msgPath) ?? "", "Attachments");
            if (!Directory.Exists(outputDir))
            {
                Directory.CreateDirectory(outputDir);
            }

            // Extract and save each attachment in its original format.
            foreach (MapiAttachment att in msg.Attachments)
            {
                try
                {
                    string outputPath = Path.Combine(outputDir, att.FileName);
                    Console.WriteLine($"Saving attachment: {att.FileName}");
                    att.Save(outputPath);
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to save attachment '{att.FileName}': {ex.Message}");
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
