using System;
using System.IO;
using Aspose.Email;
using Aspose.Email.Mapi;

namespace AsposeEmailMsgAttachmentFilter
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                // Path to the MSG file (adjust as needed)
                string msgFilePath = "sample.msg";

                // Verify that the MSG file exists before attempting to load it
                if (!File.Exists(msgFilePath))
                {
                try
                {
                    using (MapiMessage placeholder = new MapiMessage(
                        "from@example.com",
                        "to@example.com",
                        "Placeholder Subject",
                        "Placeholder body."))
                    {
                        placeholder.Save(msgFilePath);
                    }
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Error creating placeholder MSG: {ex.Message}");
                    return;
                }

                    Console.Error.WriteLine($"Error: The file '{msgFilePath}' does not exist.");
                    return;
                }

                // Load the MSG file inside a using block to ensure proper disposal
                using (MapiMessage message = MapiMessage.Load(msgFilePath))
                {
                    // Iterate through all attachments and output only PDF files
                    foreach (MapiAttachment attachment in message.Attachments)
                    {
                        if (attachment.FileName != null &&
                            attachment.FileName.EndsWith(".pdf", StringComparison.OrdinalIgnoreCase))
                        {
                            Console.WriteLine($"PDF Attachment: {attachment.FileName}");
                            // Example: Save the PDF attachment to disk (optional)
                            // string outputPath = Path.Combine("Output", attachment.FileName);
                            // attachment.Save(outputPath);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"An error occurred: {ex.Message}");
            }
        }
    }
}
