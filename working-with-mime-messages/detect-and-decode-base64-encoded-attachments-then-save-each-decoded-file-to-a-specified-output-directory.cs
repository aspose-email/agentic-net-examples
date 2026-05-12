using Aspose.Email;
using System;
using System.IO;
using Aspose.Email.Mapi;

class Program
{
    static void Main(string[] args)
    {
        try
        {
            // Input MSG file path (adjust as needed)
            string inputMsgPath = "input.msg";
            // Output directory for decoded attachments
            string outputDirectory = "output";

            // Guard input file existence
            if (!File.Exists(inputMsgPath))
            {
                try
                {
                    using (MapiMessage placeholder = new MapiMessage(
                        "from@example.com",
                        "to@example.com",
                        "Placeholder Subject",
                        "Placeholder body."))
                    {
                        placeholder.Save(inputMsgPath);
                    }
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Error creating placeholder MSG: {ex.Message}");
                    return;
                }

                Console.Error.WriteLine($"Input file not found: {inputMsgPath}");
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
            using (MapiMessage msg = MapiMessage.Load(inputMsgPath))
            {
                foreach (MapiAttachment attachment in msg.Attachments)
                {
                    string fileName = string.IsNullOrEmpty(attachment.FileName) ? "unnamed_attachment" : attachment.FileName;
                    string outputPath = Path.Combine(outputDirectory, fileName);

                    try
                    {
                        // Save the attachment; Aspose.Email automatically handles decoding (including Base64)
                        attachment.Save(outputPath);
                        Console.WriteLine($"Saved attachment: {outputPath}");
                    }
                    catch (Exception saveEx)
                    {
                        Console.Error.WriteLine($"Failed to save attachment '{fileName}': {saveEx.Message}");
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
