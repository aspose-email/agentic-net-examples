using Aspose.Email;
using System;
using System.IO;
using Aspose.Email.Storage.Pst;

class Program
{
    static void Main()
    {
        try
        {
            string pstPath = "sample.pst";
            string entryId = "EnterMessageEntryIdHere";
            string outputPath = "extracted_message.eml";

            if (!File.Exists(pstPath))
            {
                Console.Error.WriteLine($"PST file not found: {pstPath}");
                return;
            }

            string outputDirectory = Path.GetDirectoryName(outputPath);
            if (!string.IsNullOrEmpty(outputDirectory) && !Directory.Exists(outputDirectory))
            {
                try
                {
                    Directory.CreateDirectory(outputDirectory);
                }
                catch (Exception dirEx)
                {
                    Console.Error.WriteLine($"Failed to create output directory: {dirEx.Message}");
                    return;
                }
            }

            try
            {
                using (PersonalStorage pst = PersonalStorage.FromFile(pstPath))
                {
                    using (FileStream outputStream = new FileStream(outputPath, FileMode.Create, FileAccess.Write))
                    {
                        pst.SaveMessageToStream(entryId, outputStream);
                    }
                }

                Console.WriteLine($"Message with EntryId '{entryId}' extracted to '{outputPath}'.");
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Error extracting message: {ex.Message}");
                return;
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
