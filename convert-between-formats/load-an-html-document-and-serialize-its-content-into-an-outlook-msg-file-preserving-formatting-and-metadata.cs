using System;
using System.IO;
using Aspose.Email;

class Program
{
    static void Main()
    {
        try
        {
            string htmlPath = "input.html";
            string msgPath = "output.msg";

            // Ensure the input HTML file exists; create a minimal placeholder if missing
            if (!File.Exists(htmlPath))
            {
                try
                {
                    File.WriteAllText(htmlPath, "<html><body><p>Placeholder</p></body></html>");
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to create placeholder HTML file: {ex.Message}");
                    return;
                }
            }

            // Read HTML content
            string htmlContent;
            try
            {
                htmlContent = File.ReadAllText(htmlPath);
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Failed to read HTML file: {ex.Message}");
                return;
            }

            // Create a MailMessage and populate it
            using (MailMessage mail = new MailMessage())
            {
                mail.Subject = "Converted from HTML";
                mail.HtmlBody = htmlContent;
                mail.From = new MailAddress("sender@example.com");
                mail.To.Add(new MailAddress("recipient@example.com"));

                // Configure MSG save options to preserve original dates
                MsgSaveOptions saveOptions = new MsgSaveOptions(MailMessageSaveType.OutlookMessageFormatUnicode)
                {
                    PreserveOriginalDates = true
                };

                // Save as Outlook MSG
                try
                {
                    mail.Save(msgPath, saveOptions);
                    Console.WriteLine($"MSG file saved to {msgPath}");
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
