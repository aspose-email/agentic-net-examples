using System;
using System.IO;
using Aspose.Email;
using Aspose.Email.Storage.Pst;
using Aspose.Email.Mapi;

namespace AsposeEmailPstDistributionList
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                // Define PST file path
                string pstPath = "sample.pst";

                // Ensure the PST file exists; create a minimal PST if it does not
                if (!File.Exists(pstPath))
                {
                    try
                    {
                        PersonalStorage.Create(pstPath, FileFormatVersion.Unicode);
                        Console.WriteLine($"Created new PST file at '{pstPath}'.");
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
                    // Verify that the PST is writable
                    if (!pst.CanWrite)
                    {
                        Console.Error.WriteLine("The PST file is read‑only and cannot be modified.");
                        return;
                    }

                    // Get the root folder
                    FolderInfo rootFolder = pst.RootFolder;

                    // Locate or create a subfolder for distribution lists
                    FolderInfo distFolder = null;
                    foreach (FolderInfo subFolder in rootFolder.GetSubFolders())
                    {
                        if (string.Equals(subFolder.DisplayName, "Distribution Lists", StringComparison.OrdinalIgnoreCase))
                        {
                            distFolder = subFolder;
                            break;
                        }
                    }

                    if (distFolder == null)
                    {
                        try
                        {
                            distFolder = rootFolder.AddSubFolder("Distribution Lists");
                            Console.WriteLine("Created 'Distribution Lists' subfolder.");
                        }
                        catch (Exception ex)
                        {
                            Console.Error.WriteLine($"Error creating subfolder: {ex.Message}");
                            return;
                        }
                    }

                    // Create a new MAPI distribution list
                    MapiDistributionListMemberCollection members = new MapiDistributionListMemberCollection();
                    members.Add(new MapiDistributionListMember("Alice", "alice@example.com"));
                    members.Add(new MapiDistributionListMember("Bob", "bob@example.com"));

                    MapiDistributionList distributionList = new MapiDistributionList("Team", members);

                    // Add the distribution list to the folder
                    try
                    {
                        distFolder.AddMapiMessageItem(distributionList);
                        Console.WriteLine("Distribution list 'Team' added to PST.");
                    }
                    catch (Exception ex)
                    {
                        Console.Error.WriteLine($"Error adding distribution list: {ex.Message}");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Unexpected error: {ex.Message}");
            }
        }
    }
}
