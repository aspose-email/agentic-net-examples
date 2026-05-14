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
            // Path to the source message file (MSG or EML)
            string messagePath = "sample.msg";

            // Verify that the source file exists before attempting to load it
            if (!File.Exists(messagePath))
            {
                try
                {
                    using (MapiMessage placeholder = new MapiMessage(
                        "from@example.com",
                        "to@example.com",
                        "Placeholder Subject",
                        "Placeholder body."))
                    {
                        placeholder.Save(messagePath);
                    }
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Error creating placeholder MSG: {ex.Message}");
                    return;
                }

                Console.Error.WriteLine($"Message file not found: {messagePath}");
                return;
            }

            // Directory where extracted audio files will be saved
            string outputDir = "ExtractedAudio";
            if (!Directory.Exists(outputDir))
            {
                Directory.CreateDirectory(outputDir);
            }

            // Load the message inside a using block to ensure proper disposal
            using (MapiMessage message = MapiMessage.Load(messagePath))
            {
                int audioCounter = 0;

                // Iterate through all attachments in the message
                foreach (MapiAttachment attachment in message.Attachments)
                {
                    // Identify audio attachments by .mp3 extension (case‑insensitive)
                    if (!string.IsNullOrEmpty(attachment.FileName) &&
                        attachment.FileName.EndsWith(".mp3", StringComparison.OrdinalIgnoreCase))
                    {
                        // Build a unique output file name for each extracted audio clip
                        string outputFileName = Path.Combine(outputDir, $"audio_{audioCounter}_{attachment.FileName}");
                        try
                        {
                            // Write the binary data of the attachment to the file system
                            File.WriteAllBytes(outputFileName, attachment.BinaryData);
                            Console.WriteLine($"Saved audio clip: {outputFileName}");
                        }
                        catch (Exception ioEx)
                        {
                            Console.Error.WriteLine($"Failed to save '{outputFileName}': {ioEx.Message}");
                        }

                        audioCounter++;
                    }
                }

                if (audioCounter == 0)
                {
                    Console.WriteLine("No embedded MP3 audio clips were found in the message.");
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"An error occurred: {ex.Message}");
        }
    }
}
