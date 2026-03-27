using System;
using System.IO;
using Aspose.Email;
using Aspose.Email.Amp;

namespace AmpMessageExample
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                string outputPath = "AmpMessage.msg";
                string directory = Path.GetDirectoryName(outputPath);
                if (!string.IsNullOrEmpty(directory) && !Directory.Exists(directory))
                {
                    Directory.CreateDirectory(directory);
                }

                using (Aspose.Email.Amp.AmpMessage ampMessage = new Aspose.Email.Amp.AmpMessage())
                {
                    ampMessage.From = new MailAddress("sender@example.com", "Sender Name");
                    ampMessage.To.Add(new MailAddress("recipient@example.com", "Recipient Name"));
                    ampMessage.Subject = "AMP Email Example";
                    ampMessage.Body = "This is the plain text body.";
                    ampMessage.IsBodyHtml = true;
                    ampMessage.HtmlBody = "<p>This is the HTML body.</p>";
                    ampMessage.AmpHtmlBody = "<amp-html><h1>Hello AMP</h1></amp-html>";

                    try
                    {
                        ampMessage.Save(outputPath);
                        Console.WriteLine("AMP message saved to " + outputPath);
                    }
                    catch (Exception ex)
                    {
                        Console.Error.WriteLine("Error saving AMP message: " + ex.Message);
                        return;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine("Unexpected error: " + ex.Message);
            }
        }
    }
}