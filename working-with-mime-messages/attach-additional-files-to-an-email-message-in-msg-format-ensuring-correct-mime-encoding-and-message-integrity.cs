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
            // Output MSG file path
            string outputMsgPath = "output.msg";

            // Ensure the output directory exists
            string outputDir = Path.GetDirectoryName(outputMsgPath);
            if (!string.IsNullOrEmpty(outputDir) && !Directory.Exists(outputDir))
            {
                Directory.CreateDirectory(outputDir);
            }

            // Create a new MAPI message
            using (MapiMessage message = new MapiMessage(
                "sender@example.com",
                "receiver@example.com",
                "Test Subject",
                "This is the body of the message."))
            {
                // Path to the attachment file
                string attachmentFilePath = "sample.txt";

                // Verify the attachment file exists; create a minimal placeholder if missing
                if (!File.Exists(attachmentFilePath))
                {
                    try
                    {
                        File.WriteAllText(attachmentFilePath, "Placeholder content");
                    }
                    catch (Exception ex)
                    {
                        Console.Error.WriteLine($"Error creating placeholder attachment: {ex.Message}");
                        return;
                    }
                }

                // Read the attachment data
                byte[] attachmentData;
                try
                {
                    attachmentData = File.ReadAllBytes(attachmentFilePath);
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Error reading attachment file: {ex.Message}");
                    return;
                }

                // Add the attachment to the message (name + byte[] overload)
                message.Attachments.Add(Path.GetFileName(attachmentFilePath), attachmentData);

                // Save the message as MSG
                try
                {
                    message.Save(outputMsgPath);
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Error saving MSG file: {ex.Message}");
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
