using System;
using System.IO;
using Aspose.Email;
using Aspose.Email.Storage.Mbox;
using Aspose.Email.Tools.Search;

class Program
{
    static void Main()
    {
        try
        {
            // Path to the source MBOX file.
            const string mboxPath = "storage.mbox";
            if (!File.Exists(mboxPath))
            {
                Console.Error.WriteLine($"Input file not found: {mboxPath}");
                return;
            }

            // Directory where matched messages will be saved.
            const string outputDir = "output";
            Directory.CreateDirectory(outputDir);

            // Create the MboxStorageReader.
            MboxStorageReader mboxReader = MboxStorageReader.CreateReader(mboxPath, new MboxLoadOptions());

            // Build a query to filter messages by subject line.
            MailQueryBuilder queryBuilder = new MailQueryBuilder();
            queryBuilder.Subject.Contains("Your Subject Filter"); // replace with desired text
            MailQuery subjectQuery = queryBuilder.GetQuery();

            // Enumerate messages that match the subject filter.
            foreach (MailMessage message in mboxReader.EnumerateMessages(subjectQuery))
            {
                Console.WriteLine($"Subject: {message.Subject}");
                Console.WriteLine($"From: {message.From}");
                Console.WriteLine($"To: {message.To}");

                // Save the matched message as an .eml file.
                string safeFileName = $"{SanitizeFileName(message.Subject)}.eml";
                string fullPath = Path.Combine(outputDir, safeFileName);
                message.Save(fullPath);
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }

    // Replaces invalid filename characters with an underscore.
    private static string SanitizeFileName(string name)
    {
        if (string.IsNullOrEmpty(name))
            return "unnamed";

        foreach (char c in Path.GetInvalidFileNameChars())
        {
            name = name.Replace(c, '_');
        }

        // Trim to a reasonable length.
        return name.Length > 100 ? name.Substring(0, 100) : name;
    }
}
