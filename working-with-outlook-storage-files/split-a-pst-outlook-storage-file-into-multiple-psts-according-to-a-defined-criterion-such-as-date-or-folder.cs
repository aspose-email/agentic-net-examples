using System;
using System.Collections.Generic;
using System.IO;
using Aspose.Email;
using Aspose.Email.Storage.Pst;
using Aspose.Email.Tools.Search;

namespace AsposeEmailPstSplitExample
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                // Paths (adjust as needed)
                string pstFilePath = "input.pst";
                string outputFolder = "SplitPstParts";

                // Create a placeholder PST file if it does not exist
                if (!File.Exists(pstFilePath))
                {
                    using (PersonalStorage pst = PersonalStorage.Create(pstFilePath, FileFormatVersion.Unicode))
                    {
                        // Add a default folder to make the PST valid
                        pst.RootFolder.AddSubFolder("Inbox");
                    }
                }

                // Ensure output directory exists
                if (!Directory.Exists(outputFolder))
                {
                    Directory.CreateDirectory(outputFolder);
                }

                // Define split criteria – e.g., messages received in the last 30 days
                var criteria = new List<MailQuery>();
                var builder = new MailQueryBuilder();
                builder.InternalDate.Since(DateTime.Now.AddDays(-30));
                MailQuery recentMessagesQuery = builder.GetQuery();
                criteria.Add(recentMessagesQuery);

                // Load the PST file and split it according to the criteria
                using (PersonalStorage pst = PersonalStorage.FromFile(pstFilePath))
                {
                    pst.SplitInto(criteria, outputFolder);
                }

                Console.WriteLine("PST splitting completed successfully.");
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Error: {ex.Message}");
            }
        }
    }
}
