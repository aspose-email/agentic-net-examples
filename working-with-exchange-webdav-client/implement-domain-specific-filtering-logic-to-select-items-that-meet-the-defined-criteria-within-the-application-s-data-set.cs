using System;
using System.IO;
using Aspose.Email;
using Aspose.Email.Storage.Mbox;

class Program
{
    static void Main(string[] args)
    {
        const string mboxPath = "storage.mbox";
        const string filterKeyword = "Important";

        // Verify that the MBOX file exists before proceeding.
        if (!File.Exists(mboxPath))
        {
            Console.Error.WriteLine($"Error: The file '{mboxPath}' does not exist.");
            return;
        }

        try
        {
            // Create an MboxStorageReader instance.
            MboxStorageReader mbox = MboxStorageReader.CreateReader(mboxPath, new MboxLoadOptions());

            // Iterate through each message info in the MBOX storage.
            foreach (MboxMessageInfo mboxMessageInfo in mbox.EnumerateMessageInfo())
            {
                // Apply simple filtering: process only messages whose subject contains the keyword.
                if (mboxMessageInfo.Subject != null && mboxMessageInfo.Subject.IndexOf(filterKeyword, StringComparison.OrdinalIgnoreCase) >= 0)
                {
                    // Extract the full MIME message.
                    MailMessage eml = mbox.ExtractMessage(mboxMessageInfo.EntryId, new EmlLoadOptions());

                    // Output basic information to the console.
                    Console.WriteLine($"Subject: {eml.Subject}");
                    Console.WriteLine($"From: {eml.From}");
                    Console.WriteLine($"To: {string.Join(", ", eml.To)}");

                    // Save the filtered message as an .eml file using the subject as part of the filename.
                    string safeSubject = string.IsNullOrWhiteSpace(eml.Subject) ? "Untitled" : eml.Subject.Replace(Path.GetInvalidFileNameChars(), '_');
                    string outputPath = $"{safeSubject}.eml";

                    // Ensure the directory for the output file exists.
                    string outputDir = Path.GetDirectoryName(outputPath);
                    if (!string.IsNullOrEmpty(outputDir) && !Directory.Exists(outputDir))
                    {
                        Directory.CreateDirectory(outputDir);
                    }

                    eml.Save(outputPath);
                    Console.WriteLine($"Saved filtered message to '{outputPath}'.");
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"An error occurred while processing the MBOX file: {ex.Message}");
        }
    }
}

// Extension method to replace invalid filename characters.
static class StringExtensions
{
    public static string Replace(this string str, char[] chars, char replacement)
    {
        foreach (char c in chars)
        {
            str = str.Replace(c, replacement);
        }
        return str;
    }
}
