using System;
using System.IO;
using Aspose.Email;
using Aspose.Email.Mime;

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
                try
                {
                    using (MailMessage placeholder = new MailMessage(
                        "sender@example.com",
                        "recipient@example.com",
                        "Placeholder Subject",
                        "Placeholder body."))
                    {
                        placeholder.Save(inputPath, new MsgSaveOptions(MailMessageSaveType.OutlookMessageFormat));
                    }
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Error creating placeholder MSG: {ex.Message}");
                    return;
                }

                using (var placeholder = new MailMessage())
                {
                    placeholder.From = new MailAddress("sender@example.com");
                    placeholder.To.Add(new MailAddress("recipient@example.com"));
                    placeholder.Subject = "Placeholder Subject";
                    placeholder.Body = "Placeholder body.";
                    placeholder.Save(inputPath, SaveOptions.DefaultMsgUnicode);
                }
            }

            using (var message = MailMessage.Load(inputPath))
            {
                message.Priority = MailPriority.High;
                message.Save(outputPath, SaveOptions.DefaultMsgUnicode);
            }

            Console.WriteLine("Message saved with high priority.");
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
