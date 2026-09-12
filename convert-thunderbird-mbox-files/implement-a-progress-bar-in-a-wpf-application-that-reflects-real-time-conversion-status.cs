using Aspose.Email;
using Aspose.Email.Mapi;
using Aspose.Email.Storage.Mbox;
using Aspose.Email.Storage.Pst;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace MboxToPstConsole
{
    class Program
    {
        private const string MboxFilePath = "sample.mbox";
        private const string PstFilePath = "output.pst";

        static async Task Main(string[] args)
        {
            try
            {
                await StartConversionAsync();
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Unhandled exception: {ex.Message}");
            }
        }

        private static async Task StartConversionAsync()
        {
            if (!File.Exists(MboxFilePath))
            {
                Console.Error.WriteLine($"Input MBOX file not found: {MboxFilePath}");
                return;
            }

            string pstDirectory = Path.GetDirectoryName(PstFilePath);
            if (!string.IsNullOrEmpty(pstDirectory) && !Directory.Exists(pstDirectory))
            {
                Directory.CreateDirectory(pstDirectory);
            }

            IProgress<int> progress = new Progress<int>(percent =>
            {
                Console.Write($"\rProgress: {percent}%   ");
            });

            await Task.Run(() => ConvertMboxToPst(MboxFilePath, PstFilePath, progress));
            Console.WriteLine("\nConversion completed successfully.");
        }

        private static void ConvertMboxToPst(string mboxPath, string pstPath, IProgress<int> progress)
        {
            using (PersonalStorage pst = PersonalStorage.Create(pstPath, FileFormatVersion.Unicode))
            {
                FolderInfo rootFolder = pst.RootFolder;

                using (MboxStorageReader mboxReader = MboxStorageReader.CreateReader(mboxPath, new MboxLoadOptions()))
                {
                    List<MboxMessageInfo> messageInfos = mboxReader.EnumerateMessageInfo().ToList();
                    int totalMessages = messageInfos.Count;
                    if (totalMessages == 0)
                    {
                        return;
                    }

                    int processed = 0;
                    foreach (MboxMessageInfo info in messageInfos)
                    {
                        MailMessage eml = mboxReader.ExtractMessage(info.EntryId, new EmlLoadOptions());
                        rootFolder.AddMessage(MapiMessage.FromMailMessage(eml));

                        processed++;
                        int percent = (int)((processed / (double)totalMessages) * 100);
                        progress.Report(percent);
                    }
                }
            }
        }
    }
}
