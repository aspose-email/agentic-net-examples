using Aspose.Email.Tools.Search;
using System;
using System.Collections.Generic;
using System.IO;
using Aspose.Email;
using Aspose.Email.Storage.Pst;

class Program
{
    static void Main()
    {
        try
        {
            // Paths for the source PST and the filtered output PST
            string sourcePstPath = "sample.pst";
            string filteredPstPath = "filtered.pst";

            // Ensure the source PST file exists; if not, create a minimal placeholder PST
            if (!File.Exists(sourcePstPath))
            {
                // Ensure the directory for the PST exists
                string sourceDir = Path.GetDirectoryName(sourcePstPath);
                if (!string.IsNullOrEmpty(sourceDir) && !Directory.Exists(sourceDir))
                {
                    Directory.CreateDirectory(sourceDir);
                }

                // Create an empty PST file with Unicode format
                using (PersonalStorage placeholder = PersonalStorage.Create(sourcePstPath, FileFormatVersion.Unicode))
                {
                    // Optionally add a default folder to avoid an empty PST
                    placeholder.RootFolder.AddSubFolder("Inbox");
                }

                Console.WriteLine($"Placeholder PST created at '{sourcePstPath}'.");
            }

            // Open the existing PST file
            using (PersonalStorage pst = PersonalStorage.FromFile(sourcePstPath))
            {
                // Build a search query: find messages whose subject contains "Invoice"
                PersonalStorageQueryBuilder queryBuilder = new PersonalStorageQueryBuilder();
                queryBuilder.Subject.Contains("Invoice");
                MailQuery subjectQuery = queryBuilder.GetQuery();

                // Prepare the list of queries for the SplitInto operation
                List<MailQuery> queries = new List<MailQuery> { subjectQuery };

                // Ensure the output directory exists
                string filteredDir = Path.GetDirectoryName(filteredPstPath);
                if (!string.IsNullOrEmpty(filteredDir) && !Directory.Exists(filteredDir))
                {
                    Directory.CreateDirectory(filteredDir);
                }

                // Split the PST based on the criteria, creating a new PST with matching items
                pst.SplitInto(queries, filteredPstPath);

                Console.WriteLine($"Filtered PST created at '{filteredPstPath}'.");
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
