using System;
using System.IO;
using Aspose.Email;
using Aspose.Email.Tools.Verifications;

class Program
{
    static void Main()
    {
        try
        {
            // Input EML file path
            string emlPath = "Message.eml";

            // Ensure the input file exists; create a minimal placeholder if missing
            if (!File.Exists(emlPath))
            {
                try
                {
                    string placeholder = "From: placeholder@example.com\r\nTo: recipient@example.com\r\nSubject: Placeholder\r\n\r\nThis is a placeholder email.";
                    File.WriteAllText(emlPath, placeholder);
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to create placeholder EML file: {ex.Message}");
                    return;
                }
            }

            // Output MSG file path
            string msgPath = "Message.msg";

            // Ensure the output directory exists
            string outputDir = Path.GetDirectoryName(msgPath);
            if (!string.IsNullOrEmpty(outputDir) && !Directory.Exists(outputDir))
            {
                try
                {
                    Directory.CreateDirectory(outputDir);
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to create output directory: {ex.Message}");
                    return;
                }
            }

            // Load the email message and save it as MSG with preserved dates
            using (MailMessage mailMessage = MailMessage.Load(emlPath))
            {
                MsgSaveOptions saveOptions = new MsgSaveOptions(MailMessageSaveType.OutlookMessageFormat)
                {
                    PreserveOriginalDates = true
                };

                try
                {
                    mailMessage.Save(msgPath, saveOptions);
                    Console.WriteLine($"Message saved as MSG to: {msgPath}");
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to save MSG file: {ex.Message}");
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
