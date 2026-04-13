using System;
using System.IO;
using Aspose.Email;
using Aspose.Email.Mapi;

class Program
{
    static void Main(string[] args)
    {
        try
        {
            // Path to the original MSG file
            string sourcePath = "input.msg";

            // Destination directory for the archived file
            string destinationDirectory = "secure_archive";

            // Full path for the archived file
            string destinationPath = Path.Combine(destinationDirectory, Path.GetFileName(sourcePath));

            // Ensure the source MSG file exists; create a minimal placeholder if it does not
            if (!File.Exists(sourcePath))
            {
                try
                {
                    using (MapiMessage placeholderMessage = new MapiMessage())
                    {
                        placeholderMessage.Save(sourcePath);
                    }
                    Console.WriteLine($"Placeholder MSG created at: {sourcePath}");
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to create placeholder MSG: {ex.Message}");
                    return;
                }
            }

            // Ensure the destination directory exists
            if (!Directory.Exists(destinationDirectory))
            {
                try
                {
                    Directory.CreateDirectory(destinationDirectory);
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to create destination directory: {ex.Message}");
                    return;
                }
            }

            // Remove all reactions (treated as attachments) from the MSG file
            try
            {
                MapiMessage.DestroyAttachments(sourcePath);
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Error while destroying attachments: {ex.Message}");
                return;
            }

            // Archive the cleaned MSG file to the secure location
            try
            {
                File.Copy(sourcePath, destinationPath, true);
                Console.WriteLine($"Cleaned MSG archived to: {destinationPath}");
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Error while archiving file: {ex.Message}");
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
