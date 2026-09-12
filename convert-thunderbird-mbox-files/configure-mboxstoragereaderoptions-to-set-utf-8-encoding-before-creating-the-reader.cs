using System;
using System.IO;
using System.Text;
using Aspose.Email;
using Aspose.Email.Storage.Mbox;

class Program
{
    static void Main()
    {
        try
        {
            string mboxPath = "storage.mbox";

            if (!File.Exists(mboxPath))
            {
                Console.Error.WriteLine($"MBOX file not found: {mboxPath}");
                return;
            }

            // Configure load options with UTF-8 encoding
            var loadOptions = new MboxLoadOptions
            {
                PreferredTextEncoding = Encoding.UTF8
            };

            // Ensure output directory exists
            string outputDir = "output";
            Directory.CreateDirectory(outputDir);

            using (MboxStorageReader mbox = MboxStorageReader.CreateReader(mboxPath, loadOptions))
            {
                int index = 0;
                while (true)
                {
                    MailMessage message = mbox.ReadNextMessage();
                    if (message == null)
                        break;

                    Console.WriteLine($"Message {++index}:");
                    Console.WriteLine($"Subject: {message.Subject}");
                    Console.WriteLine($"From: {message.From}");
                    Console.WriteLine($"To: {message.To}");

                    string safeFileName = string.IsNullOrWhiteSpace(message.Subject)
                        ? $"Untitled_{index}"
                        : message.Subject;

                    foreach (char c in Path.GetInvalidFileNameChars())
                    {
                        safeFileName = safeFileName.Replace(c, '_');
                    }

                    // Truncate filename if too long for the file system
                    int maxFileNameLength = 200;
                    if (safeFileName.Length > maxFileNameLength)
                        safeFileName = safeFileName.Substring(0, maxFileNameLength);

                    string emlPath = Path.Combine(outputDir, $"{safeFileName}.eml");

                    try
                    {
                        message.Save(emlPath);
                        Console.WriteLine($"Saved to {emlPath}");
                    }
                    catch (Exception ex)
                    {
                        Console.Error.WriteLine($"Failed to save message to {emlPath}: {ex.Message}");
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
