using System;
using System.IO;
using Aspose.Email;

class Program
{
    static void Main()
    {
        try
        {
            string htmlFilePath = "sample.html";
            // Ensure the HTML input file exists; create a minimal placeholder if missing.
            if (!File.Exists(htmlFilePath))
            {
                try
                {
                    File.WriteAllText(htmlFilePath, "<html><body><p>Placeholder content</p></body></html>");
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Error creating placeholder HTML file: {ex.Message}");
                    return;
                }
            }

            // Load the HTML file into a MailMessage object.
            Aspose.Email.HtmlLoadOptions loadOptions = new Aspose.Email.HtmlLoadOptions();
            using (Aspose.Email.MailMessage mailMessage = Aspose.Email.MailMessage.Load(htmlFilePath, loadOptions))
            {
                string mhtmlFilePath = "output.mhtml";

                // Save the MailMessage as MHTML using default options.
                mailMessage.Save(mhtmlFilePath, Aspose.Email.SaveOptions.DefaultMhtml);
                Console.WriteLine($"MHTML file saved to: {mhtmlFilePath}");
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}