using System;
using System.IO;
using Aspose.Email;

namespace AsposeEmailMsgSerialization
{
    // Author: Aspose.Email example for MSG serialization with full header, attachment, and property preservation.
    class Program
    {
        static void Main()
        {
            const string inputPath = "input.eml";
            const string outputPath = "output.msg";

            // Ensure the output directory exists.
            try
            {
                string? outputDir = Path.GetDirectoryName(outputPath);
                if (!string.IsNullOrEmpty(outputDir) && !Directory.Exists(outputDir))
                {
                    Directory.CreateDirectory(outputDir);
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Failed to prepare output directory: {ex.Message}");
                return;
            }

            MailMessage message = null;
            try
            {
                if (File.Exists(inputPath))
                {
                    // Load existing EML file.
                    message = MailMessage.Load(inputPath);
                }
                else
                {
                    // Create a minimal placeholder message when the source file is missing.
                    message = new MailMessage
                    {
                        From = new MailAddress("sender@example.com"),
                        To = new MailAddressCollection { new MailAddress("recipient@example.com") },
                        Subject = "Placeholder Subject",
                        Body = "This is a placeholder email generated because the input file was not found."
                    };
                }

                // Preserve original dates and use Unicode MSG format.
                MsgSaveOptions saveOptions = new MsgSaveOptions(MailMessageSaveType.OutlookMessageFormatUnicode)
                {
                    PreserveOriginalDates = true
                };

                // Save the message as MSG with all headers, attachments, and properties preserved.
                message.Save(outputPath, saveOptions);
                Console.WriteLine($"Message successfully saved to '{outputPath}'.");
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Error processing email: {ex.Message}");
            }
            finally
            {
                // Dispose the MailMessage if it implements IDisposable.
                if (message != null)
                {
                    message.Dispose();
                }
            }
        }
    }
}
