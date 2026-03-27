using System;
using System.IO;
using Aspose.Email;
using Aspose.Email.Mapi;
using Aspose.Email.Storage.Pst;
using Aspose.Email.Storage;

class Program
{
    static void Main(string[] args)
    {
        try
        {
            string emlPath = "input.eml";
            string ostPath = "output.ost";

            // Verify input file exists
            if (!File.Exists(emlPath))
            {
                Console.Error.WriteLine($"Input file not found: {emlPath}");
                return;
            }

            // Ensure output directory exists
            string outputDir = Path.GetDirectoryName(ostPath);
            if (!string.IsNullOrEmpty(outputDir) && !Directory.Exists(outputDir))
            {
                try
                {
                    Directory.CreateDirectory(outputDir);
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to create output directory: {ex.Message}");
                    return;
                }
            }

            // Load the EML message
            using (MailMessage mailMessage = MailMessage.Load(emlPath, new EmlLoadOptions()))
            {
                // Convert to MAPI message
                MapiMessage mapiMessage = MapiMessage.FromMailMessage(mailMessage);

                // Create a new OST (PersonalStorage) file
                using (PersonalStorage personalStorage = PersonalStorage.Create(ostPath, FileFormatVersion.Unicode))
                {
                    // Get or create the Inbox folder
                    FolderInfo inboxFolder;
                    try
                    {
                        inboxFolder = personalStorage.RootFolder.GetSubFolder("Inbox");
                    }
                    catch
                    {
                        inboxFolder = null;
                    }

                    if (inboxFolder == null)
                    {
                        inboxFolder = personalStorage.RootFolder.AddSubFolder("Inbox");
                    }

                    // Add the message to the Inbox folder
                    inboxFolder.AddMessage(mapiMessage);
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}