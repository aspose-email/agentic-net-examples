using System;
using System.IO;
using Aspose.Email;

class Program
{
    static void Main()
    {
        try
        {
            string outputPath = "output.msg";
            string directory = Path.GetDirectoryName(outputPath);
            if (!string.IsNullOrEmpty(directory) && !Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }

            using (Aspose.Email.MailMessage message = new Aspose.Email.MailMessage())
            {
                message.From = new Aspose.Email.MailAddress("sender@example.com");
                message.To.Add(new Aspose.Email.MailAddress("recipient@example.com"));
                message.Subject = "Test Email";
                message.Body = "This is a test email saved as MSG.";

                Aspose.Email.MsgSaveOptions saveOptions = new Aspose.Email.MsgSaveOptions(Aspose.Email.MailMessageSaveType.OutlookMessageFormat);
                message.Save(outputPath, saveOptions);
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine(ex.Message);
        }
    }
}