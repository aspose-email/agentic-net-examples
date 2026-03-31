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
            string outputDir = Path.GetDirectoryName(outputPath);
            if (!string.IsNullOrEmpty(outputDir) && !Directory.Exists(outputDir))
            {
                Directory.CreateDirectory(outputDir);
            }

            // Create and populate the email message
            using (MailMessage message = new MailMessage())
            {
                // Standard fields
                message.From = new MailAddress("sender@example.com");
                message.To.Add(new MailAddress("recipient@example.com"));
                message.CC.Add(new MailAddress("cc@example.com"));
                message.Bcc.Add(new MailAddress("bcc@example.com"));
                message.Subject = "Test Subject";
                message.Body = "This is the email body.";

                // Save as MSG file with Unicode format
                MsgSaveOptions saveOptions = new MsgSaveOptions(MailMessageSaveType.OutlookMessageFormatUnicode);
                message.Save(outputPath, saveOptions);
            }

            Console.WriteLine("MSG file created successfully at: " + Path.GetFullPath(outputPath));
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine("Error: " + ex.Message);
        }
    }
}
