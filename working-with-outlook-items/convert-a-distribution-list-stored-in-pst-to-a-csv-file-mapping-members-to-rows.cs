using System;
using System.IO;
using System.Text;
using System.Collections.Generic;
using Aspose.Email;
using Aspose.Email.Storage.Pst;
using Aspose.Email.Mapi;

class Program
{
    static void Main()
    {
        try
        {
            string pstPath = "sample.pst";
            string csvPath = "distribution_list.csv";

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
                Directory.CreateDirectory(csvDirectory);
            }

            using (PersonalStorage pst = PersonalStorage.FromFile(pstPath))
            {
                // Collect all folders recursively starting from root
                List<FolderInfo> allFolders = new List<FolderInfo>();
                FolderInfo rootFolder = pst.RootFolder;
                allFolders.Add(rootFolder);

                for (int i = 0; i < allFolders.Count; i++)
                {
                    FolderInfo current = allFolders[i];
                    foreach (FolderInfo subFolder in current.GetSubFolders())
                    {
                        allFolders.Add(subFolder);
                    }
                }

                using (StreamWriter writer = new StreamWriter(csvPath, false, Encoding.UTF8))
                {
                    // Write CSV header
                    writer.WriteLine("DisplayName,EmailAddress");

                    foreach (FolderInfo folder in allFolders)
                    {
                        foreach (MessageInfo messageInfo in folder.EnumerateMessages())
                        {
                            using (MapiMessage message = pst.ExtractMessage(messageInfo))
                            {
                                if (message.SupportedType == MapiItemType.DistList)
                                {
                                    MapiDistributionList distributionList = (MapiDistributionList)message.ToMapiMessageItem();

                                    foreach (MapiDistributionListMember member in distributionList.Members)
                                    {
                                        string displayName = member.DisplayName ?? string.Empty;
                                        string emailAddress = member.EmailAddress ?? string.Empty;

                                        // Escape quotes for CSV
                                        displayName = displayName.Replace("\"", "\"\"");
                                        emailAddress = emailAddress.Replace("\"", "\"\"");

                                        writer.WriteLine($"\"{displayName}\",\"{emailAddress}\"");
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine(ex.Message);
            return;
        }
    }
}
