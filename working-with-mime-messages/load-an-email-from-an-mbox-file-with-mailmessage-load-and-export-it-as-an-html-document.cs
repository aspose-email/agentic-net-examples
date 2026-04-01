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
            string mboxPath = "sample.mbox";
            string htmlPath = "output.html";

            // Ensure the output directory exists
            string htmlDir = Path.GetDirectoryName(htmlPath);
            if (!string.IsNullOrEmpty(htmlDir) && !Directory.Exists(htmlDir))
            {
                Directory.CreateDirectory(htmlDir);
            }

            // Guard MBOX file existence; create a minimal placeholder if missing
            if (!File.Exists(mboxPath))
            {
                try
                {
                    string emlContent = "Subject: Test Email\r\nFrom: sender@example.com\r\nTo: receiver@example.com\r\n\r\nThis is a test email.";
                    string mboxContent = "From - Mon Jan 01 00:00:00 2020\r\n" + emlContent.Replace("\r\n", "\n").Replace("\n", "\r\n");
                    File.WriteAllText(mboxPath, mboxContent);
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to create placeholder MBOX file: {ex.Message}");
                    return;
                }
            }

            // Read the first message from the MBOX storage
            using (MboxStorageReader mboxReader = MboxStorageReader.CreateReader(mboxPath, new MboxLoadOptions()))
            {
                MailMessage extractedMessage;
                try
                {
                    extractedMessage = mboxReader.ReadNextMessage();
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to read message from MBOX: {ex.Message}");
                    return;
                }

                if (extractedMessage == null)
                {
                    Console.Error.WriteLine("No messages found in the MBOX file.");
                    return;
                }

                // Save the extracted message to a temporary EML file
                string tempEmlPath = Path.Combine(Path.GetTempPath(), "temp.eml");
                try
                {
                    extractedMessage.Save(tempEmlPath);
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to save temporary EML file: {ex.Message}");
                    return;
                }

                // Load the message using MailMessage.Load
                MailMessage loadedMessage;
                try
                {
                    loadedMessage = MailMessage.Load(tempEmlPath);
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to load message from temporary EML: {ex.Message}");
                    return;
                }

                // Export the loaded message as HTML
                try
                {
                    HtmlSaveOptions htmlOptions = new HtmlSaveOptions();
                    loadedMessage.Save(htmlPath, htmlOptions);
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to save HTML file: {ex.Message}");
                    return;
                }
                finally
                {
                    // Clean up the temporary EML file
                    try
                    {
                        if (File.Exists(tempEmlPath))
                        {
                            File.Delete(tempEmlPath);
                        }
                    }
                    catch { /* ignore cleanup errors */ }
                }

                Console.WriteLine($"Message successfully exported to HTML at: {htmlPath}");
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
