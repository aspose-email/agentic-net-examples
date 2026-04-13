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
            string inputMsgPath = @"C:\Temp\sample.msg";
            string attachmentsOutputDir = @"C:\Temp\Attachments";

            // Verify input MSG file exists
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

                Console.Error.WriteLine($"Input MSG file not found: {inputMsgPath}");
                return;
            }

            // Ensure output directory exists
            if (!Directory.Exists(attachmentsOutputDir))
            {
                try
                {
                    Directory.CreateDirectory(attachmentsOutputDir);
                }
                catch (Exception dirEx)
                {
                    Console.Error.WriteLine($"Failed to create output directory: {dirEx.Message}");
                    return;
                }
            }

            // Load the MSG file
            using (MapiMessage msg = MapiMessage.Load(inputMsgPath))
            {
                // Save each attachment to the external directory
                foreach (MapiAttachment attachment in msg.Attachments)
                {
                    try
                    {
                        string destPath = Path.Combine(attachmentsOutputDir, attachment.FileName);
                        attachment.Save(destPath);
                        Console.WriteLine($"Saved attachment: {destPath}");
                    }
                    catch (Exception attEx)
                    {
                        Console.Error.WriteLine($"Failed to save attachment '{attachment.FileName}': {attEx.Message}");
                    }
                }
            }

            // Remove attachments from the original MSG file
            try
            {
                MapiAttachmentCollection removed = MapiMessage.RemoveAttachments(inputMsgPath);
                Console.WriteLine($"Removed {removed.Count} attachment(s) from the MSG file.");
            }
            catch (Exception remEx)
            {
                Console.Error.WriteLine($"Failed to remove attachments from MSG file: {remEx.Message}");
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
