using System;
using System.IO;
using Aspose.Email;
using Aspose.Email.Mapi;
using Aspose.Email.Storage.Pst;

class Program
{
    static void Main()
    {
        try
        {
            string pstPath = "sample.pst";

            // Ensure the directory for the PST exists
            string pstDirectory = Path.GetDirectoryName(pstPath);
            if (!string.IsNullOrEmpty(pstDirectory) && !Directory.Exists(pstDirectory))
            {
                Directory.CreateDirectory(pstDirectory);
            }

            // Create the PST file if it does not exist
            if (!File.Exists(pstPath))
            {
                PersonalStorage.Create(pstPath, FileFormatVersion.Unicode);
            }

            // Open the PST file
            using (PersonalStorage pst = PersonalStorage.FromFile(pstPath))
            {
                // Get the Inbox folder; create it if missing
                FolderInfo inbox = pst.RootFolder.GetSubFolder("Inbox");
                if (inbox == null)
                {
                    inbox = pst.RootFolder.AddSubFolder("Inbox");
                }

                // Create a regular email message
                MapiMessage email = new MapiMessage(
                    "sender@example.com",
                    "recipient@example.com",
                    "Sample Subject",
                    "This is a sample body.");

                // Add the email message to the PST folder
                string entryId = inbox.AddMessage(email);
                Console.WriteLine($"Message added with EntryId: {entryId}");

                // Create a distribution list
                MapiDistributionListMember member = new MapiDistributionListMember("John Doe", "john.doe@example.com");
                MapiDistributionListMemberCollection members = new MapiDistributionListMemberCollection();
                members.Add(member);
                MapiDistributionList distList = new MapiDistributionList("Sample List", members);

                // Save the distribution list to a separate MSG file
                string dlPath = "SampleList.msg";
                distList.Save(dlPath);
                Console.WriteLine($"Distribution list saved to {dlPath}");
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
