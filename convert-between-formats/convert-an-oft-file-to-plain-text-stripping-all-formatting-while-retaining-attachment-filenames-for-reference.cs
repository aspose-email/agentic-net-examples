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
            string inputPath = "template.oft";
            string outputPath = "output.txt";

            // Verify input file exists
            if (!File.Exists(inputPath))
            {
                try
                {
                    using (MapiMessage placeholder = new MapiMessage(
                        "from@example.com",
                        "to@example.com",
                        "Placeholder Subject",
                        "Placeholder body."))
                    {
                        placeholder.Save(inputPath);
                    }
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Error creating placeholder MSG: {ex.Message}");
                    return;
                }

                Console.Error.WriteLine($"Input file not found: {inputPath}");
                return;
            }

            // Ensure output directory exists
            string outputDir = Path.GetDirectoryName(outputPath);
            if (!string.IsNullOrEmpty(outputDir) && !Directory.Exists(outputDir))
            {
                try
                {
                    Directory.CreateDirectory(outputDir);
                }
                catch (Exception dirEx)
                {
                    Console.Error.WriteLine($"Failed to create output directory: {dirEx.Message}");
                    return;
                }
            }

            // Load the OFT file and extract plain text and attachment names
            using (MapiMessage mapMessage = MapiMessage.Load(inputPath))
            {
                string bodyText = mapMessage.Body ?? string.Empty;

                List<string> attachmentFileNames = new List<string>();
                foreach (MapiAttachment attachment in mapMessage.Attachments)
                {
                    if (!string.IsNullOrEmpty(attachment.FileName))
                    {
                        attachmentFileNames.Add(attachment.FileName);
                    }
                }

                // Build the output content
                using (StreamWriter writer = new StreamWriter(outputPath, false))
                {
                    writer.WriteLine("Message Body:");
                    writer.WriteLine(bodyText);
                    writer.WriteLine();
                    writer.WriteLine("Attachments:");
                    if (attachmentFileNames.Count == 0)
                    {
                        writer.WriteLine("None");
                    }
                    else
                    {
                        foreach (string fileName in attachmentFileNames)
                        {
                            writer.WriteLine(fileName);
                        }
                    }
                }
            }

            Console.WriteLine($"Conversion completed. Output saved to {outputPath}");
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
