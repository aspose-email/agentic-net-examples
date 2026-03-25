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
            string outputPath = "output.msg";
            string outputDirectory = Path.GetDirectoryName(outputPath);
            if (!Directory.Exists(outputDirectory))
            {
                Directory.CreateDirectory(outputDirectory);
            }

            using (Aspose.Email.Amp.AmpMessage ampMessage = new Aspose.Email.Amp.AmpMessage())
            {
                ampMessage.From = new MailAddress("sender@example.com");
                ampMessage.To.Add(new MailAddress("recipient@example.com"));
                ampMessage.Subject = "AMP Email Example";
                ampMessage.Body = "This is the fallback plain text body.";
                ampMessage.AmpHtmlBody = "<!doctype html><html amp4email><head><meta charset=\"utf-8\"><script async src=\"https://cdn.ampproject.org/v0.js\"></script></head><body><h1>Hello AMP Email</h1></body></html>";

                ampMessage.Save(outputPath);
            }

            Console.WriteLine("AMP email saved to: " + outputPath);
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine(ex.Message);
        }
    }
}