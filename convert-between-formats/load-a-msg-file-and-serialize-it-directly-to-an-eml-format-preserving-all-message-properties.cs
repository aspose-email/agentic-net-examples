using System;
using System.IO;
using Aspose.Email;

class Program
{
    static void Main()
    {
        try
        {
            string inputPath = "input.msg";
            string outputPath = "output.eml";

            // Ensure input MSG exists; create a minimal placeholder if missing
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

                using (MailMessage placeholder = new MailMessage())
                {
                    placeholder.From = new MailAddress("sender@example.com");
                    placeholder.To.Add(new MailAddress("recipient@example.com"));
                    placeholder.Subject = "Placeholder Message";
                    placeholder.Body = "This is a placeholder MSG created because the input file was missing.";
                    placeholder.Save(inputPath);
                }
            }

            // Load the MSG file
            using (MailMessage message = MailMessage.Load(inputPath))
            {
                // Save directly as EML, preserving all properties
                message.Save(outputPath);
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
