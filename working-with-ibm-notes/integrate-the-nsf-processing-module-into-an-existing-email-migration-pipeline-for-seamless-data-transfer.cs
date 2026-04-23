using System;
using System.IO;
using Aspose.Email;
using Aspose.Email.Storage.Nsf;

class Program
{
    static void Main()
    {
        try
        {
            string nsfPath = "source.nsf";
            string outputDirectory = "ExportedMessages";

            // Verify NSF file exists
            if (!File.Exists(nsfPath))
            {
                Console.Error.WriteLine($"NSF file not found: {nsfPath}");
                return;
            }

            // Ensure output directory exists
            try
            {
                if (!Directory.Exists(outputDirectory))
                {
                    Directory.CreateDirectory(outputDirectory);
                }
            }
            catch (Exception dirEx)
            {
                Console.Error.WriteLine($"Failed to create output directory: {dirEx.Message}");
                return;
            }

            // Open NSF storage
            using (NotesStorageFacility nsf = new NotesStorageFacility(nsfPath))
            {
                foreach (MailMessage message in nsf.EnumerateMessages())
                {
                    using (MailMessage mail = message)
                    {
                        // Prepare a safe filename based on subject
                        string subject = string.IsNullOrWhiteSpace(mail.Subject) ? "NoSubject" : mail.Subject;
                        foreach (char invalidChar in Path.GetInvalidFileNameChars())
                        {
                            subject = subject.Replace(invalidChar, '_');
                        }

                        string emlFilePath = Path.Combine(outputDirectory,
                            $"{subject}_{Guid.NewGuid()}.eml");

                        try
                        {
                            mail.Save(emlFilePath);
                            Console.WriteLine($"Saved: {emlFilePath}");
                        }
                        catch (Exception saveEx)
                        {
                            Console.Error.WriteLine($"Failed to save message '{subject}': {saveEx.Message}");
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
