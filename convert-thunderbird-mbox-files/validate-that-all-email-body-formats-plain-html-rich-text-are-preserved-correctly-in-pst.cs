using System;
using System.IO;
using Aspose.Email;
using Aspose.Email.Mapi;
using Aspose.Email.Storage.Pst;

class Program
{
    static void Main()
    {
        try
        {
            // Define file paths
            const string plainEmlPath = "plain.eml";
            const string htmlEmlPath = "html.eml";
            const string rtfEmlPath = "rtf.eml";
            const string pstPath = "output.pst";

            // Ensure source EML files exist; create minimal placeholders if missing
            EnsurePlainEmlExists(plainEmlPath);
            EnsureHtmlEmlExists(htmlEmlPath);
            EnsureRtfEmlExists(rtfEmlPath);

            // Load the messages
            MailMessage plainMessage = MailMessage.Load(plainEmlPath);
            MailMessage htmlMessage = MailMessage.Load(htmlEmlPath);
            MailMessage rtfMessage = MailMessage.Load(rtfEmlPath);

            // Ensure output directory exists
            string pstDir = Path.GetDirectoryName(pstPath);
            if (!string.IsNullOrEmpty(pstDir))
                Directory.CreateDirectory(pstDir);

            // Create or overwrite the PST file
            if (File.Exists(pstPath))
                File.Delete(pstPath);

            using (PersonalStorage pst = PersonalStorage.Create(pstPath, FileFormatVersion.Unicode))
            {
                // Create a folder to store the messages
                FolderInfo inbox = pst.RootFolder.AddSubFolder("Inbox");

                // Add messages to the PST folder
                inbox.AddMessage(MapiMessage.FromMailMessage(plainMessage));
                inbox.AddMessage(MapiMessage.FromMailMessage(htmlMessage));
                inbox.AddMessage(MapiMessage.FromMailMessage(rtfMessage));
            }

            // Re-open the PST for reading and verify body formats
            using (PersonalStorage pst = PersonalStorage.FromFile(pstPath))
            {
                FolderInfo inbox = pst.RootFolder.GetSubFolder("Inbox");
                foreach (MessageInfo msgInfo in inbox.EnumerateMessages())
                {
                    // Extract the full message as MapiMessage, then convert to MailMessage
                    MapiMessage mapiMsg = pst.ExtractMessage(msgInfo);
                    MailMessage extracted = mapiMsg.ToMailMessage(new MailConversionOptions());

                    // Simple verification of body content
                    Console.WriteLine($"Subject: {extracted.Subject}");
                    Console.WriteLine($"Plain Body: {(string.IsNullOrEmpty(extracted.Body) ? "<empty>" : extracted.Body)}");
                    Console.WriteLine($"HTML Body: {(string.IsNullOrEmpty(extracted.HtmlBody) ? "<empty>" : extracted.HtmlBody)}");
                    Console.WriteLine(new string('-', 40));
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }

    // Helper to ensure a plain text EML file exists
    private static void EnsurePlainEmlExists(string path)
    {
        if (!File.Exists(path))
        {
            MailMessage msg = new MailMessage
            {
                Subject = "Plain Text Message",
                Body = "This is a plain text body.",
                From = new MailAddress("sender@example.com")
            };
            msg.To.Add(new MailAddress("recipient@example.com"));
            msg.Save(path, SaveOptions.DefaultEml);
        }
    }

    // Helper to ensure an HTML EML file exists
    private static void EnsureHtmlEmlExists(string path)
    {
        if (!File.Exists(path))
        {
            MailMessage msg = new MailMessage
            {
                Subject = "HTML Message",
                HtmlBody = "<html><body><h1>HTML Body</h1></body></html>",
                From = new MailAddress("sender@example.com")
            };
            msg.To.Add(new MailAddress("recipient@example.com"));
            msg.Save(path, SaveOptions.DefaultEml);
        }
    }

    // Helper to ensure an RTF-like EML file exists (using plain body as placeholder)
    private static void EnsureRtfEmlExists(string path)
    {
        if (!File.Exists(path))
        {
            MailMessage msg = new MailMessage
            {
                Subject = "Rich Text Message",
                Body = @"{\rtf1\ansi This is {\b rich} text.}",
                From = new MailAddress("sender@example.com")
            };
            msg.To.Add(new MailAddress("recipient@example.com"));
            msg.Save(path, SaveOptions.DefaultEml);
        }
    }
}
