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
            // Path to the source MSG file
            string inputMsgPath = "input.msg";

            // Verify the input file exists
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

                Console.Error.WriteLine($"Input file '{inputMsgPath}' does not exist.");
                return;
            }

            // Directory where extracted embedded messages will be saved
            string outputDirectory = "ExtractedMessages";
            Directory.CreateDirectory(outputDirectory);

            // Load the MSG file
            using (MapiMessage message = MapiMessage.Load(inputMsgPath))
            {
                // Iterate through all attachments
                foreach (MapiAttachment attachment in message.Attachments)
                {
                    // Identify embedded messages by .msg extension
                    if (string.Equals(Path.GetExtension(attachment.FileName), ".msg", StringComparison.OrdinalIgnoreCase))
                    {
                        string outputPath = Path.Combine(outputDirectory, attachment.FileName);
                        // Save the embedded message as a separate MSG file
                        attachment.Save(outputPath);
                        Console.WriteLine($"Extracted embedded message to: {outputPath}");
                    }
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
