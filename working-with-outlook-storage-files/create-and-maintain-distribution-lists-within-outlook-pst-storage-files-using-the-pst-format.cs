using System;
using System.IO;
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

            // Ensure the PST file exists; create if missing
            if (!File.Exists(pstPath))
            {
                try
                {
                    PersonalStorage.Create(pstPath, FileFormatVersion.Unicode);
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Error creating PST file: {ex.Message}");
                    return;
                }
            }

            // Open the PST file
            using (PersonalStorage pst = PersonalStorage.FromFile(pstPath))
            {
                // Get the root folder
                FolderInfo rootFolder = pst.RootFolder;

                // Create a new distribution list
                MapiDistributionList dl = new MapiDistributionList();
                dl.DisplayName = "Sample Distribution List";

                // Add members to the distribution list
                dl.Members.Add(new MapiDistributionListMember("Alice", "alice@example.com"));
                dl.Members.Add(new MapiDistributionListMember("Bob", "bob@example.com"));
                dl.Members.Add(new MapiDistributionListMember("Charlie", "charlie@example.com"));

                // Convert the distribution list to a MapiMessage and add it to the PST
                using (MapiMessage dlMessage = dl.GetUnderlyingMessage())
                {
                    rootFolder.AddMessage(dlMessage);
                }

                // List all distribution lists in the PST
                foreach (MessageInfo msgInfo in rootFolder.EnumerateMessages())
                {
                    using (MapiMessage msg = pst.ExtractMessage(msgInfo))
                    {
                        if (msg.SupportedType == MapiItemType.DistList)
                        {
                            MapiDistributionList existingDl = (MapiDistributionList)msg.ToMapiMessageItem();
                            Console.WriteLine($"Distribution List: {existingDl.DisplayName}");
                            Console.WriteLine($"Members count: {existingDl.Members.Count}");
                            foreach (MapiDistributionListMember member in existingDl.Members)
                            {
                                Console.WriteLine($" - {member.DisplayName} <{member.EmailAddress}>");
                            }
                        }
                    }
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
