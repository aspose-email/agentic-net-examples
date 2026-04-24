using System;
using System.IO;
using System.Collections.Generic;
using Aspose.Email;
using Aspose.Email.Storage;
using Aspose.Email.Storage.Pst;
using Aspose.Email.Mapi;

class Program
{
    static void Main()
    {
        try
        {
            string mboxPath = "input.mbox";
            string pstPath = "output.pst";

            // Verify input file exists
            if (!File.Exists(mboxPath))
            {
                Console.Error.WriteLine($"Input MBOX file not found: {mboxPath}");
                return;
            }

            // Ensure output directory exists
            string pstDirectory = Path.GetDirectoryName(pstPath);
            if (!string.IsNullOrEmpty(pstDirectory) && !Directory.Exists(pstDirectory))
            {
                try
                {
                    Directory.CreateDirectory(pstDirectory);
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to create output directory: {ex.Message}");
                    return;
                }
            }

            // Convert MBOX to PST
            PersonalStorage pst = null;
            try
            {
                pst = MailStorageConverter.MboxToPst(mboxPath, pstPath);
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Conversion failed: {ex.Message}");
                return;
            }

            // Exclude messages with empty bodies
            try
            {
                List<string> entriesToDelete = new List<string>();

                foreach (FolderInfo folder in pst.RootFolder.GetSubFolders())
                {
                    foreach (MessageInfo msgInfo in folder.EnumerateMessages())
                    {
                        using (MapiMessage mapiMsg = pst.ExtractMessage(msgInfo))
                        {
                            string body = mapiMsg.Body ?? string.Empty;
                            if (string.IsNullOrWhiteSpace(body))
                            {
                                entriesToDelete.Add(msgInfo.EntryIdString);
                            }
                        }
                    }
                }

                foreach (string entryId in entriesToDelete)
                {
                    try
                    {
                        pst.DeleteItem(entryId);
                    }
                    catch (Exception delEx)
                    {
                        Console.Error.WriteLine($"Failed to delete message {entryId}: {delEx.Message}");
                    }
                }
            }
            finally
            {
                pst?.Dispose();
            }

            Console.WriteLine("Conversion completed. Empty-body messages were excluded.");
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
