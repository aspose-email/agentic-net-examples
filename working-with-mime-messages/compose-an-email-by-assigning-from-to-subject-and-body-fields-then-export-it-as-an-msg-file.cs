using System;
using System.IO;
using Aspose.Email;

class Program
{
    static void Main()
    {
        try
        {
            // Define output MSG file path
            string outputPath = "output.msg";

            // Ensure the output directory exists
            string outputDirectory = Path.GetDirectoryName(outputPath);
            if (!string.IsNullOrEmpty(outputDirectory) && !Directory.Exists(outputDirectory))
            {
                Directory.CreateDirectory(outputDirectory);
            }

            // Create and configure the email message
            using (MailMessage message = new MailMessage())
            {
                message.From = new MailAddress("sender@example.com");
                MailAddressCollection toAddresses = new MailAddressCollection();
                toAddresses.Add(new MailAddress("recipient@example.com"));
                message.To = toAddresses;
                message.Subject = "Sample Email";
                message.Body = "This is a sample email body.";

                // Save the message as MSG using MsgSaveOptions
                MsgSaveOptions saveOptions = new MsgSaveOptions(MailMessageSaveType.OutlookMessageFormat);
                message.Save(outputPath, saveOptions);
            }

            Console.WriteLine("Message saved successfully to: " + outputPath);
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine("Error: " + ex.Message);
        }
    }
}
