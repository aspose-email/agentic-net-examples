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
            // Path to the source MSG file
            string msgPath = @"c:\outlookmessage.msg";

            // Verify that the MSG file exists
            if (!File.Exists(msgPath))
            {
                Console.Error.WriteLine($"Error: File not found – {msgPath}");
                return;
            }

            // Ensure the output directory exists
            string outputDir = @"c:\Attachments";
            if (!Directory.Exists(outputDir))
            {
                Directory.CreateDirectory(outputDir);
            }

            // Load the MSG file
            using (MapiMessage msg = MapiMessage.Load(msgPath))
            {
                // Iterate through each attachment
                foreach (MapiAttachment att in msg.Attachments)
                {
                    // Determine the full path for the saved attachment
                    string attachmentPath = Path.Combine(outputDir, att.FileName);

                    // Save the attachment to disk
                    att.Save(attachmentPath);
                    Console.WriteLine($"Saved attachment: {attachmentPath}");
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
