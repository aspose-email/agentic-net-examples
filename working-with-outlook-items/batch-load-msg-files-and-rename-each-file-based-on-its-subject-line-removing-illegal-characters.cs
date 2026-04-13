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
            string inputFolder = "InputMsgs";

            // Ensure the input directory exists
            try
            {
                Directory.CreateDirectory(inputFolder);
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Failed to create or access directory '{inputFolder}': {ex.Message}");
                return;
            }

            // Get all MSG files in the folder
            string[] msgFiles;
            try
            {
                msgFiles = Directory.GetFiles(inputFolder, "*.msg");
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Error enumerating MSG files: {ex.Message}");
                return;
            }

            // If no MSG files are present, create a minimal placeholder MSG
            if (msgFiles.Length == 0)
            {
                string placeholderPath = Path.Combine(inputFolder, "placeholder.msg");
                try
                {
                    using (MapiMessage placeholder = new MapiMessage(
                        "from@example.com",
                        "to@example.com",
                        "Placeholder Subject",
                        "Placeholder Body"))
                    {
                        placeholder.Save(placeholderPath);
                    }
                    Console.WriteLine($"Created placeholder MSG at '{placeholderPath}'.");
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to create placeholder MSG: {ex.Message}");
                    return;
                }

                // Refresh the file list after creating the placeholder
                try
                {
                    msgFiles = Directory.GetFiles(inputFolder, "*.msg");
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Error enumerating MSG files after placeholder creation: {ex.Message}");
                    return;
                }
            }

            foreach (string filePath in msgFiles)
            {
                try
                {
                    using (MapiMessage msg = MapiMessage.Load(filePath))
                    {
                        string subject = msg.Subject ?? "NoSubject";

                        // Remove illegal filename characters
                        char[] invalidChars = Path.GetInvalidFileNameChars();
                        foreach (char c in invalidChars)
                        {
                            subject = subject.Replace(c.ToString(), string.Empty);
                        }

                        // Trim whitespace and limit length if necessary
                        subject = subject.Trim();
                        if (subject.Length == 0)
                        {
                            subject = "NoSubject";
                        }

                        string newFileName = subject + ".msg";
                        string newFilePath = Path.Combine(inputFolder, newFileName);

                        // If the new filename already exists, append a numeric suffix
                        int suffix = 1;
                        while (File.Exists(newFilePath) && !string.Equals(filePath, newFilePath, StringComparison.OrdinalIgnoreCase))
                        {
                            newFileName = $"{subject}_{suffix}.msg";
                            newFilePath = Path.Combine(inputFolder, newFileName);
                            suffix++;
                        }

                        // Rename the file if the name has changed
                        if (!string.Equals(filePath, newFilePath, StringComparison.OrdinalIgnoreCase))
                        {
                            File.Move(filePath, newFilePath);
                            Console.WriteLine($"Renamed '{Path.GetFileName(filePath)}' to '{newFileName}'.");
                        }
                        else
                        {
                            Console.WriteLine($"File '{Path.GetFileName(filePath)}' already has the correct name.");
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Error processing file '{filePath}': {ex.Message}");
                    // Continue with next file
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unhandled exception: {ex.Message}");
        }
    }
}
