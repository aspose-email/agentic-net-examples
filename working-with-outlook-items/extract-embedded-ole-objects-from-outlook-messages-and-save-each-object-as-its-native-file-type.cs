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
            // Path to the Outlook MSG file
            string msgFilePath = "sample.msg";

            // Directory where extracted OLE objects will be saved
            string outputDirectory = "OleObjects";

            // Verify that the MSG file exists
            if (!File.Exists(msgFilePath))
            {
                try
                {
                    using (MapiMessage placeholder = new MapiMessage(
                        "from@example.com",
                        "to@example.com",
                        "Placeholder Subject",
                        "Placeholder body."))
                    {
                        placeholder.Save(msgFilePath);
                    }
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Error creating placeholder MSG: {ex.Message}");
                    return;
                }

                Console.Error.WriteLine($"Input file not found: {msgFilePath}");
                return;
            }

            // Ensure the output directory exists
            if (!Directory.Exists(outputDirectory))
            {
                Directory.CreateDirectory(outputDirectory);
            }

            // Load the MSG file
            using (MapiMessage message = MapiMessage.Load(msgFilePath))
            {
                // Iterate through all attachments (OLE objects are represented as attachments)
                foreach (MapiAttachment attachment in message.Attachments)
                {
                    // Build a safe file name for the extracted object
                    string safeFileName = $"{Path.GetFileNameWithoutExtension(msgFilePath)}_{attachment.FileName}";
                    string outputPath = Path.Combine(outputDirectory, safeFileName);

                    // Save the attachment to disk
                    attachment.Save(outputPath);
                    Console.WriteLine($"Saved OLE object: {outputPath}");
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
