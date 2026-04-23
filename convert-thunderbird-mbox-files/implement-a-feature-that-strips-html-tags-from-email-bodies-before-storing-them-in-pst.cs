using System;
using System.IO;
using System.Text.RegularExpressions;
using Aspose.Email;
using Aspose.Email.Storage;
using Aspose.Email.Storage.Pst;

class Program
{
    static void Main(string[] args)
    {
        try
        {
            string mboxPath = "input.mbox";
            string pstPath = "output.pst";

            // Verify input MBOX file exists
            if (!File.Exists(mboxPath))
            {
                Console.Error.WriteLine($"MBOX file not found: {mboxPath}");
                return;
            }

            // Ensure output directory exists
            string pstDirectory = Path.GetDirectoryName(pstPath);
            if (!string.IsNullOrEmpty(pstDirectory) && !Directory.Exists(pstDirectory))
            {
                Directory.CreateDirectory(pstDirectory);
            }

            // Define a handler that strips HTML tags from each message before it is added to PST
            MailStorageConverter.MailHandler htmlStripHandler = (MailMessage message) =>
            {
                if (!string.IsNullOrEmpty(message.HtmlBody))
                {
                    string plainText = StripHtml(message.HtmlBody);
                    message.Body = plainText;
                    message.IsBodyHtml = false;
                }
            };

            // Convert MBOX to PST using the handler
            using (PersonalStorage pst = MailStorageConverter.MboxToPst(mboxPath, pstPath, htmlStripHandler))
            {
                // PST is created and disposed automatically
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }

    // Simple HTML tag remover using regular expressions
    private static string StripHtml(string html)
    {
        if (string.IsNullOrEmpty(html))
            return string.Empty;

        // Remove script and style blocks
        string withoutScripts = Regex.Replace(html, @"<(script|style)[^>]*?>.*?</\1>", string.Empty, RegexOptions.Singleline | RegexOptions.IgnoreCase);
        // Remove all HTML tags
        string withoutTags = Regex.Replace(withoutScripts, @"<[^>]+>", string.Empty);
        // Decode HTML entities
        string decoded = System.Net.WebUtility.HtmlDecode(withoutTags);
        // Trim whitespace
        return decoded.Trim();
    }
}
