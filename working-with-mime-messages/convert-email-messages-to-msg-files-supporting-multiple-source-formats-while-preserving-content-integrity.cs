using System;
using System.IO;
using Aspose.Email;
using Aspose.Email.Mapi;

class Program
{
    static void Main()
    {
        try
        {
            // Define source and destination paths
            string sourcePath = "input.eml";   // can be .eml, .msg, etc.
            string destinationPath = "output.msg";

            // Verify source file exists
            if (!File.Exists(sourcePath))
            {
                Console.Error.WriteLine($"Error: Source file not found – {sourcePath}");
                return;
            }

            // Ensure destination directory exists
            string destDir = Path.GetDirectoryName(destinationPath);
            if (!string.IsNullOrEmpty(destDir) && !Directory.Exists(destDir))
            {
                try
                {
                    Directory.CreateDirectory(destDir);
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Error: Unable to create directory – {destDir}. {ex.Message}");
                    return;
                }
            }

            // Determine source file type by extension
            string ext = Path.GetExtension(sourcePath).ToLowerInvariant();

            // Convert to MapiMessage and save as MSG
            if (ext == ".eml")
            {
                // Load EML as MailMessage
                using (MailMessage mail = MailMessage.Load(sourcePath))
                {
                    // Convert to MapiMessage
                    MapiMessage mapi = MapiMessage.FromMailMessage(mail);
                    // Save as MSG (Unicode format is default)
                    mapi.Save(destinationPath);
                }
            }
            else if (ext == ".msg")
            {
                // Load existing MSG directly
                using (MapiMessage mapi = MapiMessage.Load(sourcePath))
                {
                    // Save (could be a copy or re‑save to ensure format)
                    mapi.Save(destinationPath);
                }
            }
            else
            {
                Console.Error.WriteLine($"Error: Unsupported source format – {ext}");
                return;
            }

            Console.WriteLine($"Message successfully saved to {destinationPath}");
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
