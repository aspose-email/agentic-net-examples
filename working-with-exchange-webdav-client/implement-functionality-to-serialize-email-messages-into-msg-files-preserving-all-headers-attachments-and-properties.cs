using System;
using System.IO;
using Aspose.Email;

class Program
{
    static void Main()
    {
        try
        {
            string emlPath = "sample.eml";
            string msgPath = "output.msg";

            // Verify input file exists
            if (!File.Exists(emlPath))
            {
                try
                {
                    using (MailMessage placeholder = new MailMessage(
                        "sender@example.com",
                        "recipient@example.com",
                        "Placeholder Subject",
                        "Placeholder body."))
                    {
                        placeholder.Save(emlPath, SaveOptions.DefaultEml);
                    }
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Error creating placeholder message: {ex.Message}");
                    return;
                }

                Console.Error.WriteLine($"Error: Input file not found – {emlPath}");
                return;
            }

            // Load the email message
            using (MailMessage mail = MailMessage.Load(emlPath))
            {
                // Display all headers
                foreach (string key in mail.Headers.Keys)
                {
                    Console.WriteLine($"{key}: {mail.Headers[key]}");
                }

                // Prepare MSG save options (Unicode MSG format, preserve original dates)
                MsgSaveOptions msgSaveOptions = new MsgSaveOptions(MailMessageSaveType.OutlookMessageFormatUnicode)
                {
                    PreserveOriginalDates = true
                };

                // Save as MSG file
                mail.Save(msgPath, msgSaveOptions);
                Console.WriteLine($"Message saved to {msgPath}");
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
