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
            // Input MSG file path
            string msgFilePath = "sample.msg";

            // Verify input file exists
            if (!File.Exists(msgFilePath))
{
    try
    {
        MailMessage placeholderMsg = new MailMessage("sender@example.com", "recipient@example.com", "Placeholder", "This is a placeholder MSG.");
        placeholderMsg.Save(msgFilePath, SaveOptions.DefaultMsgUnicode);
    }
    catch (Exception ex)
    {
        Console.Error.WriteLine($"Failed to create placeholder MSG: {ex.Message}");
        return;
    }
}


            // Load the MSG file
            using (MapiMessage msg = MapiMessage.Load(msgFilePath))
            {
                // Extract all attachments (including TNEF) to the current directory
                foreach (MapiAttachment attachment in msg.Attachments)
                {
                    // Preserve original file name; if empty, generate a unique name
                    string attachmentFileName = string.IsNullOrEmpty(attachment.FileName)
                        ? $"attachment_{Guid.NewGuid()}"
                        : attachment.FileName;

                    // Save the attachment content
                    attachment.Save(attachmentFileName);
                    Console.WriteLine($"Saved attachment: {attachmentFileName}");
                }

                // Convert the MAPI message to a MailMessage while preserving TNEF data
                MailConversionOptions conversionOptions = new MailConversionOptions();
                conversionOptions.ConvertAsTnef = true;

                using (MailMessage mail = msg.ToMailMessage(conversionOptions))
                {
                    // Example edit: prepend text to the subject
                    mail.Subject = "Edited – " + mail.Subject;

                    // Save the edited message back to MSG format
                    string editedMsgPath = "edited.msg";
                    mail.Save(editedMsgPath, SaveOptions.DefaultMsg);
                    Console.WriteLine($"Edited message saved as: {editedMsgPath}");
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
