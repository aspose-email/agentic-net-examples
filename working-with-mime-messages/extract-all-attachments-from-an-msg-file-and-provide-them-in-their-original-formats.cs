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
            // Path to the MSG file
            string msgPath = "sample.msg";

            // Verify that the MSG file exists before attempting to load it
            if (!File.Exists(msgPath))
            {
                Console.Error.WriteLine($"The file '{msgPath}' does not exist.");
                return;
            }

            // Load the MSG file and extract its attachments
            using (MapiMessage msg = MapiMessage.Load(msgPath))
            {
                MapiAttachmentCollection attachments = msg.Attachments;

                if (attachments == null || attachments.Count == 0)
                {
                    Console.WriteLine("No attachments found in the message.");
                    return;
                }

                foreach (MapiAttachment attachment in attachments)
                {
                    // Output attachment information
                    Console.WriteLine($"Saving attachment: {attachment.FileName}");

                    // Save the attachment using its original file name
                    try
                    {
                        attachment.Save(attachment.FileName);
                    }
                    catch (Exception ex)
                    {
                        Console.Error.WriteLine($"Failed to save attachment '{attachment.FileName}': {ex.Message}");
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
