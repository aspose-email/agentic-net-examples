using System;
using System.IO;
using System.Text;
using Aspose.Email;
using Aspose.Email.Storage.Mbox;

class Program
{
    static void Main(string[] args)
    {
        try
        {
            // Path to the multilingual MBOX archive
            string mboxPath = "multilingual.mbox";

            // Verify that the MBOX file exists before proceeding
            if (!File.Exists(mboxPath))
            {
                Console.Error.WriteLine($"MBOX file not found: {mboxPath}");
                return;
            }

            // Output directory for extracted .eml files
            string outputDir = "ExtractedMessages";
            Directory.CreateDirectory(outputDir);

            // Create the MBOX reader with UTF‑8 encoding
            var loadOptions = new MboxLoadOptions { PreferredTextEncoding = Encoding.UTF8 };
            using (MboxStorageReader reader = MboxStorageReader.CreateReader(mboxPath, loadOptions))
            {
                MailMessage message;
                int index = 0;
                while ((message = reader.ReadNextMessage()) != null)
                {
                    // Prepare a safe file name for the extracted .eml file
                    string subject = string.IsNullOrEmpty(message.Subject) ? $"NoSubject_{index}" : message.Subject;
                    foreach (char invalidChar in Path.GetInvalidFileNameChars())
                    {
                        subject = subject.Replace(invalidChar, '_');
                    }
                    string emlFilePath = Path.Combine(outputDir, $"{subject}_{index}.eml");

                    // Save the extracted message, handling any I/O errors gracefully
                    try
                    {
                        message.Save(emlFilePath);
                        Console.WriteLine($"Saved: {emlFilePath}");
                    }
                    catch (Exception saveEx)
                    {
                        Console.Error.WriteLine($"Failed to save message {index}: {saveEx.Message}");
                    }

                    index++;
                }
            }
        }
        catch (Exception ex)
        {
            // Top‑level exception guard
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
