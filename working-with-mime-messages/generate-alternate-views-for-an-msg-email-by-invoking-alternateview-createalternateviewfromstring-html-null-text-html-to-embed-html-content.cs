using System;
using System.IO;
using Aspose.Email;

class Program
{
    static void Main()
    {
        try
        {
            string outputPath = "output.msg";
            string outputDir = Path.GetDirectoryName(outputPath);
            if (!string.IsNullOrEmpty(outputDir) && !Directory.Exists(outputDir))
            {
                Directory.CreateDirectory(outputDir);
            }

            using (MailMessage message = new MailMessage())
            {
                message.From = "sender@example.com";
                message.To = "recipient@example.com";
                message.Subject = "Sample email with HTML view";

                string htmlContent = "<html><body><h1>Hello</h1><p>This is an HTML view.</p></body></html>";
                AlternateView htmlView = AlternateView.CreateAlternateViewFromString(htmlContent, null, "text/html");
                message.AlternateViews.Add(htmlView);

                try
                {
                    message.Save(outputPath, SaveOptions.DefaultMsgUnicode);
                    Console.WriteLine("Message saved to " + outputPath);
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine("Failed to save message: " + ex.Message);
                    return;
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine("Error: " + ex.Message);
        }
    }
}
