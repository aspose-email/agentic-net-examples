using System;
using System.Collections.Generic;
using System.IO;
using Aspose.Email;
using Aspose.Email.Mapi;
using Aspose.Email.Storage.Pst;
using Aspose.Email.Tools.Search;

namespace PSTCriteriaSearch
{
    class Program
    {
        static void Main()
        {
            try
            {
                // Ensure the source PST file exists before proceeding.
                string sourcePstPath = "storage.pst";
                if (!File.Exists(sourcePstPath))
                {
                    Console.Error.WriteLine($"Source PST file not found: {sourcePstPath}");
                    return;
                }

                // Define the output PST that will contain the filtered messages.
                string filteredPstPath = "filtered.pst";
                if (File.Exists(filteredPstPath))
                {
                    // Remove any previous filtered PST to avoid conflicts.
                    File.Delete(filteredPstPath);
                }

                // Open the source PST file.
                using (PersonalStorage sourcePst = PersonalStorage.FromFile(sourcePstPath))
                {
                    // Build a criteria: messages whose subject contains the word "Invoice".
                    MailQueryBuilder queryBuilder = new MailQueryBuilder();
                    queryBuilder.Subject.Contains("Invoice");
                    MailQuery subjectQuery = queryBuilder.GetQuery();

                    // Add the query to a list (SplitInto accepts a list of queries).
                    List<MailQuery> queries = new List<MailQuery> { subjectQuery };

                    // Split the PST based on the criteria, creating a new PST with matching items.
                    sourcePst.SplitInto(queries, filteredPstPath);
                }

                // Verify that the filtered PST was created.
                if (!File.Exists(filteredPstPath))
                {
                    Console.Error.WriteLine("Filtered PST was not created.");
                    return;
                }

                // Prepare output directory for extracted .msg files.
                string outputDir = "output";
                Directory.CreateDirectory(outputDir);

                // Open the filtered PST and enumerate the matching messages.
                using (PersonalStorage filteredPst = PersonalStorage.FromFile(filteredPstPath))
                {
                    foreach (FolderInfo folderInfo in filteredPst.RootFolder.GetSubFolders())
                    {
                        Console.WriteLine($"Folder: {folderInfo.DisplayName}");
                        foreach (MessageInfo messageInfo in folderInfo.EnumerateMessages())
                        {
                            // Extract the full message as a MapiMessage.
                            MapiMessage mapiMessage = filteredPst.ExtractMessage(messageInfo);
                            Console.WriteLine($"Subject: {mapiMessage.Subject}");

                            // Create a safe file name.
                            string safeFileName = string.IsNullOrWhiteSpace(mapiMessage.Subject) ? "Untitled" : mapiMessage.Subject;
                            foreach (char c in Path.GetInvalidFileNameChars())
                            {
                                safeFileName = safeFileName.Replace(c, '_');
                            }

                            // Build the full output path and ensure the directory exists.
                            string outputPath = Path.Combine(outputDir, $"{safeFileName}.msg");
                            mapiMessage.Save(outputPath);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Error: {ex.Message}");
            }
        }
    }
}
