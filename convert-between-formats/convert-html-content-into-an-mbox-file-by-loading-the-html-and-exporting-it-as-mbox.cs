using System;
using System.IO;
using Aspose.Email;
using Aspose.Email.Mapi;
using Aspose.Email.Storage;
using Aspose.Email.Storage.Pst;

class Program
{
    static void Main()
    {
        try
        {
            // Define file paths
            string htmlPath = "input.html";
            string pstPath = "temp.pst";
            string mboxPath = "output.mbox";

            // Ensure input HTML exists; create placeholder if missing
            if (!File.Exists(htmlPath))
            {
                try
                {
                    Directory.CreateDirectory(Path.GetDirectoryName(htmlPath) ?? ".");
                    File.WriteAllText(htmlPath, "<html><body><p>Placeholder HTML content.</p></body></html>");
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to create placeholder HTML file: {ex.Message}");
                    return;
                }
            }

            // Ensure output directory exists
            try
            {
                string mboxDir = Path.GetDirectoryName(mboxPath) ?? ".";
                Directory.CreateDirectory(mboxDir);
                string pstDir = Path.GetDirectoryName(pstPath) ?? ".";
                Directory.CreateDirectory(pstDir);
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Failed to create output directories: {ex.Message}");
                return;
            }

            // Load HTML content
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

            // Create MailMessage with HTML body
            using (MailMessage mailMessage = new MailMessage())
            {
                mailMessage.Subject = "Converted HTML Message";
                mailMessage.HtmlBody = htmlContent;

                // Create PST storage
                using (PersonalStorage pst = PersonalStorage.Create(pstPath, FileFormatVersion.Unicode))
                {
                    // Add Inbox folder
                    FolderInfo inboxFolder = pst.RootFolder.AddSubFolder("Inbox");

                    // Convert MailMessage to MapiMessage
                    MapiMessage mapiMessage = MapiMessage.FromMailMessage(mailMessage);

                    // Add the message to the Inbox folder
                    inboxFolder.AddMessage(mapiMessage);

                    // Convert PST to MBOX
                    try
                    {
                        MailboxConverter.ConvertPersonalStorageToMbox(pst, mboxPath, null);
                        Console.WriteLine($"MBOX file created at: {mboxPath}");
                    }
                    catch (Exception ex)
                    {
                        Console.Error.WriteLine($"Failed to convert PST to MBOX: {ex.Message}");
                        return;
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
