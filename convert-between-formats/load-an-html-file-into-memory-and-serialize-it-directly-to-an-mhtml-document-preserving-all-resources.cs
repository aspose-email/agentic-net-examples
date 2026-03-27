using System;
using System.IO;
using Aspose.Email;

class Program
{
    static void Main(string[] args)
    {
        try
        {
            string htmlPath = "sample.html";
            string mhtmlPath = "output.mhtml";

            // Ensure the HTML file exists; create a minimal placeholder if missing.
            if (!File.Exists(htmlPath))
            {
                try
                {
                    File.WriteAllText(htmlPath, "<html><body><p>Placeholder</p></body></html>");
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Error creating placeholder HTML file: {ex.Message}");
                    return;
                }
            }

            try
            {
                // Load the HTML file into a MailMessage.
                HtmlLoadOptions loadOptions = new HtmlLoadOptions();
                using (MailMessage message = MailMessage.Load(htmlPath, loadOptions))
                {
                    // Save the message as MHTML, preserving all resources.
                    message.Save(mhtmlPath, SaveOptions.DefaultMhtml);
                    Console.WriteLine($"MHTML file saved to {mhtmlPath}");
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Error processing email: {ex.Message}");
                return;
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
