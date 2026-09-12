using Aspose.Email;
using System;
using System.IO;
using Aspose.Email.Mapi;

class Program
{
    static void Main()
    {
        try
        {
            // Author note: Example demonstrates extracting all attachments from a MSG file.
            string inputMsgPath = "c:\\outlookmessage.msg";
            string outputFolder = "Attachments";

            // Verify input file exists
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

                Console.Error.WriteLine($"Input file not found: {inputMsgPath}");
                return;
            }

            // Ensure output directory exists
            try
            {
                Directory.CreateDirectory(outputFolder);
            }
            catch (Exception dirEx)
            {
                Console.Error.WriteLine($"Failed to create output directory '{outputFolder}': {dirEx.Message}");
                return;
            }

            // Load the MSG file
            MapiMessage msg = MapiMessage.Load(inputMsgPath);

            // Iterate through attachments and save each to disk
            foreach (MapiAttachment att in msg.Attachments)
            {
                Console.WriteLine($"Attachment Name: {att.FileName}");
                string outputPath = Path.Combine(outputFolder, att.FileName);

                try
                {
                    att.Save(outputPath);
                }
                catch (Exception saveEx)
                {
                    Console.Error.WriteLine($"Failed to save attachment '{att.FileName}': {saveEx.Message}");
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
