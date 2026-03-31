using Aspose.Email.Mapi;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Aspose.Email;
using Aspose.Email.Storage.Pst;

namespace PstMessageIterator
{
    // IEnumerable implementation for iterating messages in a PST file.
    public class PstMessageEnumerable : IEnumerable<MessageInfo>, IDisposable
    {
        private readonly PersonalStorage _personalStorage;
        private readonly FolderInfo _folderInfo;

        // Expose the underlying PersonalStorage for extracting messages.
        public PersonalStorage Storage => _personalStorage;

        // Constructor opens the PST file and selects the specified folder (default: Inbox).
        public PstMessageEnumerable(string pstFilePath, string folderEntryId = null)
        {
            // Load the PST file.
            _personalStorage = PersonalStorage.FromFile(pstFilePath);

            // If a specific folder entry ID is provided, use it; otherwise use the Inbox folder.
            if (!string.IsNullOrEmpty(folderEntryId))
            {
                _folderInfo = _personalStorage.GetFolderById(folderEntryId);
            }
            else
            {
                // Get the standard Inbox folder.
                _folderInfo = _personalStorage.GetPredefinedFolder(StandardIpmFolder.Inbox);
            }
        }

        // Return the enumerator that iterates over MessageInfo objects.
        public IEnumerator<MessageInfo> GetEnumerator()
        {
            return _folderInfo.EnumerateMessages().GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        // Dispose the PersonalStorage when done.
        public void Dispose()
        {
            _personalStorage?.Dispose();
        }
    }

    class Program
    {
        static void Main()
        {
            try
            {
                // Paths for the PST file and the output directory.
                string pstPath = "sample.pst";
                string outputDirectory = "ExtractedMessages";

                // Guard input PST file.
                if (!File.Exists(pstPath))
                {
                    // Create a minimal placeholder PST if it does not exist.
                    using (PersonalStorage.Create(pstPath, FileFormatVersion.Unicode))
                    {
                        // Empty PST created.
                    }
                    Console.Error.WriteLine($"Info: Created placeholder PST at '{pstPath}'.");
                }

                // Ensure the output directory exists.
                if (!Directory.Exists(outputDirectory))
                {
                    Directory.CreateDirectory(outputDirectory);
                }

                // Iterate over messages using the custom IEnumerable implementation.
                using (PstMessageEnumerable pstMessages = new PstMessageEnumerable(pstPath))
                {
                    foreach (MessageInfo messageInfo in pstMessages)
                    {
                        // Extract the full MAPI message.
                        using (MapiMessage mapiMessage = pstMessages.Storage.ExtractMessage(messageInfo))
                        {
                            // Build a safe file name from the subject.
                            string subject = mapiMessage.Subject ?? "NoSubject";
                            foreach (char invalidChar in Path.GetInvalidFileNameChars())
                            {
                                subject = subject.Replace(invalidChar, '_');
                            }

                            string outputPath = Path.Combine(outputDirectory, $"{subject}.msg");

                            // Save the message as an Outlook MSG file.
                            try
                            {
                                mapiMessage.Save(outputPath);
                                Console.WriteLine($"Saved: {outputPath}");
                            }
                            catch (Exception ex)
                            {
                                Console.Error.WriteLine($"Error saving message '{subject}': {ex.Message}");
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // Top‑level exception guard.
                Console.Error.WriteLine($"Unhandled error: {ex.Message}");
            }
        }
    }
}
