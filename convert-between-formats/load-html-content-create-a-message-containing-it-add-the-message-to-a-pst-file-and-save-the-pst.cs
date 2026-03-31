using System;
using System.IO;
using Aspose.Email;
using Aspose.Email.Storage.Pst;
using Aspose.Email.Mapi;

class Program
{
    static void Main(string[] args)
    {
        try
        {
            // Define input HTML file path
            string htmlFilePath = "input.html";

            // Ensure the HTML file exists; create a minimal placeholder if missing
            if (!File.Exists(htmlFilePath))
            {
                try
                {
                    File.WriteAllText(htmlFilePath, "<html><body><p>Placeholder HTML content.</p></body></html>");
                }
                catch (Exception ioEx)
                {
                    Console.Error.WriteLine($"Failed to create placeholder HTML file: {ioEx.Message}");
                    return;
                }
            }

            // Read HTML content
            string htmlContent;
            try
            {
                htmlContent = File.ReadAllText(htmlFilePath);
            }
            catch (Exception ioEx)
            {
                Console.Error.WriteLine($"Failed to read HTML file: {ioEx.Message}");
                return;
            }

            // Define PST file path
            string pstFilePath = "output.pst";
            string pstDirectory = Path.GetDirectoryName(pstFilePath);
            if (!string.IsNullOrEmpty(pstDirectory) && !Directory.Exists(pstDirectory))
            {
                try
                {
                    Directory.CreateDirectory(pstDirectory);
                }
                catch (Exception dirEx)
                {
                    Console.Error.WriteLine($"Failed to create PST directory: {dirEx.Message}");
                    return;
                }
            }

            // Create a mail message with HTML body
            using (MailMessage mailMessage = new MailMessage())
            {
                mailMessage.From = "sender@example.com";
                mailMessage.To = "recipient@example.com";
                mailMessage.Subject = "HTML Message";
                mailMessage.HtmlBody = htmlContent;

                // Convert MailMessage to MapiMessage
                using (MapiMessage mapiMessage = MapiMessage.FromMailMessage(mailMessage))
                {
                    // Create a new PST file (Unicode format) and add the message
                    using (PersonalStorage pst = PersonalStorage.Create(pstFilePath, FileFormatVersion.Unicode))
                    {
                        pst.RootFolder.AddMessage(mapiMessage);
                    }
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine(ex.Message);
        }
    }
}
