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
            string msgPath = "sample.msg";

            // Verify that the MSG file exists before attempting to load it.
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


            // Load the MSG file, set the Seen (Read) flag, and save it back.
            using (MapiMessage msg = MapiMessage.Load(msgPath))
            {
                // Set the MSGFLAG_READ flag to mark the message as read/Seen.
                msg.SetMessageFlags(MapiMessageFlags.MSGFLAG_READ);
                msg.Save(msgPath);
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
