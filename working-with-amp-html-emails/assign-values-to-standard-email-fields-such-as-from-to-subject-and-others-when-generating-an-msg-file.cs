using System;
using System.IO;
using Aspose.Email;
using Aspose.Email.Mapi;

namespace AsposeEmailMsgExample
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                // Output MSG file path
                string outputPath = "GeneratedMessage.msg";

                // Ensure the output directory exists
                string outputDirectory = Path.GetDirectoryName(outputPath);
                if (!string.IsNullOrEmpty(outputDirectory) && !Directory.Exists(outputDirectory))
                {
                    Directory.CreateDirectory(outputDirectory);
                }

                // Create a MapiMessage and assign standard fields
                Aspose.Email.Mapi.MapiMessage message = new Aspose.Email.Mapi.MapiMessage(
                    "sender@example.com",
                    "recipient@example.com",
                    "Sample Subject",
                    "This is the body of the message."
                );

                // Additional field assignments
                message.SenderName = "Sender Name";
                message.SenderEmailAddress = "sender@example.com";

                // Save the message to a file using a FileStream
                using (Aspose.Email.Mapi.MapiMessage disposableMessage = message)
                {
                    using (FileStream fileStream = new FileStream(outputPath, FileMode.Create, FileAccess.Write))
                    {
                        disposableMessage.Save(fileStream);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine(ex.Message);
                return;
            }
        }
    }
}