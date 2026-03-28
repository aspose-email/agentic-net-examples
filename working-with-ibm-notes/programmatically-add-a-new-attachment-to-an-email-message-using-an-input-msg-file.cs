using System;
using System.IO;
using Aspose.Email;

class Program
{
    static void Main()
    {
        try
        {
            string inputMsgPath = "input.msg";
            string attachmentFilePath = "newAttachment.txt";
            string outputMsgPath = "output.msg";

            // Verify input MSG file exists
            if (!File.Exists(inputMsgPath))
{
    try
    {
        MailMessage placeholderMsg = new MailMessage("sender@example.com", "recipient@example.com", "Placeholder", "This is a placeholder MSG.");
        placeholderMsg.Save(inputMsgPath, SaveOptions.DefaultMsgUnicode);
    }
    catch (Exception ex)
    {
        Console.Error.WriteLine($"Failed to create placeholder MSG: {ex.Message}");
        return;
    }
}


            // Verify attachment file exists
            if (!File.Exists(attachmentFilePath))
            {
                Console.Error.WriteLine($"Error: Attachment file not found – {attachmentFilePath}");
                return;
            }

            // Load the existing MSG message
            using (MailMessage message = MailMessage.Load(inputMsgPath))
            {
                // Create attachment and add it to the message
                using (Attachment attachment = new Attachment(attachmentFilePath))
                {
                    message.AddAttachment(attachment);
                }

                // Save the updated message to a new MSG file
                message.Save(outputMsgPath);
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
