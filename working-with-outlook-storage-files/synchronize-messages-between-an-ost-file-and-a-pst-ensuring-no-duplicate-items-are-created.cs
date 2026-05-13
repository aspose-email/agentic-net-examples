using Aspose.Email;
using System;
using System.Collections.Generic;
using System.IO;
using Aspose.Email.Mapi;
using Aspose.Email.Storage.Pst;

class Program
{
    static void Main()
    {
        try
        {
            string ostFilePath = "source.ost";
            string pstFilePath = "target.pst";

            if (!File.Exists(ostFilePath))
            {
                Console.Error.WriteLine($"OST file not found: {ostFilePath}");
                return;
            }

            // Open or create PST storage
            PersonalStorage pstStorage;
            try
            {
                if (File.Exists(pstFilePath))
                {
                    pstStorage = PersonalStorage.FromFile(pstFilePath);
                }
                else
                {
                    pstStorage = PersonalStorage.Create(pstFilePath, FileFormatVersion.Unicode);
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Failed to open or create PST file: {ex.Message}");
                return;
            }

            using (pstStorage)
            {
                FolderInfo pstRootFolder = pstStorage.RootFolder;

                // Build a set of existing subjects to avoid duplicates
                var existingSubjects = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
                foreach (MessageInfo msgInfo in pstRootFolder.EnumerateMessages())
                {
                    if (!string.IsNullOrEmpty(msgInfo.Subject))
                        existingSubjects.Add(msgInfo.Subject);
                }

                // Open OST storage
                using (PersonalStorage ostStorage = PersonalStorage.FromFile(ostFilePath))
                {
                    FolderInfo ostRootFolder = ostStorage.RootFolder;

                    foreach (MessageInfo ostMsgInfo in ostRootFolder.EnumerateMessages())
                    {
                        if (string.IsNullOrEmpty(ostMsgInfo.Subject))
                            continue; // skip items without subject

                        if (existingSubjects.Contains(ostMsgInfo.Subject))
                            continue; // duplicate found

                        MapiMessage mapiMessage;
                        try
                        {
                            mapiMessage = ostStorage.ExtractMessage(ostMsgInfo);
                        }
                        catch (Exception ex)
                        {
                            Console.Error.WriteLine($"Failed to extract message '{ostMsgInfo.Subject}': {ex.Message}");
                            continue;
                        }

                        using (mapiMessage)
                        {
                            try
                            {
                                pstRootFolder.AddMessage(mapiMessage);
                                existingSubjects.Add(ostMsgInfo.Subject);
                            }
                            catch (Exception ex)
                            {
                                Console.Error.WriteLine($"Failed to add message '{ostMsgInfo.Subject}' to PST: {ex.Message}");
                            }
                        }
                    }
                }
                // Disposing pstStorage persists changes; no SaveAs needed.
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
