using System;
using System.IO;
using Aspose.Email;

class Program
{
    static void Main()
    {
        try
        {
            string htmlPath = "sample.html";
            string msgPath = "output.msg";

            // Ensure the HTML source file exists; create a minimal placeholder if missing.
            if (!File.Exists(htmlPath))
            {
                try
                {
                    File.WriteAllText(htmlPath, "<html><body><p>Hello World</p></body></html>");
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to create placeholder HTML file: {ex.Message}");
                    return;
                }
            }

            // Load the HTML document into a MailMessage.
            MailMessage mailMessage;
            try
            {
                HtmlLoadOptions htmlLoadOptions = new HtmlLoadOptions();
                mailMessage = MailMessage.Load(htmlPath, htmlLoadOptions);
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Failed to load HTML file: {ex.Message}");
                return;
            }

            using (mailMessage)
            {
                // Populate essential metadata if not already present.
                if (mailMessage.From == null)
                {
                    mailMessage.From = new MailAddress("sender@example.com", "Sender");
                }

                if (mailMessage.To == null || mailMessage.To.Count == 0)
                {
                    mailMessage.To.Add(new MailAddress("recipient@example.com", "Recipient"));
                }

                if (string.IsNullOrEmpty(mailMessage.Subject))
                {
                    mailMessage.Subject = "Converted HTML to MSG";
                }

                // Save the message as an Outlook MSG file, preserving formatting.
                try
                {
                    mailMessage.Save(msgPath, SaveOptions.DefaultMsg);
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to save MSG file: {ex.Message}");
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
