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
            string inputMsgPath = "sample.msg";

            // Verify input file exists
            if (!File.Exists(inputMsgPath))
            {
                Console.Error.WriteLine($"Input file not found: {inputMsgPath}");
                return;
            }

            // Load the MSG file and process attachments
            try
            {
                using (MapiMessage message = MapiMessage.Load(inputMsgPath))
                {
                    foreach (MapiAttachment attachment in message.Attachments)
                    {
                        // Ensure attachment has a file name
                        string originalName = attachment.FileName;
                        if (string.IsNullOrEmpty(originalName))
                        {
                            originalName = "attachment.bin";
                        }

                        string outputFileName = $"output_{originalName}";
                        try
                        {
                            // Save the attachment to the prefixed file name
                            attachment.Save(outputFileName);
                            Console.WriteLine($"Saved attachment to {outputFileName}");
                        }
                        catch (Exception ex)
                        {
                            Console.Error.WriteLine($"Failed to save attachment '{originalName}': {ex.Message}");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Error processing MSG file: {ex.Message}");
                return;
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
