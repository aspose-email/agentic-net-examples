using System;
using System.IO;
using Aspose.Email;
using Aspose.Email.Mapi;
using Aspose.Email.Storage.Pst;

class Program
{
    static void Main(string[] args)
    {
        try
        {
            // Path to the PST file
            const string pstPath = "storage.pst";

            // Verify PST file exists
            if (!File.Exists(pstPath))
            {
                Console.Error.WriteLine($"PST file not found: {pstPath}");
                return;
            }

            // Obtain the EntryId of the message to extract
            string entryId;
            if (args.Length > 0 && !string.IsNullOrWhiteSpace(args[0]))
            {
                entryId = args[0];
            }
            else
            {
                Console.Write("Enter the message EntryId: ");
                entryId = Console.ReadLine();
                if (string.IsNullOrWhiteSpace(entryId))
                {
                    Console.Error.WriteLine("EntryId cannot be empty.");
                    return;
                }
            }

            // Open the PST file
            using (PersonalStorage pst = PersonalStorage.FromFile(pstPath))
            {
                // Extract the message by EntryId
                MapiMessage message = pst.ExtractMessage(entryId);

                if (message == null)
                {
                    Console.Error.WriteLine($"No message found with EntryId: {entryId}");
                    return;
                }

                // Prepare a safe file name using the subject
                string safeSubject = string.IsNullOrWhiteSpace(message.Subject) ? "Untitled" : message.Subject;
                foreach (char c in Path.GetInvalidFileNameChars())
                {
                    safeSubject = safeSubject.Replace(c, '_');
                }

                // Ensure output directory exists
                string outputDir = "output";
                Directory.CreateDirectory(outputDir);

                // Build full output path
                string outputPath = Path.Combine(outputDir, $"{safeSubject}.msg");

                // Save the extracted message
                message.Save(outputPath);
                Console.WriteLine($"Message saved to: {outputPath}");
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
