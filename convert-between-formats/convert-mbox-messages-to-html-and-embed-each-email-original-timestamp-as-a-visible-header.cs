using System;
using System.IO;
using Aspose.Email;
using Aspose.Email.Storage.Mbox;
using Aspose.Email.Tools.Search;

class Program
{
    static void Main()
    {
        try
        {
            const string mboxPath = "storage.mbox";
            const string outputDir = "output_html";

            if (!File.Exists(mboxPath))
            {
                Console.Error.WriteLine($"Input MBOX file not found: {mboxPath}");
                return;
            }

            try
            {
                if (!Directory.Exists(outputDir))
                    Directory.CreateDirectory(outputDir);
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Failed to create output directory: {ex.Message}");
                return;
            }

            using (MboxStorageReader mboxReader = MboxStorageReader.CreateReader(mboxPath, new MboxLoadOptions()))
            {
                int messageIndex = 0;
                MailMessage mailMessage;
                while ((mailMessage = mboxReader.ReadNextMessage()) != null)
                {
                    try
                    {
                        // Build a visible timestamp header (using the original Date header)
                        string timestampHeader = $"<h2>{mailMessage.Date.ToString("F")}</h2>";

                        // Prepend the timestamp to the HTML body
                        if (!string.IsNullOrEmpty(mailMessage.HtmlBody))
                        {
                            mailMessage.HtmlBody = timestampHeader + mailMessage.HtmlBody;
                        }
                        else
                        {
                            string plainBody = string.IsNullOrEmpty(mailMessage.Body) ? string.Empty : mailMessage.Body;
                            mailMessage.HtmlBody = timestampHeader + $"<pre>{plainBody}</pre>";
                        }

                        // Create a safe file name based on the subject and index
                        string safeSubject = string.IsNullOrEmpty(mailMessage.Subject) ? "NoSubject" : mailMessage.Subject;
                        foreach (char c in Path.GetInvalidFileNameChars())
                            safeSubject = safeSubject.Replace(c, '_');

                        string outputPath = Path.Combine(outputDir, $"{safeSubject}_{messageIndex}.html");

                        // Save as HTML
                        mailMessage.Save(outputPath, SaveOptions.DefaultHtml);
                    }
                    catch (Exception ex)
                    {
                        Console.Error.WriteLine($"Failed to process message #{messageIndex}: {ex.Message}");
                    }

                    messageIndex++;
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
