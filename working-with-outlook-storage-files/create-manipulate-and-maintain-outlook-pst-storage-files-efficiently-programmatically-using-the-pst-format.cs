using System;
using System.IO;
using Aspose.Email;
using Aspose.Email.Storage.Pst;
using Aspose.Email.Mapi;

class Program
{
    static void Main(string[] args)
    {
        try
        {
            string pstPath = "sample.pst";
            string outputDirectory = "output";

            // Ensure output directory exists
            if (!Directory.Exists(outputDirectory))
            {
                Directory.CreateDirectory(outputDirectory);
            }

            // Create a new PST file if it does not exist
            if (!File.Exists(pstPath))
            {
                using (PersonalStorage pst = PersonalStorage.Create(pstPath, FileFormatVersion.Unicode))
                {
                    // Create a simple MAPI message
                    MapiMessage message = new MapiMessage(
                        "sender@example.com",
                        "receiver@example.com",
                        "Sample Subject",
                        "This is the body of the message.");

                    // Add the message to the root folder
                    string entryId = pst.RootFolder.AddMessage(message);

                    // Save the message as a .msg file (default Unicode format)
                    string msgPath = Path.Combine(outputDirectory, "SampleMessage.msg");
                    message.Save(msgPath);
                }
            }
            else
            {
                // Open existing PST and enumerate messages
                using (PersonalStorage pst = PersonalStorage.FromFile(pstPath))
                {
                    foreach (FolderInfo folder in pst.RootFolder.GetSubFolders())
                    {
                        foreach (MessageInfo info in folder.EnumerateMessages())
                        {
                            using (MapiMessage msg = pst.ExtractMessage(info))
                            {
                                Console.WriteLine($"Subject: {msg.Subject}");
                            }
                        }
                    }
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
