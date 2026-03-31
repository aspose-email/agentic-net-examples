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

            // Ensure the output directory exists
            string directory = Path.GetDirectoryName(outputPath);
            if (!string.IsNullOrEmpty(directory) && !Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }

            // Create and configure the mail message
            using (MailMessage message = new MailMessage())
            {
                message.From = new MailAddress("sender@example.com");
                message.To.Add(new MailAddress("recipient@example.com"));
                message.Subject = "Sample Message";

                // Set plain‑text body
                message.Body = "This is a plain text body.";

                // Uncomment the following lines to use an HTML body instead
                // message.HtmlBody = "<html><body><h1>Hello, World!</h1></body></html>";
                // message.IsBodyHtml = true;

                // Save the message as an Outlook MSG file
                message.Save(outputPath, new MsgSaveOptions(MailMessageSaveType.OutlookMessageFormat));
            }

            Console.WriteLine("Message saved to " + outputPath);
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine("Error: " + ex.Message);
        }
    }
}
