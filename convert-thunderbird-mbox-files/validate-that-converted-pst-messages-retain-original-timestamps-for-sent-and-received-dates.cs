using Aspose.Email.Mapi;
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
            const string pstPath = "sample.pst";

            // Ensure PST file exists; create a minimal placeholder if missing
            if (!File.Exists(pstPath))
            {
                try
                {
                    // Create an empty Unicode PST file
                    PersonalStorage.Create(pstPath, FileFormatVersion.Unicode);
                    Console.WriteLine($"Created placeholder PST file at '{pstPath}'.");
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to create placeholder PST: {ex.Message}");
                    return;
                }
            }

            // Open the PST file
            try
            {
                using (PersonalStorage pst = PersonalStorage.FromFile(pstPath))
                {
                    // Iterate through all subfolders starting from the root
                    foreach (FolderInfo folder in pst.RootFolder.GetSubFolders())
                    {
                        Console.WriteLine($"Folder: {folder.DisplayName}");

                        foreach (MessageInfo messageInfo in folder.EnumerateMessages())
                        {
                            Console.WriteLine($"Subject: {messageInfo.Subject}");

                            // Extract the full MAPI message
                            using (MapiMessage mapiMessage = pst.ExtractMessage(messageInfo))
                            {
                                // Sent date (ClientSubmitTime) and received date (DeliveryTime)
                                DateTime? sentDate = mapiMessage.ClientSubmitTime;
                                DateTime? receivedDate = mapiMessage.DeliveryTime;

                                string sentStr = sentDate.HasValue ? sentDate.Value.ToString("o") : "N/A";
                                string receivedStr = receivedDate.HasValue ? receivedDate.Value.ToString("o") : "N/A";

                                Console.WriteLine($"Sent: {sentStr}");
                                Console.WriteLine($"Received: {receivedStr}");
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Error processing PST file: {ex.Message}");
                return;
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
