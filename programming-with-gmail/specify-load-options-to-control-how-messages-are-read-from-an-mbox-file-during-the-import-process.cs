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
            // Path to the MBOX file
            string mboxPath = "storage.mbox";

            // Verify the MBOX file exists; create a minimal placeholder if missing
            if (!File.Exists(mboxPath))
            {
                File.WriteAllText(mboxPath, string.Empty);
                Console.Error.WriteLine($"MBOX file not found. Created empty placeholder at '{mboxPath}'.");
                return;
            }

            // Create the MBOX reader with specific load options
            var mboxLoadOptions = new MboxLoadOptions
            {
                PreferredTextEncoding = Encoding.UTF8,
                LeaveOpen = false
            };

            using (MboxStorageReader mbox = MboxStorageReader.CreateReader(mboxPath, mboxLoadOptions))
            {
                // Ensure the output directory exists
                string outputDir = "output";
                Directory.CreateDirectory(outputDir);

                // Read messages sequentially
                while (true)
                {
                    MailMessage eml = mbox.ReadNextMessage();
                    if (eml == null)
                        break;

                    Console.WriteLine($"Subject: {eml.Subject}");
                    Console.WriteLine($"From: {eml.From}");
                    Console.WriteLine($"To: {eml.To}");

                    // Build a safe file name for the extracted message
                    string safeSubject = GetSafeFileName(eml.Subject);
                    string outputPath = Path.Combine(outputDir, $"{safeSubject}.eml");

                    try
                    {
                        // Save the extracted message
                        eml.Save(outputPath);
                        Console.WriteLine($"Saved to '{outputPath}'.");
                    }
                    catch (Exception saveEx)
                    {
                        Console.Error.WriteLine($"Failed to save message '{eml.Subject}': {saveEx.Message}");
                    }
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }

    // Helper to replace invalid filename characters with an underscore
    private static string GetSafeFileName(string name)
    {
        if (string.IsNullOrEmpty(name))
            return "Untitled";

        char[] invalidChars = Path.GetInvalidFileNameChars();
        var sb = new StringBuilder(name);
        foreach (char c in invalidChars)
        {
            sb.Replace(c, '_');
        }
        return sb.ToString();
    }
}
