using System;
using System.IO;
using Aspose.Email;
using Aspose.Email.Storage;

namespace AsposeEmailSample
{
    class Program
    {
        static void Main()
        {
            try
            {
                // Define file paths
                string msgFilePath = "sample.msg";
                string emlFilePath = "sample.eml";

                // Create a simple MailMessage
                using (MailMessage message = new MailMessage("from@example.com", "to@example.com", "Test Subject", "Hello World"))
                {
                    // Save as MSG using Unicode format
                    MsgSaveOptions msgSaveOptions = new MsgSaveOptions(MailMessageSaveType.OutlookMessageFormatUnicode);
                    message.Save(msgFilePath, msgSaveOptions);

                    // Save as EML (default format)
                    message.Save(emlFilePath);
                }

                // Validate that the MSG file can be reopened
                if (File.Exists(msgFilePath))
                {
                    using (MailMessage loadedMsg = MailMessage.Load(msgFilePath))
                    {
                        Console.WriteLine($"Loaded MSG Subject: {loadedMsg.Subject}");
                    }
                }
                else
                {
                    Console.Error.WriteLine("MSG file not found.");
                }

                // Validate that the EML file can be reopened
                if (File.Exists(emlFilePath))
                {
                    using (MailMessage loadedEml = MailMessage.Load(emlFilePath))
                    {
                        Console.WriteLine($"Loaded EML Subject: {loadedEml.Subject}");
                    }
                }
                else
                {
                    Console.Error.WriteLine("EML file not found.");
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Error: {ex.Message}");
            }
        }
    }
}
