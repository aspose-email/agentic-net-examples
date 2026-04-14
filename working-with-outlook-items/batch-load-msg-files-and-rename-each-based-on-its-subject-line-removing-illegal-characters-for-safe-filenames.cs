using System;
using System.IO;
using System.Collections.Generic;
using Aspose.Email;
using Aspose.Email.Mapi;

class Program
{
    static void Main()
    {
        try
        {
            string inputFolder = "InputMsgs";
            string outputFolder = "RenamedMsgs";

            // Ensure input directory exists
            if (!Directory.Exists(inputFolder))
            {
                Console.Error.WriteLine($"Error: Input directory not found – {inputFolder}");
                return;
            }

            // Ensure output directory exists or create it
            if (!Directory.Exists(outputFolder))
            {
                try
                {
                    Directory.CreateDirectory(outputFolder);
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Error: Could not create output directory – {outputFolder}. {ex.Message}");
                    return;
                }
            }

            // Get all .msg files in the input folder
            string[] msgFiles;
            try
            {
                msgFiles = Directory.GetFiles(inputFolder, "*.msg");
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Error: Unable to enumerate files in {inputFolder}. {ex.Message}");
                return;
            }

            foreach (string msgFilePath in msgFiles)
            {
                if (!File.Exists(msgFilePath))
                {
                try
                {
                    using (MapiMessage placeholder = new MapiMessage(
                        "from@example.com",
                        "to@example.com",
                        "Placeholder Subject",
                        "Placeholder body."))
                    {
                        placeholder.Save(msgFilePath);
                    }
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Error creating placeholder MSG: {ex.Message}");
                    return;
                }

                    Console.Error.WriteLine($"Warning: File not found – {msgFilePath}");
                    continue;
                }

                try
                {
                    using (MapiMessage msg = MapiMessage.Load(msgFilePath))
                    {
                        string subject = msg.Subject ?? "Unnamed";
                        // Remove invalid filename characters
                        char[] invalidChars = Path.GetInvalidFileNameChars();
                        foreach (char c in invalidChars)
                        {
                            subject = subject.Replace(c.ToString(), string.Empty);
                        }
                        // Trim and replace remaining whitespace with underscore
                        subject = subject.Trim();
                        if (subject.Length == 0)
                        {
                            subject = "Unnamed";
                        }
                        subject = subject.Replace(' ', '_');

                        string newFileName = $"{subject}.msg";
                        string newFilePath = Path.Combine(outputFolder, newFileName);
                        int duplicateIndex = 1;
                        while (File.Exists(newFilePath))
                        {
                            newFileName = $"{subject}_{duplicateIndex}.msg";
                            newFilePath = Path.Combine(outputFolder, newFileName);
                            duplicateIndex++;
                        }

                        msg.Save(newFilePath);
                        Console.WriteLine($"Renamed '{Path.GetFileName(msgFilePath)}' to '{newFileName}'.");
                    }
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Error processing file '{msgFilePath}': {ex.Message}");
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
