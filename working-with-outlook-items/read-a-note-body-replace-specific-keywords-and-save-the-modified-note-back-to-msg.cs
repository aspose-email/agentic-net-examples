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
            string inputPath = "note.msg";
            string outputPath = "note_modified.msg";

            // Ensure the input file exists; create a minimal placeholder if it does not.
            if (!File.Exists(inputPath))
            {
                try
                {
                    string inputDirectory = Path.GetDirectoryName(inputPath);
                    if (!string.IsNullOrEmpty(inputDirectory) && !Directory.Exists(inputDirectory))
                    {
                        Directory.CreateDirectory(inputDirectory);
                    }

                    using (MapiMessage placeholder = new MapiMessage())
                    {
                        placeholder.Subject = "Placeholder Note";
                        placeholder.Body = "This is a placeholder note body.";
                        placeholder.MessageClass = "IPM.StickyNote";
                        placeholder.Save(inputPath);
                    }

                    Console.WriteLine($"Placeholder MSG created at '{inputPath}'.");
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to create placeholder MSG: {ex.Message}");
                    return;
                }
            }

            // Load the existing MSG, modify its body, and save the result.
            try
            {
                using (MapiMessage message = MapiMessage.Load(inputPath))
                {
                    string originalBody = message.Body ?? string.Empty;

                    // Replace specific keywords in the body.
                    string modifiedBody = originalBody.Replace("oldKeyword", "newKeyword")
                                                      .Replace("sample", "example");

                    message.Body = modifiedBody;

                    // Ensure the output directory exists.
                    string outputDirectory = Path.GetDirectoryName(outputPath);
                    if (!string.IsNullOrEmpty(outputDirectory) && !Directory.Exists(outputDirectory))
                    {
                        Directory.CreateDirectory(outputDirectory);
                    }

                    // Save the modified message using Unicode MSG format.
                    MsgSaveOptions saveOptions = new MsgSaveOptions(MailMessageSaveType.OutlookMessageFormatUnicode);
                    message.Save(outputPath, saveOptions);
                }

                Console.WriteLine($"Modified MSG saved to '{outputPath}'.");
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Error processing MSG files: {ex.Message}");
                return;
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
