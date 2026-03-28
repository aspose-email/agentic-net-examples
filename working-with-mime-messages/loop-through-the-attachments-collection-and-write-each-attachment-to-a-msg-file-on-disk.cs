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
            // Path to the source Outlook MSG file
            string msgPath = "input.msg";

            // Verify the source file exists before attempting to load it
            if (!File.Exists(msgPath))
            {
                Console.Error.WriteLine($"Message file not found: {msgPath}");
                return;
            }

            // Load the MSG file inside a using block to ensure proper disposal
            using (MapiMessage message = MapiMessage.Load(msgPath))
            {
                // Get the collection of attachments from the message
                MapiAttachmentCollection attachments = message.Attachments;

                // Iterate through each attachment and save it to disk
                foreach (MapiAttachment attachment in attachments)
                {
                    // Determine the output file name (use the original attachment name)
                    string outputPath = attachment.FileName;

                    // Ensure the target directory exists
                    string outputDir = Path.GetDirectoryName(outputPath);
                    if (!string.IsNullOrEmpty(outputDir) && !Directory.Exists(outputDir))
                    {
                        Directory.CreateDirectory(outputDir);
                    }

                    // Save the attachment to the specified file
                    attachment.Save(outputPath);
                    Console.WriteLine($"Saved attachment: {outputPath}");
                }
            }
        }
        catch (Exception ex)
        {
            // Write any unexpected errors to the error console
            Console.Error.WriteLine(ex.Message);
        }
    }
}
