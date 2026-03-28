using System;
using System.IO;
using Aspose.Email;

class Program
{
    static void Main(string[] args)
    {
        try
        {
            // Define input and output file paths
            string inputPath = "sample.eml";
            string outputPath = "output.html";

            // Ensure the input file exists; create a minimal placeholder if missing
            if (!File.Exists(inputPath))
            {
                string placeholder = "From: example@example.com\r\nTo: recipient@example.com\r\nSubject: Test\r\n\r\nThis is a test email.";
                File.WriteAllText(inputPath, placeholder);
            }

            // Load the email message and save it as HTML
            using (MailMessage message = MailMessage.Load(inputPath))
            {
                message.Save(outputPath, SaveOptions.DefaultHtml);
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine(ex.Message);
        }
    }
}
