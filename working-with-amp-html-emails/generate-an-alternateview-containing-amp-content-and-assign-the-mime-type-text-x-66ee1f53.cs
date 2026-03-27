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

            using (Aspose.Email.Amp.AmpMessage ampMessage = new Aspose.Email.Amp.AmpMessage())
            {
                ampMessage.From = "sender@example.com";
                ampMessage.To = "recipient@example.com";
                ampMessage.Subject = "AMP Email Example";

                string ampHtml = "<amp-html><h1>Hello AMP</h1></amp-html>";
                Aspose.Email.AlternateView ampView = Aspose.Email.AlternateView.CreateAlternateViewFromString(
                    ampHtml,
                    new Aspose.Email.Mime.ContentType("text/x-amp-html")
                );

                ampMessage.AlternateViews.Add(ampView);

                using (FileStream fileStream = new FileStream(outputPath, FileMode.Create, FileAccess.Write))
                {
                    ampMessage.Save(fileStream);
                }

                Console.WriteLine("AMP message saved to " + outputPath);
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine("Error: " + ex.Message);
        }
    }
}