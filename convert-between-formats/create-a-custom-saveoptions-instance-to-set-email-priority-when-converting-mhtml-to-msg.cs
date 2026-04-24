using System;
using System.IO;
using Aspose.Email;

class Program
{
    static void Main()
    {
        try
        {
            string inputPath = "input.mht";
            string outputPath = "output.msg";

            // Ensure the input MHTML file exists; create a minimal placeholder if missing.
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
                        placeholder.Save(inputPath, SaveOptions.DefaultEml);
                    }
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Error creating placeholder message: {ex.Message}");
                    return;
                }

                try
                {
                    File.WriteAllText(inputPath, "<html><body><p>Placeholder MHTML content</p></body></html>");
                }
                catch (Exception ioEx)
                {
                    Console.Error.WriteLine($"Failed to create placeholder MHTML file: {ioEx.Message}");
                    return;
                }
            }

            // Load the MHTML message.
            using (MailMessage message = MailMessage.Load(inputPath))
            {
                // Set the email priority.
                message.Priority = MailPriority.High;

                // Create custom save options for MSG format.
                MsgSaveOptions saveOptions = new MsgSaveOptions(MailMessageSaveType.OutlookMessageFormatUnicode);

                // Save the message as MSG with the specified options.
                try
                {
                    message.Save(outputPath, saveOptions);
                }
                catch (Exception saveEx)
                {
                    Console.Error.WriteLine($"Failed to save MSG file: {saveEx.Message}");
                    return;
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
