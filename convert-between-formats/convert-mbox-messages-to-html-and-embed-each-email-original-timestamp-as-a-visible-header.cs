using System;
using System.IO;
using Aspose.Email;
using Aspose.Email.Storage.Mbox;

class Program
{
    static void Main()
    {
        try
        {
            string mboxPath = "input.mbox";
            string outputDirectory = "output_html";

            if (!File.Exists(mboxPath))
            {
                Console.Error.WriteLine($"MBOX file not found: {mboxPath}");
                return;
            }

            try
            {
                Directory.CreateDirectory(outputDirectory);
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Failed to create output directory: {ex.Message}");
                return;
            }

            using (FileStream mboxStream = File.OpenRead(mboxPath))
            {
                MboxStorageReader reader = MboxStorageReader.CreateReader(mboxStream, new MboxLoadOptions());
                using (reader)
                {
                    int messageIndex = 0;
                    MailMessage mailMessage;
                    while ((mailMessage = reader.ReadNextMessage()) != null)
                    {
                        using (mailMessage)
                        {
                            string timestampHeader = $"<h2>{mailMessage.Date}</h2>";
                            string bodyContent = string.IsNullOrEmpty(mailMessage.HtmlBody)
                                ? System.Net.WebUtility.HtmlEncode(mailMessage.Body)
                                : mailMessage.HtmlBody;
                            string combinedHtml = $"{timestampHeader}<hr/>{bodyContent}";

                            string outputPath = Path.Combine(outputDirectory, $"message_{messageIndex}.html");
                            try
                            {
                                File.WriteAllText(outputPath, combinedHtml);
                            }
                            catch (Exception ex)
                            {
                                Console.Error.WriteLine($"Failed to write HTML for message {messageIndex}: {ex.Message}");
                            }

                            messageIndex++;
                        }
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
