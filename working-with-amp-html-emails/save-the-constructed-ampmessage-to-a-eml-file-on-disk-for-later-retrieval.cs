using System;
using System.IO;
using Aspose.Email;
using Aspose.Email.Amp;

class Program
{
    static void Main(string[] args)
    {
        try
        {
            string outputPath = "output.eml";
            string directoryPath = Path.GetDirectoryName(outputPath);
            if (!string.IsNullOrEmpty(directoryPath) && !Directory.Exists(directoryPath))
            {
                Directory.CreateDirectory(directoryPath);
            }

            using (AmpMessage ampMessage = new AmpMessage())
            {
                ampMessage.From = "sender@example.com";
                ampMessage.To.Add("recipient@example.com");
                ampMessage.Subject = "Test AMP Email";
                ampMessage.Body = "This is a plain text body.";
                ampMessage.IsBodyHtml = false;
                ampMessage.AmpHtmlBody = "<amp-html><h1>Hello AMP</h1></amp-html>";

                ampMessage.Save(outputPath);
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine(ex.Message);
        }
    }
}
