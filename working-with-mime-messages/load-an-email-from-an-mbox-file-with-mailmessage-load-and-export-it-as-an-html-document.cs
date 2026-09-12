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
            const string outputDir = "output";

            // Verify the MBOX file exists before proceeding.
            if (!File.Exists(mboxPath))
            {
                Console.Error.WriteLine($"Input file not found: {mboxPath}");
                return;
            }

            // Ensure the output directory exists.
            if (!Directory.Exists(outputDir))
                Directory.CreateDirectory(outputDir);

            // Create a reader for the MBOX storage.
            using (MboxStorageReader mboxReader = MboxStorageReader.CreateReader(mboxPath, new MboxLoadOptions()))
            {
                while (true)
                {
                    // Read the next message from the MBOX file.
                    MailMessage mailMessage = mboxReader.ReadNextMessage();
                    if (mailMessage == null)
                        break;

                    // Prepare a safe file name for the HTML output.
                    string safeSubject = string.IsNullOrWhiteSpace(mailMessage.Subject) ? "Untitled" : mailMessage.Subject;
                    foreach (char c in Path.GetInvalidFileNameChars())
                    {
                        safeSubject = safeSubject.Replace(c, '_');
                    }

                    string htmlPath = Path.Combine(outputDir, $"{safeSubject}.html");

                    // Save the email as an HTML document.
                    mailMessage.Save(htmlPath, SaveOptions.DefaultHtml);
                    Console.WriteLine($"Saved HTML: {htmlPath}");
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
