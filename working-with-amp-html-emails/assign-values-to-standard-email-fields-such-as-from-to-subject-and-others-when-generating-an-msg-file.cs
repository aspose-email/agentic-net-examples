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
            string outputFile = "Message.msg";
            string outputDir = Path.GetDirectoryName(outputFile);
            if (!string.IsNullOrEmpty(outputDir) && !Directory.Exists(outputDir))
            {
                Directory.CreateDirectory(outputDir);
            }

            // Create a new MAPI message with standard fields
            using (MapiMessage message = new MapiMessage(
                "sender@example.com",
                "recipient@example.com",
                "Sample Subject",
                "This is the message body.",
                OutlookMessageFormat.Unicode))
            {
                // Assign additional standard fields
                message.Subject = "Updated Subject";
                message.Body = "Updated body content.";
                message.SenderEmailAddress = "sender@example.com";
                message.SenderName = "Sender Name";

                // Save the message to an MSG file
                using (FileStream stream = new FileStream(outputFile, FileMode.Create, FileAccess.Write))
                {
                    message.Save(stream);
                }
            }

            Console.WriteLine("MSG file created at: " + Path.GetFullPath(outputFile));
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine("Error: " + ex.Message);
        }
    }
}
