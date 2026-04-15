using System;
using System.IO;
using Aspose.Email;

class Program
{
    static void Main(string[] args)
    {
        try
        {
            string msgPath = "sample.msg";
            string emlPath = "output.eml";

            if (!File.Exists(msgPath))
            {
                try
                {
                    using (MailMessage placeholder = new MailMessage(
                        "sender@example.com",
                        "recipient@example.com",
                        "Placeholder Subject",
                        "Placeholder body."))
                    {
                        placeholder.Save(msgPath, new MsgSaveOptions(MailMessageSaveType.OutlookMessageFormat));
                    }
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Error creating placeholder MSG: {ex.Message}");
                    return;
                }

                Console.Error.WriteLine($"Input file not found: {msgPath}");
                return;
            }

            try
            {
                using (MailMessage message = MailMessage.Load(msgPath))
                {
                    EmlSaveOptions saveOptions = new EmlSaveOptions(MailMessageSaveType.EmlFormat)
                    {
                        PreserveEmbeddedMessageFormat = true
                    };

                    message.Save(emlPath, saveOptions);
                }

                Console.WriteLine($"MSG file converted to EML successfully: {emlPath}");
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Error during conversion: {ex.Message}");
                return;
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
