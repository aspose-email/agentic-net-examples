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
            // Input MSG file path
            const string msgPath = "input.msg";
            // Directory where extracted files will be saved
            const string outputDir = "output";

            // Verify input file exists
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

            // Ensure output directory exists
            if (!Directory.Exists(outputDir))
            {
                Directory.CreateDirectory(outputDir);
            }

            // Load the Outlook MSG file
            MapiMessage msg = MapiMessage.Load(msgPath);

            // Process each attachment in the MSG
            foreach (MapiAttachment attachment in msg.Attachments)
            {
                // Save the attachment to the output folder
                string attachmentPath = Path.Combine(outputDir, attachment.FileName);
                attachment.Save(attachmentPath);
                Console.WriteLine($"Saved attachment: {attachmentPath}");

                // If the attachment is a TNEF file (commonly with .dat extension), extract its contents
                if (Path.GetExtension(attachmentPath).Equals(".dat", StringComparison.OrdinalIgnoreCase))
                {
                    try
                    {
                        // Load the TNEF content as a MapiMessage
                        MapiMessage tnefMessage = MapiMessage.LoadFromTnef(attachmentPath);

                        // Extract inner attachments from the TNEF message
                        foreach (MapiAttachment innerAttachment in tnefMessage.Attachments)
                        {
                            string innerPath = Path.Combine(outputDir, innerAttachment.FileName);
                            innerAttachment.Save(innerPath);
                            Console.WriteLine($"Saved inner attachment: {innerPath}");
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.Error.WriteLine($"Failed to process TNEF attachment '{attachment.FileName}': {ex.Message}");
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
