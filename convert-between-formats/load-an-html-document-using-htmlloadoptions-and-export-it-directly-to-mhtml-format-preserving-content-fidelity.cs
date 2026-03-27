using System;
using System.IO;
using Aspose.Email;

class Program
{
    static void Main()
    {
        try
        {
            string htmlFilePath = "input.html";
            string mhtmlFilePath = "output.mhtml";

            // Verify that the source HTML file exists
            if (!File.Exists(htmlFilePath))
            {
                Console.Error.WriteLine($"Source file not found: {htmlFilePath}");
                return;
            }

            // Load the HTML document into a MailMessage using HtmlLoadOptions
            using (MailMessage message = MailMessage.Load(htmlFilePath, new HtmlLoadOptions()))
            {
                // Save the MailMessage directly to MHTML format with default options
                message.Save(mhtmlFilePath, SaveOptions.DefaultMhtml);
                Console.WriteLine($"MHTML file saved to: {mhtmlFilePath}");
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}