using Aspose.Email.Mapi;
using System;
using System.IO;
using Aspose.Email;
using Aspose.Email.Storage.Pst;

class Program
{
    static void Main(string[] args)
    {
        try
        {
            string sourcePath = "TestEml.eml";
            string targetPath = "output.ost";

            // Ensure source EML exists; create a placeholder if it does not.
            if (!File.Exists(sourcePath))
            {
                try
                {
                    using (MailMessage placeholder = new MailMessage(
                        "sender@example.com",
                        "recipient@example.com",
                        "Placeholder Subject",
                        "Placeholder body."))
                    {
                        placeholder.Save(sourcePath, SaveOptions.DefaultEml);
                    }
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Error creating placeholder message: {ex.Message}");
                    return;
                }

                try
                {
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Error creating placeholder EML: {ex.Message}");
                    return;
                }

                Console.Error.WriteLine($"Source file not found. Placeholder created at '{sourcePath}'.");
                return;
            }

            // Ensure target directory exists.
            string targetDir = Path.GetDirectoryName(Path.GetFullPath(targetPath));
            if (!Directory.Exists(targetDir))
            {
                try
                {
                    Directory.CreateDirectory(targetDir);
                }
                catch (Exception dirEx)
                {
                    Console.Error.WriteLine($"Failed to create target directory: {dirEx.Message}");
                    return;
                }
            }

            // Load the EML message with appropriate options.
            var emlLoadOptions = new EmlLoadOptions
            {
                PreserveTnefAttachments = true,
                PreserveEmbeddedMessageFormat = true
            };

            using (MailMessage mailMessage = MailMessage.Load(sourcePath, emlLoadOptions))
            {
                // Convert MailMessage to MapiMessage.
                MapiMessage mapiMessage = MapiMessage.FromMailMessage(mailMessage);

                // Create OST file with Unicode format.
                using (PersonalStorage pst = PersonalStorage.Create(targetPath, FileFormatVersion.Unicode))
                {
                    // Add the message to the root folder.
                    pst.RootFolder.AddMessage(mapiMessage);
                }

                Console.WriteLine($"Conversion succeeded: '{sourcePath}' -> '{targetPath}'");
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
