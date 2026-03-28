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
            // Path to the MSG file that contains the Notes document.
            const string msgFilePath = "notes_document.msg";

            // Verify that the file exists before attempting to load it.
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


            // Load the MSG file into a MapiMessage instance.
            using (MapiMessage msg = MapiMessage.Load(msgFilePath))
            {
                // Display basic properties to confirm successful loading.
                Console.WriteLine($"Subject: {msg.Subject}");
                Console.WriteLine($"Body: {msg.Body}");

                // The UNID (Universal ID) of a Notes document is stored as a MAPI property.
                // Its tag is 0x0E03 (PidTagMessageId). Retrieve it as a string.
                // Adjust the tag if a different property is used for UNID in your environment.
                const long UNID_TAG = 0x0E03;
                string unid = msg.GetPropertyString(UNID_TAG);
                Console.WriteLine($"UNID: {unid}");
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
