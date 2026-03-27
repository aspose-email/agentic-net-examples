using System;
using System.IO;
using Aspose.Email;

class Program
{
    static void Main()
    {
        try
        {
            string msgPath = "sample.msg";

            if (!File.Exists(msgPath))
            {
                Console.Error.WriteLine($"Error: File not found – {msgPath}");
                return;
            }

            // Load the MSG file into a MailMessage object
            using (MailMessage message = MailMessage.Load(msgPath, new MsgLoadOptions()))
            {
                string htmlPath = Path.ChangeExtension(msgPath, ".html");

                // Save the message as HTML, preserving formatting and attachments
                message.Save(htmlPath, SaveOptions.DefaultHtml);

                Console.WriteLine($"HTML file saved to: {htmlPath}");
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
