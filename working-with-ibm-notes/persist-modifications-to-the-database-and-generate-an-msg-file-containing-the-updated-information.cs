using Aspose.Email;
using System;
using System.IO;
using Aspose.Email.Storage.Pst;
using Aspose.Email.Mapi;

class Program
{
    static void Main()
    {
        try
        {
            // Input PST file path
            const string pstPath = "storage.pst";

            // Output directory for the generated MSG file
            const string outputDir = "Output";

            // Ensure the output directory exists
            if (!Directory.Exists(outputDir))
            {
                Directory.CreateDirectory(outputDir);
            }

            // Open the PST file
            using (PersonalStorage pst = PersonalStorage.FromFile(pstPath))
            {
                // Access the root folder
                FolderInfo rootFolder = pst.RootFolder;

                // Enumerate messages in the root folder
                foreach (MessageInfo messageInfo in rootFolder.EnumerateMessages())
                {
                    // Extract the full MAPI message
                    MapiMessage message = pst.ExtractMessage(messageInfo);

                    // Modify the message (example: prepend "Updated" to the subject)
                    string originalSubject = message.Subject ?? "NoSubject";
                    message.Subject = "Updated " + originalSubject;

                    // Persist the updated message back into the PST
                    rootFolder.UpdateMessage(messageInfo.EntryIdString, message);

                    // Save the updated message as an MSG file
                    string safeFileName = GetSafeFileName(message.Subject) + ".msg";
                    string msgPath = Path.Combine(outputDir, safeFileName);
                    message.Save(msgPath);

                    // Process only the first message for this example
                    break;
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine("Error: " + ex.Message);
        }
    }

    // Helper to create a file-system‑safe filename from a subject
    private static string GetSafeFileName(string name)
    {
        foreach (char c in Path.GetInvalidFileNameChars())
        {
            name = name.Replace(c, '_');
        }
        return string.IsNullOrWhiteSpace(name) ? "Message" : name;
    }
}
