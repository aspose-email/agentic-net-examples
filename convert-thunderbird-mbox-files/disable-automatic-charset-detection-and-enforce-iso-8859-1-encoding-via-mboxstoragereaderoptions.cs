using System;
using System.IO;
using System.Text;
using Aspose.Email;
using Aspose.Email.Storage.Mbox;

class Program
{
    static void Main()
    {
        // Author note: Simple console app that reads a Thunderbird MBOX file,
        // disables charset auto‑detection by specifying ISO‑8859‑1 encoding,
        // and saves each message as an individual .eml file.

        string mboxPath = "input.mbox";
        string outputDir = "output";

        // Guard input file existence
        if (!File.Exists(mboxPath))
        {
            Console.Error.WriteLine($"Input MBOX file not found: {mboxPath}");
            return;
        }

        // Ensure the output directory exists
        if (!Directory.Exists(outputDir))
        {
            Directory.CreateDirectory(outputDir);
        }

        try
        {
            // Configure load options: enforce ISO‑8859‑1 encoding
            MboxLoadOptions loadOptions = new MboxLoadOptions
            {
                PreferredTextEncoding = Encoding.GetEncoding(28591) // ISO‑8859‑1
            };

            // Create the reader with the specified options
            using (MboxStorageReader reader = MboxStorageReader.CreateReader(mboxPath, loadOptions))
            {
                int messageIndex = 0;
                while (true)
                {
                    // Sequentially read each message; returns null when no more messages
                    MailMessage message = reader.ReadNextMessage();
                    if (message == null)
                        break;

                    // Build a safe filename from the subject
                    string subject = string.IsNullOrEmpty(message.Subject) ? $"Message_{messageIndex}" : message.Subject;
                    foreach (char invalid in Path.GetInvalidFileNameChars())
                        subject = subject.Replace(invalid, '_');

                    string emlPath = Path.Combine(outputDir, $"{subject}.eml");
                    message.Save(emlPath);
                    messageIndex++;
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error processing MBOX file: {ex.Message}");
        }
    }
}
