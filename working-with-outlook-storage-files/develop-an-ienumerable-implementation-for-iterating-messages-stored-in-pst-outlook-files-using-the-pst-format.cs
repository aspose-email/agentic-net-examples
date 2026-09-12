using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Aspose.Email;
using Aspose.Email.Mapi;
using Aspose.Email.Storage.Pst;

namespace PstMessageEnumerator
{
    public class PstMessageEnumerable : IEnumerable<MailMessage>
    {
        private readonly string _pstFilePath;

        public PstMessageEnumerable(string pstFilePath)
        {
            _pstFilePath = pstFilePath;
        }

        public IEnumerator<MailMessage> GetEnumerator()
        {
            if (!File.Exists(_pstFilePath))
                yield break;

            using (PersonalStorage pst = PersonalStorage.FromFile(_pstFilePath))
            {
                // Enumerate root folder messages
                foreach (MessageInfo msgInfo in pst.RootFolder.EnumerateMessages())
                {
                    MapiMessage mapiMsg = pst.ExtractMessage(msgInfo);
                    yield return mapiMsg.ToMailMessage(new MailConversionOptions());
                }

                // Enumerate all subfolders recursively
                foreach (FolderInfo folder in GetAllSubFolders(pst.RootFolder))
                {
                    foreach (MessageInfo msgInfo in folder.EnumerateMessages())
                    {
                        MapiMessage mapiMsg = pst.ExtractMessage(msgInfo);
                        yield return mapiMsg.ToMailMessage(new MailConversionOptions());
                    }
                }
            }
        }

        private IEnumerable<FolderInfo> GetAllSubFolders(FolderInfo parent)
        {
            foreach (FolderInfo subFolder in parent.GetSubFolders())
            {
                yield return subFolder;
                foreach (var deeper in GetAllSubFolders(subFolder))
                    yield return deeper;
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                string pstPath = "storage.pst";

                // Create a minimal PST file if it does not exist (placeholder)
                if (!File.Exists(pstPath))
                {
                    using (PersonalStorage pst = PersonalStorage.Create(pstPath, FileFormatVersion.Unicode))
                    {
                        // Create a sample folder
                        FolderInfo inbox = pst.RootFolder.AddSubFolder("Inbox");

                        // Create a sample mail message
                        MailMessage sample = new MailMessage("sender@example.com", "recipient@example.com", "Sample Subject", "Sample body");

                        // Convert to MAPI message and add to the folder
                        MapiMessage mapiSample = MapiMessage.FromMailMessage(sample);
                        inbox.AddMessage(mapiSample);
                    }
                }

                PstMessageEnumerable messages = new PstMessageEnumerable(pstPath);

                foreach (MailMessage mailMessage in messages)
                {
                    Console.WriteLine($"Subject: {mailMessage.Subject}");
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Error: {ex.Message}");
            }
        }
    }
}
