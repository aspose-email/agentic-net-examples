using Aspose.Email.Storage.Pst;
using System;
using System.IO;
using Aspose.Email;
using Aspose.Email.Mapi;
using Aspose.Email.Storage;

namespace OutlookDistributionListPstSample
{
    class Program
    {
        static void Main()
        {
            try
            {
                // Define PST file path
                string pstPath = "OutlookDistributionLists.pst";

                // Ensure the directory for the PST file exists
                string pstDirectory = Path.GetDirectoryName(Path.GetFullPath(pstPath));
                if (!Directory.Exists(pstDirectory))
                {
                    Directory.CreateDirectory(pstDirectory);
                }

                // Create a new PST file if it does not exist, otherwise open existing one
                PersonalStorage pst;
                if (!File.Exists(pstPath))
                {
                    pst = PersonalStorage.Create(pstPath, FileFormatVersion.Unicode);
                }
                else
                {
                    pst = PersonalStorage.FromFile(pstPath);
                }

                using (pst)
                {
                    // Create (or get) a folder to store distribution lists
                    FolderInfo distFolder;
                    try
                    {
                        distFolder = pst.RootFolder.GetSubFolder("DistributionLists");
                    }
                    catch (ArgumentException)
                    {
                        // Folder does not exist; create it
                        distFolder = pst.RootFolder.AddSubFolder("DistributionLists");
                    }

                    // Prepare distribution list members
                    MapiDistributionListMemberCollection members = new MapiDistributionListMemberCollection();
                    members.Add(new MapiDistributionListMember("John Doe", "john.doe@example.com"));
                    members.Add(new MapiDistributionListMember("Jane Smith", "jane.smith@example.com"));

                    // Create the distribution list
                    MapiDistributionList distributionList = new MapiDistributionList("Team Contacts", members);

                    // Retrieve the underlying MAPI message representation
                    MapiMessage underlyingMessage = distributionList.GetUnderlyingMessage();

                    // Add the distribution list message to the PST folder
                    distFolder.AddMessage(underlyingMessage);
                }

                Console.WriteLine("Distribution list successfully saved to PST.");
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Error: {ex.Message}");
                return;
            }
        }
    }
}
