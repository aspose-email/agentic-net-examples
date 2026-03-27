using System;
using System.IO;
using Aspose.Email;
using Aspose.Email.Amp;

class Program
{
    static void Main()
    {
        try
        {
            string outputPath = "amp_message.msg";
            string directory = Path.GetDirectoryName(outputPath);
            if (!string.IsNullOrEmpty(directory) && !Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }

            using (AmpMessage ampMessage = new AmpMessage())
            {
                ampMessage.From = new MailAddress("sender@example.com", "Sender Name");
                ampMessage.To.Add(new MailAddress("recipient@example.com", "Recipient Name"));
                ampMessage.Subject = "AMP Email Example";
                ampMessage.Body = "This is a plain text body.";
                ampMessage.IsBodyHtml = true;
                ampMessage.HtmlBody = "<p>This is an HTML body.</p>";
                ampMessage.AmpHtmlBody = "<amp-html><h1>Hello AMP</h1></amp-html>";

                using (FileStream fileStream = new FileStream(outputPath, FileMode.Create, FileAccess.Write))
                {
                    ampMessage.Save(fileStream, SaveOptions.DefaultMsg);
                }
            }

            Console.WriteLine("AMP message saved to " + outputPath);
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine("Error: " + ex.Message);
        }
    }
}
