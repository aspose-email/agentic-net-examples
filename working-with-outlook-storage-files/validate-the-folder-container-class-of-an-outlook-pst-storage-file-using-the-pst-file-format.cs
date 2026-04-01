using System;
using System.IO;
using Aspose.Email;
using Aspose.Email.Storage.Pst;

class Program
{
    static void Main()
    {
        try
        {
            string pstPath = "sample.pst";

            // Ensure the PST file exists; create a minimal one if missing.
            if (!File.Exists(pstPath))
            {
                try
                {
                    // Create a new Unicode PST file.
                    using (PersonalStorage pstCreate = PersonalStorage.Create(pstPath, FileFormatVersion.Unicode))
                    {
                        // Optionally create a default Inbox folder.
                        pstCreate.CreatePredefinedFolder("Inbox", StandardIpmFolder.Inbox);
                    }
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Error creating placeholder PST: {ex.Message}");
                    return;
                }
            }

            // Open the PST file.
            using (PersonalStorage pst = PersonalStorage.FromFile(pstPath))
            {
                // Retrieve the Inbox predefined folder.
                FolderInfo inboxFolder;
                try
                {
                    inboxFolder = pst.GetPredefinedFolder(StandardIpmFolder.Inbox);
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Error retrieving Inbox folder: {ex.Message}");
                    return;
                }

                // Validate the container class of the folder.
                try
                {
                    string containerClass = inboxFolder.ContainerClass;
                    Console.WriteLine($"Inbox folder container class: {containerClass}");
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Error accessing ContainerClass: {ex.Message}");
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
