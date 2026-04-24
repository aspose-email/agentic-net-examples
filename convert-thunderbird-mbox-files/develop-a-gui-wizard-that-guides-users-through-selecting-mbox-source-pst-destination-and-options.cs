using System;
using System.IO;
using Aspose.Email;
using Aspose.Email.Storage;
using Aspose.Email.Storage.Pst;

class Program
{
    static void Main()
    {
        try
        {
            // Prompt for MBOX source path
            Console.Write("Enter the full path to the source MBOX file: ");
            string mboxPath = Console.ReadLine();

            // Prompt for PST destination path
            Console.Write("Enter the full path for the destination PST file: ");
            string pstPath = Console.ReadLine();

            // Prompt for option to remove signature
            Console.Write("Remove signatures during conversion? (y/n): ");
            string removeSignatureInput = Console.ReadLine();
            bool removeSignature = removeSignatureInput != null && removeSignatureInput.Trim().ToLower() == "y";

            // Guard source file existence
            if (string.IsNullOrEmpty(mboxPath))
            {
                Console.Error.WriteLine("Source MBOX path is empty.");
                return;
            }

            if (!File.Exists(mboxPath))
            {
                try
                {
                    // Create an empty placeholder MBOX file
                    using (FileStream placeholder = File.Create(mboxPath))
                    {
                        // No content needed
                    }
                    Console.WriteLine($"Placeholder MBOX file created at: {mboxPath}");
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to create placeholder MBOX file: {ex.Message}");
                    return;
                }
            }

            // Guard destination directory existence
            if (string.IsNullOrEmpty(pstPath))
            {
                Console.Error.WriteLine("Destination PST path is empty.");
                return;
            }

            string pstDirectory = Path.GetDirectoryName(pstPath);
            if (!string.IsNullOrEmpty(pstDirectory) && !Directory.Exists(pstDirectory))
            {
                try
                {
                    Directory.CreateDirectory(pstDirectory);
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to create destination directory: {ex.Message}");
                    return;
                }
            }

            // Set conversion options
            MboxToPstConversionOptions options = new MboxToPstConversionOptions();
            options.RemoveSignature = removeSignature;
            options.MessageHandler = delegate (MailMessage message)
            {
                Console.WriteLine($"Processing message: {message.Subject}");
            };

            // Perform conversion
            try
            {
                MailStorageConverter.MboxToPst(mboxPath, pstPath, options);
                Console.WriteLine("Conversion completed successfully.");
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Conversion failed: {ex.Message}");
                return;
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
