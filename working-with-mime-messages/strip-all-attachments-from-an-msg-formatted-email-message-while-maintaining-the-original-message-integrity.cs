using System;
using System.IO;
using Aspose.Email.Mapi;

class Program
{
    static void Main()
    {
        try
        {
            string inputPath = "message.msg";
            string outputPath = "message_no_attachments.msg";

            if (!File.Exists(inputPath))
{
    try
    {
        MailMessage placeholderMsg = new MailMessage("sender@example.com", "recipient@example.com", "Placeholder", "This is a placeholder MSG.");
        placeholderMsg.Save(inputPath, SaveOptions.DefaultMsgUnicode);
    }
    catch (Exception ex)
    {
        Console.Error.WriteLine($"Failed to create placeholder MSG: {ex.Message}");
        return;
    }
}


            // Create a temporary copy to operate on, preserving the original file.
            string tempPath = Path.Combine(Path.GetDirectoryName(outputPath) ?? string.Empty,
                                           Path.GetFileNameWithoutExtension(outputPath) + "_temp.msg");

            try
            {
                File.Copy(inputPath, tempPath, true);
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Failed to copy file: {ex.Message}");
                return;
            }

            // Remove all attachments from the temporary MSG file.
            try
            {
                MapiAttachmentCollection removed = MapiMessage.RemoveAttachments(tempPath);
                // Optionally, you could iterate over 'removed' to log removed attachment names.
                // foreach (MapiAttachment att in removed)
                // {
                //     Console.WriteLine($"Removed attachment: {att.FileName}");
                // }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Failed to remove attachments: {ex.Message}");
                return;
            }

            // Load the cleaned message and save it to the desired output path.
            try
            {
                using (MapiMessage cleanedMessage = MapiMessage.Load(tempPath))
                {
                    cleanedMessage.Save(outputPath);
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Failed to save cleaned message: {ex.Message}");
                return;
            }
            finally
            {
                // Clean up the temporary file.
                try
                {
                    if (File.Exists(tempPath))
                    {
                        File.Delete(tempPath);
                    }
                }
                catch
                {
                    // Suppress any errors during cleanup.
                }
            }

            Console.WriteLine($"Attachments stripped successfully. Output saved to: {outputPath}");
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
