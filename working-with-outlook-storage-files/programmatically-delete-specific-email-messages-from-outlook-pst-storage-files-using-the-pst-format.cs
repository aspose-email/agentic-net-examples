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

            // Verify PST file exists
            if (!File.Exists(pstPath))
            {
                Console.Error.WriteLine($"Error: File not found – {pstPath}");
                return;
            }

            // Open PST file
            using (PersonalStorage pst = PersonalStorage.FromFile(pstPath))
            {
                // Access the Inbox folder (adjust folder name as needed)
                FolderInfo inbox = pst.RootFolder.GetSubFolder("Inbox");

                // Iterate through messages and delete those matching a condition
                foreach (MessageInfo msgInfo in inbox.EnumerateMessages())
                {
                    // Example condition: delete messages whose subject contains "DeleteMe"
                    if (msgInfo.Subject != null && msgInfo.Subject.Contains("DeleteMe"))
                    {
                        // Delete the message using its string entry ID
                        pst.DeleteItem(msgInfo.EntryIdString);
                    }
                }
                // Changes are saved when the PersonalStorage object is disposed
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine(ex.Message);
        }
    }
}
