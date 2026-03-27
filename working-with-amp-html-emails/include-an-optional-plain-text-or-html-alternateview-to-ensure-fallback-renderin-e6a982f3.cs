using System;
using System.IO;
using Aspose.Email;
using Aspose.Email.Amp;
using Aspose.Email.Mime;

class Program
{
    static void Main()
    {
        try
        {
            string outputPath = "AmpEmail_out.msg";
            string outputDir = Path.GetDirectoryName(outputPath);
            if (!string.IsNullOrEmpty(outputDir) && !Directory.Exists(outputDir))
            {
                Directory.CreateDirectory(outputDir);
            }

            using (Aspose.Email.Amp.AmpMessage ampMessage = new Aspose.Email.Amp.AmpMessage())
            {
                ampMessage.From = "sender@example.com";
                ampMessage.To.Add("recipient@example.com");
                ampMessage.Subject = "AMP Email Example";

                // Plain‑text alternate view
                using (Aspose.Email.AlternateView plainView = Aspose.Email.AlternateView.CreateAlternateViewFromString(
                    "This is the plain text version.",
                    new Aspose.Email.Mime.ContentType("text/plain")))
                {
                    ampMessage.AlternateViews.Add(plainView);
                }

                // HTML alternate view
                using (Aspose.Email.AlternateView htmlView = Aspose.Email.AlternateView.CreateAlternateViewFromString(
                    "<html><body><h1>Hello AMP</h1></body></html>",
                    new Aspose.Email.Mime.ContentType("text/html")))
                {
                    ampMessage.AlternateViews.Add(htmlView);
                }

                // AMP HTML body
                ampMessage.AmpHtmlBody = "<amp-html><h1>AMP Content</h1></amp-html>";

                // Save the message using the stream overload
                using (FileStream fileStream = new FileStream(outputPath, FileMode.Create, FileAccess.Write))
                {
                    ampMessage.Save(fileStream, Aspose.Email.SaveOptions.DefaultMsgUnicode);
                }
            }

            Console.WriteLine("AMP email saved to " + outputPath);
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine("Error: " + ex.Message);
        }
    }
}