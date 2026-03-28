using System;
using System.IO;
using Aspose.Email;

class Program
{
    static void Main()
    {
        try
        {
            string inputHtmlPath = "input.html";
            string outputMsgPath = "output.msg";

            // Ensure the input HTML file exists; create a minimal placeholder if missing.
            if (!File.Exists(inputHtmlPath))
            {
                try
                {
                    File.WriteAllText(inputHtmlPath, "<html><body><p>Placeholder email content.</p></body></html>");
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to create placeholder HTML file: {ex.Message}");
                    return;
                }
            }

            // Load the HTML email and convert it to MSG format.
            try
            {
                using (MailMessage message = MailMessage.Load(inputHtmlPath, new HtmlLoadOptions()))
                {
                    // Save as Outlook MSG (Unicode) format.
                    message.Save(outputMsgPath, SaveOptions.DefaultMsgUnicode);
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Error during conversion: {ex.Message}");
                return;
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
