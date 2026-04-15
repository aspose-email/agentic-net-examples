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

                Console.Error.WriteLine($"Input file not found: {inputPath}");
                return;
            }

            try
            {
                using (MailMessage message = MailMessage.Load(inputPath))
                {
                    string body = message.Body ?? string.Empty;
                    // Normalize line endings to CRLF
                    string normalized = body.Replace("\r\n", "\n").Replace("\n", "\r\n");
                    message.Body = normalized;

                    // Save the modified message
                    message.Save(outputPath, SaveOptions.DefaultMsg);
                }
            }
            catch (Exception ioEx)
            {
                Console.Error.WriteLine($"Error processing message: {ioEx.Message}");
                return;
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
