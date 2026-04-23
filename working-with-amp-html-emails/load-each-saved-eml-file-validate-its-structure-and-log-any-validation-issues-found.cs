using System;
using System.IO;
using Aspose.Email;
using Aspose.Email.Tools.Verifications;

class Program
{
    static void Main()
    {
        try
        {
            // Directory that contains the .eml files
            string emailsDirectory = "Emails";

            // Ensure the directory exists; create if missing
            if (!Directory.Exists(emailsDirectory))
            {
                try
                {
                    Directory.CreateDirectory(emailsDirectory);
                }
                catch (Exception dirEx)
                {
                    Console.Error.WriteLine($"Failed to create directory '{emailsDirectory}': {dirEx.Message}");
                    return;
                }
            }

            // Get all .eml files in the directory
            string[] emlFiles;
            try
            {
                emlFiles = Directory.GetFiles(emailsDirectory, "*.eml");
            }
            catch (Exception getFilesEx)
            {
                Console.Error.WriteLine($"Failed to enumerate .eml files: {getFilesEx.Message}");
                return;
            }

            // If no .eml files are present, create a minimal placeholder file
            if (emlFiles.Length == 0)
            {
                string placeholderPath = Path.Combine(emailsDirectory, "placeholder.eml");
                string minimalEmlContent = "Subject: Placeholder\r\n\r\nThis is a minimal placeholder email.";
                try
                {
                    File.WriteAllText(placeholderPath, minimalEmlContent);
                    emlFiles = new string[] { placeholderPath };
                }
                catch (Exception writeEx)
                {
                    Console.Error.WriteLine($"Failed to create placeholder .eml file: {writeEx.Message}");
                    return;
                }
            }

            // Validate each .eml file and log any issues
            foreach (string emlFilePath in emlFiles)
            {
                try
                {
                    MessageValidationResult validationResult = MessageValidator.Validate(emlFilePath);
                    Console.WriteLine($"File: {Path.GetFileName(emlFilePath)}");
                    Console.WriteLine($"Validation Result: {validationResult}");
                }
                catch (Exception validateEx)
                {
                    Console.Error.WriteLine($"Error validating '{emlFilePath}': {validateEx.Message}");
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
