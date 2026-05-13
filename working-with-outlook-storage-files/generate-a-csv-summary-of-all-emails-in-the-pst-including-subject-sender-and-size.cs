using Aspose.Email.Mapi;
using System;
using System.IO;
using System.Text;
using Aspose.Email;
using Aspose.Email.Storage.Pst;

class Program
{
    static void Main()
    {
        try
        {
            string pstPath = "sample.pst";
            string csvPath = "emails_summary.csv";

            // Verify PST file exists
            if (!File.Exists(pstPath))
            {
                Console.Error.WriteLine($"PST file not found: {pstPath}");
                return;
            }

            // Ensure output directory exists
            string csvDirectory = Path.GetDirectoryName(csvPath);
            if (!string.IsNullOrEmpty(csvDirectory) && !Directory.Exists(csvDirectory))
            {
                try
                {
                    Directory.CreateDirectory(csvDirectory);
                }
                catch (Exception dirEx)
                {
                    Console.Error.WriteLine($"Failed to create directory '{csvDirectory}': {dirEx.Message}");
                    return;
                }
            }

            // Open PST file
            try
            {
                using (PersonalStorage pst = PersonalStorage.FromFile(pstPath))
                {
                    // Prepare CSV writer
                    try
                    {
                        using (StreamWriter csvWriter = new StreamWriter(csvPath, false, Encoding.UTF8))
                        {
                            // Write CSV header
                            csvWriter.WriteLine("Subject,Sender,Size");

                            // Process root folder and its subfolders recursively
                            ProcessFolder(pst.RootFolder, pst, csvWriter);
                        }
                    }
                    catch (Exception ioEx)
                    {
                        Console.Error.WriteLine($"Error writing CSV file: {ioEx.Message}");
                        return;
                    }
                }
            }
            catch (Exception pstEx)
            {
                Console.Error.WriteLine($"Error opening PST file: {pstEx.Message}");
                return;
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }

    private static void ProcessFolder(FolderInfo folder, PersonalStorage pst, StreamWriter csvWriter)
    {
        // Enumerate messages in the current folder
        foreach (MessageInfo messageInfo in folder.EnumerateMessages())
        {
            // Extract full message
            try
            {
                using (MapiMessage message = pst.ExtractMessage(messageInfo))
                {
                    // Determine size by saving to a memory stream
                    long messageSize = 0;
                    using (MemoryStream ms = new MemoryStream())
                    {
                        message.Save(ms);
                        messageSize = ms.Length;
                    }

                    // Prepare CSV fields (escape commas if needed)
                    string subject = EscapeCsvField(messageInfo.Subject);
                    string sender = EscapeCsvField(messageInfo.SenderRepresentativeName);
                    csvWriter.WriteLine($"{subject},{sender},{messageSize}");
                }
            }
            catch (Exception msgEx)
            {
                Console.Error.WriteLine($"Failed to process message '{messageInfo.Subject}': {msgEx.Message}");
                // Continue with next message
            }
        }

        // Recursively process subfolders
        foreach (FolderInfo subFolder in folder.GetSubFolders())
        {
            ProcessFolder(subFolder, pst, csvWriter);
        }
    }

    private static string EscapeCsvField(string field)
    {
        if (field == null)
            return string.Empty;

        if (field.Contains(",") || field.Contains("\"") || field.Contains("\n") || field.Contains("\r"))
        {
            string escaped = field.Replace("\"", "\"\"");
            return $"\"{escaped}\"";
        }
        else
        {
            return field;
        }
    }
}
