using Aspose.Email.Mapi;
using System;
using System.IO;
using Aspose.Email;
using Aspose.Email.Storage.Pst;

class Program
{
    static void Main()
    {
        try
        {
            // Input HTML file path
            const string htmlPath = "content.html";

            // Ensure the HTML file exists; create a minimal placeholder if missing
            if (!File.Exists(htmlPath))
            {
                try
                {
                    File.WriteAllText(htmlPath, "<html><body><p>Placeholder content</p></body></html>");
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

            // Create a mail message with the HTML body
            MailMessage mailMessage = new MailMessage();
            mailMessage.From = new MailAddress("sender@example.com");
            mailMessage.To.Add(new MailAddress("recipient@example.com"));
            mailMessage.Subject = "HTML Content Message";
            mailMessage.IsBodyHtml = true;
            mailMessage.HtmlBody = htmlContent;

            // PST file path
            const string pstPath = "output.pst";

            // Ensure the directory for the PST exists
            string pstDirectory = Path.GetDirectoryName(pstPath);
            if (!string.IsNullOrEmpty(pstDirectory) && !Directory.Exists(pstDirectory))
            {
                try
                {
                    Directory.CreateDirectory(pstDirectory);
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to create PST directory: {ex.Message}");
                    return;
                }
            }

            // Load existing PST or create a new one
            PersonalStorage pst;
            if (File.Exists(pstPath))
            {
                try
                {
                    pst = PersonalStorage.FromFile(pstPath);
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to load PST file: {ex.Message}");
                    return;
                }
            }
            else
            {
                try
                {
                    pst = PersonalStorage.Create(pstPath, FileFormatVersion.Unicode);
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to create PST file: {ex.Message}");
                    return;
                }
            }

            // Add the message to the root folder of the PST
            try
            {
                FolderInfo rootFolder = pst.RootFolder;
                rootFolder.AddMessage(MapiMessage.FromMailMessage(mailMessage));
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Failed to add message to PST: {ex.Message}");
                // Dispose PST before exiting
                pst.Dispose();
                return;
            }

            // Dispose PST (saves changes)
            pst.Dispose();

            Console.WriteLine("Message added to PST successfully.");
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
