using System;
using System.IO;
using Aspose.Email;

class Program
{
    static void Main()
    {
        try
        {
            string emlPath = "Message.eml";
            string msgPath = "outTest_out.msg";

            // Ensure the input EML file exists; create a minimal placeholder if missing.
            if (!File.Exists(emlPath))
            {
                try
                {
                    string placeholder = "From: test@example.com\r\nTo: test@example.com\r\nSubject: Test\r\n\r\nBody of the email.";
                    File.WriteAllText(emlPath, placeholder);
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to create placeholder EML file: {ex.Message}");
                    return;
                }
            }

            // Load the email, set high priority, and save as MSG.
            try
            {
                using (MailMessage mailMessage = MailMessage.Load(emlPath))
                {
                    mailMessage.Priority = MailPriority.High;

                    MsgSaveOptions saveOptions = new MsgSaveOptions(MailMessageSaveType.OutlookMessageFormatUnicode);
                    mailMessage.Save(msgPath, saveOptions);
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Error processing email files: {ex.Message}");
                return;
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
