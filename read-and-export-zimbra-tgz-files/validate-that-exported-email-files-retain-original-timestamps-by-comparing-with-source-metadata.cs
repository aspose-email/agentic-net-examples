using System;
using System.IO;
using Aspose.Email;

class Program
{
    static void Main()
    {
        try
        {
            string sourcePath = "source.eml";
            string msgPath = "exported.msg";

            // Ensure source EML exists; create a minimal placeholder if missing
            if (!File.Exists(sourcePath))
            {
                try
                {
                    using (MailMessage placeholder = new MailMessage(
                        "sender@example.com",
                        "recipient@example.com",
                        "Placeholder Subject",
                        "Placeholder body."))
                    {
                        placeholder.Save(sourcePath, SaveOptions.DefaultEml);
                    }
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Error creating placeholder message: {ex.Message}");
                    return;
                }

                try
                {
                    using (StreamWriter writer = new StreamWriter(sourcePath, false))
                    {
                        writer.WriteLine("From: sender@example.com");
                        writer.WriteLine("To: recipient@example.com");
                        writer.WriteLine("Subject: Placeholder Email");
                        writer.WriteLine("Date: " + DateTime.UtcNow.ToString("r"));
                        writer.WriteLine();
                        writer.WriteLine("This is a placeholder email body.");
                    }
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to create placeholder EML file: {ex.Message}");
                    return;
                }
            }

            // Load the source email
            MailMessage sourceMessage;
            try
            {
                sourceMessage = MailMessage.Load(sourcePath);
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Failed to load source email: {ex.Message}");
                return;
            }

            using (sourceMessage)
            {
                DateTime sourceDate = sourceMessage.Date;

                // Save as MSG preserving original dates
                MsgSaveOptions saveOptions = new MsgSaveOptions(MailMessageSaveType.OutlookMessageFormatUnicode)
                {
                    PreserveOriginalDates = true
                };

                try
                {
                    sourceMessage.Save(msgPath, saveOptions);
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to save MSG file: {ex.Message}");
                    return;
                }

                // Load the exported MSG
                MailMessage exportedMessage;
                try
                {
                    exportedMessage = MailMessage.Load(msgPath);
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to load exported MSG: {ex.Message}");
                    return;
                }

                using (exportedMessage)
                {
                    DateTime exportedDate = exportedMessage.Date;

                    // Compare timestamps
                    if (sourceDate == exportedDate)
                    {
                        Console.WriteLine("Success: Exported email retains the original timestamp.");
                    }
                    else
                    {
                        Console.WriteLine("Failure: Timestamp mismatch.");
                        Console.WriteLine($"Source Date:   {sourceDate:O}");
                        Console.WriteLine($"Exported Date: {exportedDate:O}");
                    }
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
