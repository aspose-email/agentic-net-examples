using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Aspose.Email.Storage.Pst;
using Aspose.Email.Mapi;

namespace PstMessageIterator
{
    // IEnumerable implementation that iterates over all MapiMessage objects in a PST file
    public class PstMessageEnumerable : IEnumerable<MapiMessage>
    {
        private readonly string _pstPath;

        public PstMessageEnumerable(string pstPath)
        {
            _pstPath = pstPath;
        }

        public IEnumerator<MapiMessage> GetEnumerator()
        {
            // Open the PST file inside a using block so it is disposed after enumeration
            using (PersonalStorage pst = PersonalStorage.FromFile(_pstPath))
            {
                // Iterate through each subfolder of the root folder
                foreach (FolderInfo folderInfo in pst.RootFolder.GetSubFolders())
                {
                    // Enumerate MAPI messages directly from the folder
                    foreach (MapiMessage message in folderInfo.EnumerateMapiMessages())
                    {
                        // Yield each message; the caller is responsible for disposing it
                        yield return message;
                    }
                }
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }

    class Program
    {
        static void Main()
        {
            try
            {
                string pstPath = "sample.pst";

                // Guard file existence
                if (!File.Exists(pstPath))
                {
                    Console.Error.WriteLine($"Error: File not found – {pstPath}");
                    return;
                }

                // Iterate over messages using the custom IEnumerable
                foreach (MapiMessage message in new PstMessageEnumerable(pstPath))
                {
                    // Ensure each message is disposed after use
                    using (MapiMessage msg = message)
                    {
                        Console.WriteLine($"Subject: {msg.Subject}");
                        Console.WriteLine($"From: {msg.SenderName}");
                        Console.WriteLine($"Sent: {msg.ClientSubmitTime}");
                        Console.WriteLine(new string('-', 40));
                    }
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Error: {ex.Message}");
            }
        }
    }
}
