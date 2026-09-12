using Aspose.Email;
using System;
using System.IO;
using Aspose.Email.Mapi;

namespace AsposeEmailAttachmentSaver
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                // Input MSG file path
                string inputMsgPath = "input.msg";

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

                // Load the Outlook message
                MapiMessage mapMsg = MapiMessage.Load(inputMsgPath);

                // Get the attachment collection
                MapiAttachmentCollection attachments = mapMsg.Attachments;

                // Process each attachment
                foreach (MapiAttachment att in attachments)
                {
                    // Build prefixed output file name
                    string prefixedFileName = "output_" + att.FileName;
                    string outputPath = Path.Combine(Path.GetDirectoryName(inputMsgPath) ?? ".", prefixedFileName);

                    // Ensure the output directory exists
                    string outputDir = Path.GetDirectoryName(outputPath);
                    if (!string.IsNullOrEmpty(outputDir) && !Directory.Exists(outputDir))
                    {
                        Directory.CreateDirectory(outputDir);
                    }

                    // Save the attachment to disk
                    att.Save(outputPath);
                    Console.WriteLine($"Saved attachment to: {outputPath}");
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Error: {ex.Message}");
            }
        }
    }
}
