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
            // Path to the MSG file
            string msgPath = "sample.msg";

            // Verify the MSG file exists
            if (!File.Exists(msgPath))
{
    try
    {
        MailMessage placeholderMsg = new MailMessage("sender@example.com", "recipient@example.com", "Placeholder", "This is a placeholder MSG.");
        placeholderMsg.Save(msgPath, SaveOptions.DefaultMsgUnicode);
    }
    catch (Exception ex)
    {
        Console.Error.WriteLine($"Failed to create placeholder MSG: {ex.Message}");
        return;
    }
}


            // Extract attachments from the MSG file
            MapiAttachmentCollection attachments;
            try
            {
                attachments = MapiMessage.RemoveAttachments(msgPath);
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Failed to extract attachments: {ex.Message}");
                return;
            }

            // Iterate through each attachment and save it to disk
            foreach (MapiAttachment attachment in attachments)
            {
                // Use the original file name for the saved attachment
                string outputPath = attachment.FileName;

                // Ensure the target directory exists
                string outputDirectory = Path.GetDirectoryName(outputPath);
                if (!string.IsNullOrEmpty(outputDirectory) && !Directory.Exists(outputDirectory))
                {
                    try
                    {
                        Directory.CreateDirectory(outputDirectory);
                    }
                    catch (Exception dirEx)
                    {
                        Console.Error.WriteLine($"Failed to create directory '{outputDirectory}': {dirEx.Message}");
                        continue;
                    }
                }

                // Save the attachment
                try
                {
                    attachment.Save(outputPath);
                    Console.WriteLine($"Saved attachment: {outputPath}");
                }
                catch (Exception saveEx)
                {
                    Console.Error.WriteLine($"Failed to save attachment '{outputPath}': {saveEx.Message}");
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
