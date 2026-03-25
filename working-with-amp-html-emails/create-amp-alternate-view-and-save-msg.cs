using System;
using System.IO;
using Aspose.Email;
using Aspose.Email.Amp;
using Aspose.Email.Mime;

class Program
{
    static void Main(string[] args)
    {
        try
        {
            string outputPath = "AmpMessage_out.msg";
            string outputDir = Path.GetDirectoryName(outputPath);
            if (!string.IsNullOrEmpty(outputDir) && !Directory.Exists(outputDir))
            {
                Directory.CreateDirectory(outputDir);
            }

            using (AmpMessage message = new AmpMessage())
            {
                message.From = "sender@example.com";
                message.To.Add("recipient@example.com");
                message.Subject = "AMP Email Example";

                string ampHtml = "<!doctype html><html amp4email><head><meta charset=\"utf-8\"><script async src=\"https://cdn.ampproject.org/v0.js\"></script></head><body><h1>Hello AMP</h1></body></html>";

                Aspose.Email.Mime.ContentType ampContentType = new Aspose.Email.Mime.ContentType("text/x-amp-html");
                AlternateView ampView = AlternateView.CreateAlternateViewFromString(ampHtml, ampContentType);
                message.AlternateViews.Add(ampView);

                try
                {
                    message.Save(outputPath, SaveOptions.DefaultMsgUnicode);
                    Console.WriteLine($"Message saved to {outputPath}");
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Error saving message: {ex.Message}");
                    return;
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}