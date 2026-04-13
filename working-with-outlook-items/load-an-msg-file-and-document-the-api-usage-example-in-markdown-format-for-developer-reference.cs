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
            // Path to the MSG file
            string msgFilePath = "sample.msg";

            // Verify that the input file exists
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

                Console.Error.WriteLine($"Input file '{msgFilePath}' not found.");
                return;
            }

            // Load the MSG file
            using (MapiMessage message = MapiMessage.Load(msgFilePath))
            {
                // Extract basic properties
                string subject = message.Subject ?? string.Empty;
                string senderName = message.SenderName ?? string.Empty;
                string body = message.Body ?? string.Empty;

                // Build markdown documentation
                string markdownContent =
                    "# MSG File Load Example\n\n" +
                    $"**File:** `{msgFilePath}`\n\n" +
                    "## Subject\n\n" + subject + "\n\n" +
                    "## Sender\n\n" + senderName + "\n\n" +
                    "## Body\n\n" + body + "\n";

                // Path for the generated markdown file
                string markdownFilePath = "MsgLoadExample.md";

                try
                {
                    // Ensure the output directory exists
                    string outputDirectory = Path.GetDirectoryName(markdownFilePath);
                    if (!string.IsNullOrEmpty(outputDirectory) && !Directory.Exists(outputDirectory))
                    {
                        Directory.CreateDirectory(outputDirectory);
                    }

                    // Write markdown to file
                    using (StreamWriter writer = new StreamWriter(markdownFilePath, false))
                    {
                        writer.Write(markdownContent);
                    }

                    Console.WriteLine($"Markdown documentation written to '{markdownFilePath}'.");
                }
                catch (Exception ioEx)
                {
                    Console.Error.WriteLine($"Failed to write markdown file: {ioEx.Message}");
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
