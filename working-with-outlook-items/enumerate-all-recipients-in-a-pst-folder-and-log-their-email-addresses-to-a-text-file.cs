using System;
using System.IO;
using Aspose.Email;
using Aspose.Email.Storage.Pst;
using Aspose.Email.Mapi;
using System.Collections.Generic;

class Program
{
    static void Main()
    {
        try
        {
            // Paths for PST input and recipients output
            string pstFilePath = "sample.pst";
            string outputFilePath = "recipients.txt";

            // Verify that the PST file exists before proceeding
            if (!File.Exists(pstFilePath))
            {
                Console.Error.WriteLine($"PST file not found: {pstFilePath}");
                return;
            }

            // Ensure the directory for the output file exists
            string outputDirectory = Path.GetDirectoryName(outputFilePath);
            if (!string.IsNullOrEmpty(outputDirectory) && !Directory.Exists(outputDirectory))
            {
                Directory.CreateDirectory(outputDirectory);
            }

            // Open the PST file
            using (PersonalStorage pst = PersonalStorage.FromFile(pstFilePath))
            {
                // Use the root folder as the starting point
                FolderInfo rootFolder = pst.RootFolder;

                // Open a writer for the recipients log
                using (StreamWriter writer = new StreamWriter(outputFilePath, false))
                {
                    // Process the root folder and all its subfolders recursively
                    ProcessFolder(rootFolder, pst, writer);
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }

    // Recursively enumerates messages in a folder, extracts recipients, and writes email addresses
    private static void ProcessFolder(FolderInfo folder, PersonalStorage pst, StreamWriter writer)
    {
        // Enumerate each message in the current folder
        foreach (MessageInfo messageInfo in folder.EnumerateMessages())
        {
            try
            {
                // Extract the collection of recipients for the message
                MapiRecipientCollection recipients = pst.ExtractRecipients(messageInfo);

                // Write each recipient's email address to the output file
                foreach (MapiRecipient recipient in recipients)
                {
                    if (!string.IsNullOrEmpty(recipient.EmailAddress))
                    {
                        writer.WriteLine(recipient.EmailAddress);
                    }
                }
            }
            catch (Exception ex)
            {
                // Log extraction errors but continue processing other messages
                Console.Error.WriteLine($"Failed to extract recipients for message \"{messageInfo.Subject}\": {ex.Message}");
            }
        }

        // Recursively process all subfolders
        foreach (FolderInfo subFolder in folder.GetSubFolders())
        {
            ProcessFolder(subFolder, pst, writer);
        }
    }
}
