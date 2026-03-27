using System;
using System.IO;
using System.Text;
using Aspose.Email;
using Aspose.Email.Mapi;

class Program
{
    static void Main()
    {
        try
        {
            string inputPath = "input.msg";
            string outputPath = "output.msg";

            if (!File.Exists(inputPath))
            {
                Console.Error.WriteLine($"Error: File not found – {inputPath}");
                return;
            }

            using (MapiMessage message = MapiMessage.Load(inputPath))
            {
                string attachmentName = "Sample.txt";
                string attachmentText = "This is a sample attachment.";
                byte[] attachmentData = Encoding.UTF8.GetBytes(attachmentText);

                // Add the attachment to the message
                message.Attachments.Add(attachmentName, attachmentData);

                // Save the updated message
                message.Save(outputPath);
                Console.WriteLine($"Attachment added and message saved to {outputPath}");
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine(ex.Message);
        }
    }
}
