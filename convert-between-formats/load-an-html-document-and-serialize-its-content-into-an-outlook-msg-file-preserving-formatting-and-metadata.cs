using System;
using System.IO;
using Aspose.Email;
using Aspose.Email.Mapi;

class Program
{
    static void Main(string[] args)
    {
        try
        {
            const string htmlPath = "sample.html";
            if (!File.Exists(htmlPath))
            {
                Console.Error.WriteLine($"Error: File not found – {htmlPath}");
                return;
            }

            string htmlContent;
            try
            {
                htmlContent = File.ReadAllText(htmlPath);
            }
            catch (Exception readEx)
            {
                Console.Error.WriteLine($"Error reading HTML file: {readEx.Message}");
                return;
            }

            const string msgPath = "output.msg";

            using (MailMessage mail = new MailMessage())
            {
                // Set basic metadata
                mail.From = new MailAddress("sender@example.com");
                mail.To.Add(new MailAddress("recipient@example.com"));
                mail.Subject = "Converted HTML to MSG";

                // Preserve the HTML body
                mail.HtmlBody = htmlContent;

                // Configure MSG save options (Unicode format)
                MsgSaveOptions saveOptions = new MsgSaveOptions(MailMessageSaveType.OutlookMessageFormatUnicode)
                {
                    PreserveOriginalDates = true
                };

                try
                {
                    mail.Save(msgPath, saveOptions);
                    Console.WriteLine($"MSG file saved successfully to {msgPath}");
                }
                catch (Exception saveEx)
                {
                    Console.Error.WriteLine($"Error saving MSG file: {saveEx.Message}");
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
