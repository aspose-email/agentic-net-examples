using System;
using System.IO;
using Aspose.Email;
using Aspose.Email.Calendar;

class Program
{
    static void Main()
    {
        try
        {
            // Prompt the user for the full file path (including file name) where the MSG will be saved.
            Console.WriteLine("Enter the full path (including file name) to save the MSG file:");
            string outputPath = Console.ReadLine();

            if (string.IsNullOrWhiteSpace(outputPath))
            {
                Console.Error.WriteLine("Error: No path provided.");
                return;
            }

            // Ensure the directory part of the path exists.
            string directory = Path.GetDirectoryName(outputPath);
            if (string.IsNullOrEmpty(directory))
            {
                Console.Error.WriteLine("Error: Invalid path.");
                return;
            }

            if (!Directory.Exists(directory))
            {
                try
                {
                    Directory.CreateDirectory(directory);
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Error: Unable to create directory – {ex.Message}");
                    return;
                }
            }

            // Create a simple mail message.
            using (MailMessage message = new MailMessage())
            {
                message.From = new MailAddress("sender@example.com");
                message.To.Add(new MailAddress("recipient@example.com"));
                message.Subject = "Sample MSG";
                message.Body = "This is a sample message saved as MSG.";

                // Configure MSG save options.
                MsgSaveOptions saveOptions = new MsgSaveOptions(MailMessageSaveType.OutlookMessageFormatUnicode)
                {
                    PreserveOriginalDates = true
                };

                // Save the message to the user‑specified location.
                try
                {
                    message.Save(outputPath, saveOptions);
                    Console.WriteLine($"Message saved successfully to: {outputPath}");
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Error: Failed to save message – {ex.Message}");
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
