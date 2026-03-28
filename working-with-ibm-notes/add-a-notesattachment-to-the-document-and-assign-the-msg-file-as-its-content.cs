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
            // Define paths
            string msgFilePath = "source.msg";
            string outputMsgPath = "parent_with_notes.msg";

            // Verify source MSG file exists
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


            // Load the MSG file to be used as a notes attachment
            using (MapiMessage notesMessage = MapiMessage.Load(msgFilePath))
            {
                // Create a new parent MAPI message
                using (MapiMessage parentMessage = new MapiMessage(
                    "Parent Subject",
                    "This is the body of the parent message.",
                    "sender@example.com",
                    "receiver@example.com"))
                {
                    // Add the loaded MSG as an embedded (notes) attachment
                    parentMessage.Attachments.Add("NotesAttachment.msg", notesMessage);

                    // Save the parent message with the notes attachment
                    parentMessage.Save(outputMsgPath);
                    Console.WriteLine($"Parent message saved with notes attachment to: {outputMsgPath}");
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
