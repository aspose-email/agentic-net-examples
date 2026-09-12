using Aspose.Email;
using Aspose.Email.Mapi;
using System;
using System.IO;

class Program
{
    static void Main()
    {
        try
        {
            // Path to the source Outlook MSG file.
            string msgPath = "input.msg";

            // Verify the source file exists; create a placeholder if it does not.
            if (!File.Exists(msgPath))
            {
                try
                {
                    var placeholder = new MapiMessage(
                        "from@example.com",
                        "to@example.com",
                        "Placeholder Subject",
                        "Placeholder body.");
                    placeholder.Save(msgPath);
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Error creating placeholder MSG: {ex.Message}");
                    return;
                }

                Console.Error.WriteLine($"Source file not found: {msgPath}");
                return;
            }

            // Load the MSG file.
            MapiMessage msg = MapiMessage.Load(msgPath);

            // Ensure there is at least one attachment.
            if (msg.Attachments == null || msg.Attachments.Count == 0)
            {
                Console.WriteLine("No attachments found in the message.");
                return;
            }

            // Prepare output directory for attachments.
            string outputDir = "attachments";
            Directory.CreateDirectory(outputDir);

            // Iterate through each attachment and save it to disk.
            foreach (MapiAttachment attachment in msg.Attachments)
            {
                // Use the original file name for the saved attachment.
                string attachmentFileName = attachment.FileName;

                // Guard against empty or invalid file names.
                if (string.IsNullOrWhiteSpace(attachmentFileName))
                {
                    Console.Error.WriteLine("Attachment has an invalid file name; skipping.");
                    continue;
                }

                // Remove any invalid characters from the file name.
                foreach (char c in Path.GetInvalidFileNameChars())
                {
                    attachmentFileName = attachmentFileName.Replace(c, '_');
                }

                // Build full path.
                string fullPath = Path.Combine(outputDir, attachmentFileName);

                // Save the attachment.
                try
                {
                    attachment.Save(fullPath);
                    Console.WriteLine($"Saved attachment: {fullPath}");
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to save attachment '{attachmentFileName}': {ex.Message}");
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
