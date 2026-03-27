using System;
using System.IO;
using Aspose.Email;
using Aspose.Email.Mapi;

class Program
{
    static void Main()
    {
        try
        {
            // Input HTML file path
            string htmlFilePath = "input.html";
            // Output MSG file path
            string msgFilePath = "output.msg";

            // Ensure the HTML file exists; create a minimal placeholder if missing
            if (!File.Exists(htmlFilePath))
            {
                try
                {
                    File.WriteAllText(htmlFilePath, "<html><body><p>Placeholder content</p></body></html>");
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
                htmlContent = File.ReadAllText(htmlFilePath);
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Failed to read HTML file: {ex.Message}");
                return;
            }

            // Create and populate a MailMessage
            using (MailMessage mailMessage = new MailMessage())
            {
                try
                {
                    mailMessage.From = new MailAddress("sender@example.com");
                    mailMessage.To.Add(new MailAddress("recipient@example.com"));
                    mailMessage.Subject = "Sample Subject";
                    mailMessage.HtmlBody = htmlContent;
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to set MailMessage properties: {ex.Message}");
                    return;
                }

                // Convert MailMessage to MapiMessage
                using (MapiMessage mapiMessage = MapiMessage.FromMailMessage(mailMessage))
                {
                    try
                    {
                        // Save as Outlook MSG file
                        mapiMessage.Save(msgFilePath);
                        Console.WriteLine($"MSG file saved to: {msgFilePath}");
                    }
                    catch (Exception ex)
                    {
                        Console.Error.WriteLine($"Failed to save MSG file: {ex.Message}");
                    }
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}