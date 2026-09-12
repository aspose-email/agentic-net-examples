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
            // Convert MBOX messages to EMLX files organized by sender domain.
            string mboxFilePath = "storage.mbox";
            string outputRootPath = "output";

            if (!File.Exists(mboxFilePath))
            {
                Console.Error.WriteLine($"Input MBOX file not found: {mboxFilePath}");
                return;
            }

            Directory.CreateDirectory(outputRootPath);

            using (MboxStorageReader mboxReader = MboxStorageReader.CreateReader(mboxFilePath, new MboxLoadOptions()))
            {
                MailMessage mailMessage;
                int messageCounter = 0;

                while ((mailMessage = mboxReader.ReadNextMessage()) != null)
                {
                    // Determine sender domain
                    string senderAddress = mailMessage.From?.Address ?? "unknown";
                    string senderDomain = "unknown";
                    int atPos = senderAddress.IndexOf('@');
                    if (atPos >= 0 && atPos < senderAddress.Length - 1)
                    {
                        senderDomain = senderAddress.Substring(atPos + 1);
                    }

                    // Create domain subfolder
                    string domainFolderPath = Path.Combine(outputRootPath, senderDomain);
                    Directory.CreateDirectory(domainFolderPath);

                    // Prepare a safe file name from the subject
                    string subject = string.IsNullOrEmpty(mailMessage.Subject) ? "NoSubject" : mailMessage.Subject;
                    foreach (char invalidChar in Path.GetInvalidFileNameChars())
                    {
                        subject = subject.Replace(invalidChar, '_');
                    }

                    // Ensure unique file name
                    string fileName = $"{subject}_{messageCounter++}.emlx";
                    string outputFilePath = Path.Combine(domainFolderPath, fileName);

                    // Save the message as EMLX
                    mailMessage.Save(outputFilePath);
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
