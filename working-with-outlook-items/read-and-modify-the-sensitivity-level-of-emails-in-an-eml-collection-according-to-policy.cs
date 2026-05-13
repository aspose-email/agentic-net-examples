using System;
using System.IO;
using Aspose.Email;

class Program
{
    static void Main(string[] args)
    {
        try
        {
            // Define the folder that contains the EML files.
            string emlFolderPath = "Emails";

            // Verify the folder exists.
            if (!Directory.Exists(emlFolderPath))
            {
                Console.Error.WriteLine($"Folder not found: {emlFolderPath}");
                return;
            }

            // Get all .eml files in the folder.
            string[] emlFilePaths;
            try
            {
                emlFilePaths = Directory.GetFiles(emlFolderPath, "*.eml");
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Failed to enumerate files: {ex.Message}");
                return;
            }

            foreach (string emlFilePath in emlFilePaths)
            {
                // Ensure the individual file exists before processing.
                if (!File.Exists(emlFilePath))
                {
                try
                {
                    using (MailMessage placeholder = new MailMessage(
                        "sender@example.com",
                        "recipient@example.com",
                        "Placeholder Subject",
                        "Placeholder body."))
                    {
                        placeholder.Save(emlFilePath, SaveOptions.DefaultEml);
                    }
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Error creating placeholder message: {ex.Message}");
                    return;
                }

                    Console.Error.WriteLine($"File not found, skipping: {emlFilePath}");
                    continue;
                }

                try
                {
                    // Load the email message.
                    using (MailMessage message = MailMessage.Load(emlFilePath))
                    {
                        // Apply policy: set sensitivity to Private.
                        message.Sensitivity = MailSensitivity.Private;

                        // Save the modified message back to the same file.
                        // Overwrite the original file.
                        message.Save(emlFilePath);
                    }
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Error processing '{emlFilePath}': {ex.Message}");
                    // Continue with next file.
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
