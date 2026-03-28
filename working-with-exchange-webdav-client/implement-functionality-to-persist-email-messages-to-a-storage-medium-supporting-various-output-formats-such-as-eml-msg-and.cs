using System;
using System.IO;
using Aspose.Email;

class Program
{
    static void Main()
    {
        try
        {
            // Define input and output file paths
            string inputPath = "sample.eml";
            string outputEmlPath = "output.eml";
            string outputMsgPath = "output.msg";
            string outputMhtmlPath = "output.mhtml";

            // Ensure the input file exists; create a minimal placeholder if missing
            if (!File.Exists(inputPath))
            {
                try
                {
                    string placeholder = "From: test@example.com\r\nTo: test@example.com\r\nSubject: Test Email\r\n\r\nThis is a test email.";
                    File.WriteAllText(inputPath, placeholder);
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to create placeholder input file: {ex.Message}");
                    return;
                }
            }

            // Ensure output directories exist
            try
            {
                string outputDir = Path.GetDirectoryName(outputEmlPath);
                if (!string.IsNullOrEmpty(outputDir) && !Directory.Exists(outputDir))
                {
                    Directory.CreateDirectory(outputDir);
                }
                outputDir = Path.GetDirectoryName(outputMsgPath);
                if (!string.IsNullOrEmpty(outputDir) && !Directory.Exists(outputDir))
                {
                    Directory.CreateDirectory(outputDir);
                }
                outputDir = Path.GetDirectoryName(outputMhtmlPath);
                if (!string.IsNullOrEmpty(outputDir) && !Directory.Exists(outputDir))
                {
                    Directory.CreateDirectory(outputDir);
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Failed to ensure output directories: {ex.Message}");
                return;
            }

            // Load the email message
            MailMessage message;
            try
            {
                message = MailMessage.Load(inputPath);
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Failed to load email from '{inputPath}': {ex.Message}");
                return;
            }

            // Use using to ensure disposal of the MailMessage
            using (message)
            {
                // Save as EML (default format)
                try
                {
                    message.Save(outputEmlPath);
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to save EML file: {ex.Message}");
                }

                // Save as MSG using appropriate SaveOptions
                try
                {
                    SaveOptions msgOptions = SaveOptions.CreateSaveOptions(MailMessageSaveType.OutlookMessageFormat);
                    message.Save(outputMsgPath, msgOptions);
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to save MSG file: {ex.Message}");
                }

                // Save as MHTML using appropriate SaveOptions
                try
                {
                    SaveOptions mhtmlOptions = SaveOptions.CreateSaveOptions(MailMessageSaveType.MHtmlFormat);
                    message.Save(outputMhtmlPath, mhtmlOptions);
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to save MHTML file: {ex.Message}");
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
