using Aspose.Email;
using System;
using System.IO;
using Aspose.Email.Mapi;

// Author: Aspose.Email example - console UI for processing MSG files
class Program
{
    static void Main()
    {
        try
        {
            Console.WriteLine("Enter the full path to the MSG file:");
            string inputPath = Console.ReadLine();

            if (string.IsNullOrWhiteSpace(inputPath))
            {
                Console.Error.WriteLine("No file path was provided.");
                return;
            }

            // Guard file existence
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

                Console.Error.WriteLine($"File not found: {inputPath}");
                return;
            }

            // Load the Outlook Message file
            MapiMessage msg = MapiMessage.Load(inputPath);

            // Display basic properties
            Console.WriteLine($"Subject: {msg.Subject}");
            Console.WriteLine($"From: {msg.SenderName}");
            Console.WriteLine($"Body: {msg.Body}");

            // Process attachments if any
            if (msg.Attachments != null && msg.Attachments.Count > 0)
            {
                Console.WriteLine("Attachments:");
                foreach (MapiAttachment att in msg.Attachments)
                {
                    Console.WriteLine($"- {att.FileName}");

                    // Save attachment to the same directory as the input file
                    string outputDirectory = Path.GetDirectoryName(inputPath) ?? string.Empty;
                    string outputPath = Path.Combine(outputDirectory, att.FileName);

                    try
                    {
                        att.Save(outputPath);
                        Console.WriteLine($"  Saved to: {outputPath}");
                    }
                    catch (Exception ex)
                    {
                        Console.Error.WriteLine($"  Failed to save attachment '{att.FileName}': {ex.Message}");
                    }
                }
            }
            else
            {
                Console.WriteLine("No attachments found.");
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"An error occurred: {ex.Message}");
        }
    }
}
