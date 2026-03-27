using System;
using System.IO;
using Aspose.Email;
using Aspose.Email.Mapi;

class Program
{
    static void Main(string[] args)
    {
        try
        {
            // Output MSG file path
            string outputPath = "output.msg";

            // Ensure the output directory exists
            string directory = Path.GetDirectoryName(outputPath);
            if (!string.IsNullOrEmpty(directory) && !Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }

            // Create a MAPI message with standard fields using the constructor
            using (MapiMessage message = new MapiMessage(
                "sender@example.com",
                "recipient1@example.com;recipient2@example.com",
                "Sample Subject",
                "This is the body of the message.",
                OutlookMessageFormat.Unicode))
            {
                // Additional optional assignments
                message.SenderEmailAddress = "sender@example.com";
                message.Subject = "Sample Subject";
                message.Body = "This is the body of the message.";

                // Save the message to an MSG file
                using (FileStream fileStream = new FileStream(outputPath, FileMode.Create, FileAccess.Write))
                {
                    message.Save(fileStream);
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine(ex.Message);
        }
    }
}
