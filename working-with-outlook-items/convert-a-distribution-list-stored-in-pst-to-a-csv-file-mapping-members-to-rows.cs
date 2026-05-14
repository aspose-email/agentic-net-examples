using System;
using System.IO;
using System.Collections.Generic;
using Aspose.Email;
using Aspose.Email.Mapi;
using Aspose.Email.Storage.Pst;

class Program
{
    static void Main(string[] args)
    {
        try
        {
            string pstPath = "input.pst";
            string csvPath = "distribution_list.csv";

            if (!File.Exists(pstPath))
            {
                Console.Error.WriteLine($"PST file not found: {pstPath}");
                return;
            }

            string csvDirectory = Path.GetDirectoryName(csvPath);
            if (!string.IsNullOrEmpty(csvDirectory) && !Directory.Exists(csvDirectory))
            {
                Directory.CreateDirectory(csvDirectory);
            }

            using (StreamWriter writer = new StreamWriter(csvPath, false))
            {
                writer.WriteLine("DistributionList,MemberDisplayName,MemberEmailAddress,MemberAddressType");

                using (PersonalStorage pst = PersonalStorage.FromFile(pstPath))
                {
                    Stack<FolderInfo> folderStack = new Stack<FolderInfo>();
                    folderStack.Push(pst.RootFolder);

                    while (folderStack.Count > 0)
                    {
                        FolderInfo currentFolder = folderStack.Pop();

                        foreach (FolderInfo subFolder in currentFolder.GetSubFolders())
                        {
                            folderStack.Push(subFolder);
                        }

                        foreach (MessageInfo messageInfo in currentFolder.EnumerateMessages())
                        {
                            using (MapiMessage mapiMessage = pst.ExtractMessage(messageInfo))
                            {
                                if (mapiMessage.SupportedType == MapiItemType.DistList)
                                {
                                    MapiDistributionList distributionList = (MapiDistributionList)mapiMessage.ToMapiMessageItem();

                                    foreach (MapiDistributionListMember member in distributionList.Members)
                                    {
                                        string line = $"{EscapeCsv(distributionList.DisplayName)},{EscapeCsv(member.DisplayName)},{EscapeCsv(member.EmailAddress)},{EscapeCsv(member.AddressType)}";
                                        writer.WriteLine(line);
                                    }
                                }
                            }
                        }
                    }
                }
            }

            Console.WriteLine($"Distribution list exported to {csvPath}");
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }

    static string EscapeCsv(string field)
    {
        if (field == null)
            return "";
        if (field.Contains(",") || field.Contains("\"") || field.Contains("\n"))
        {
            string escaped = field.Replace("\"", "\"\"");
            return $"\"{escaped}\"";
        }
        return field;
    }
}
