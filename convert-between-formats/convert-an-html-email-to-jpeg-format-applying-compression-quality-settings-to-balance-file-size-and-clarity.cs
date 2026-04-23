using System;
using System.IO;
using Aspose.Email;
using Aspose.Words;
using Aspose.Words.Saving;

class Program
{
    static void Main()
    {
        try
        {
            string inputHtmlPath = "email.html";
            if (!File.Exists(inputHtmlPath))
            {
                Console.Error.WriteLine($"Input file '{inputHtmlPath}' does not exist.");
                return;
            }

            string htmlContent;
            try
            {
                htmlContent = File.ReadAllText(inputHtmlPath);
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Failed to read input file: {ex.Message}");
                return;
            }

            string tempMhtmlPath = "temp.mhtml";
            try
            {
                using (MailMessage mailMessage = new MailMessage())
                {
                    mailMessage.HtmlBody = htmlContent;
                    mailMessage.Save(tempMhtmlPath, Aspose.Email.SaveOptions.DefaultMhtml);
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Failed to create MHTML file: {ex.Message}");
                return;
            }

            string outputJpegPath = "output.jpeg";
            try
            {
                Document document = new Document(tempMhtmlPath);
            {
                    ImageSaveOptions jpegOptions = new ImageSaveOptions(SaveFormat.Jpeg);
                    jpegOptions.JpegQuality = 80; // Adjust quality to balance size and clarity
                    document.Save(outputJpegPath, jpegOptions);
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Failed to render JPEG: {ex.Message}");
                return;
            }
            finally
            {
                try
                {
                    if (File.Exists(tempMhtmlPath))
                    {
                        File.Delete(tempMhtmlPath);
                    }
                }
                catch
                {
                    // Suppress any cleanup errors
                }
            }

            Console.WriteLine($"JPEG image saved to '{outputJpegPath}'.");
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
