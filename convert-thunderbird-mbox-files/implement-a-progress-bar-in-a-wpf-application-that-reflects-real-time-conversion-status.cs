using System;
using System.IO;
using Aspose.Email;
using Aspose.Email.Storage;
using Aspose.Email.Storage.Pst;

class Program
{
    static void Main()
    {
        try
        {
            string emlPath = "sample.eml";
            string msgPath = "output.msg";

            // Ensure input EML file exists
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

                try
                {
                    using (var writer = new StreamWriter(emlPath))
                    {
                        writer.WriteLine("From: sender@example.com");
                        writer.WriteLine("To: recipient@example.com");
                        writer.WriteLine("Subject: Test Email");
                        writer.WriteLine();
                        writer.WriteLine("This is a placeholder email.");
                    }
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to create placeholder EML: {ex.Message}");
                    return;
                }
            }

            // Load the EML file
            MailMessage mailMessage;
            try
            {
                mailMessage = MailMessage.Load(emlPath);
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Failed to load EML file: {ex.Message}");
                return;
            }

            // Prepare save options with a progress handler
            var saveOptions = new MsgSaveOptions(MailMessageSaveType.OutlookMessageFormatUnicode)
            {
                PreserveOriginalDates = true,
                CustomProgressHandler = info =>
                {
                    if (info.TotalMimePartCount > 0)
                    {
                        int percent = (int)((info.SavedMimePartCount / (double)info.TotalMimePartCount) * 100);
                        Console.Write($"\rSaving... {percent}%");
                    }
                }
            };

            // Save as MSG with progress reporting
            try
            {
                mailMessage.Save(msgPath, saveOptions);
                Console.WriteLine("\nConversion completed successfully.");
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Failed to save MSG file: {ex.Message}");
            }
            finally
            {
                mailMessage.Dispose();
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
