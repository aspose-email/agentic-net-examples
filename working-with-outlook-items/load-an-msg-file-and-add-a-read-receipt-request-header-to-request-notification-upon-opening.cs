using System;
using System.IO;
using Aspose.Email;
using Aspose.Email.Storage;

class Program
{
    static void Main()
    {
        try
        {
            const string inputPath = "sample.msg";
            const string outputPath = "sample_modified.msg";

            // Guard file existence
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

                Console.Error.WriteLine($"Input file not found: {inputPath}");
                return;
            }

            try
            {
                // Load MSG as MailMessage
                using (MailMessage mail = MailMessage.Load(inputPath))
                {
                    // Add a read receipt request header
                    mail.Headers.Add(HeaderType.XConfirmReadingTo, "mailto:sender@example.com");

                    // Display all headers using the required iteration pattern
                    foreach (string key in mail.Headers.Keys)
                    {
                        Console.WriteLine($"{key}: {mail.Headers[key]}");
                    }

                    // Save the modified message back to MSG format
                    mail.Save(outputPath, SaveOptions.DefaultMsgUnicode);
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Error processing message: {ex.Message}");
                return;
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
