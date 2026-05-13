using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using Aspose.Email;
using Aspose.Email.Mapi;
using Aspose.Email.Storage.Pst;

namespace AsposeEmailVotingExport
{
    public class MessageVotingInfo
    {
        public string Subject { get; set; }
        public string[] VotingButtons { get; set; }
        public int VoteCount { get; set; }
    }

    public class Program
    {
        public static void Main(string[] args)
        {
            try
            {
                string pstPath = "sample.pst";
                string jsonPath = "votingButtons.json";

                // Guard input PST file existence
                if (!File.Exists(pstPath))
                {
                    Console.Error.WriteLine($"Input PST file not found: {pstPath}");
                    return;
                }

                // Ensure output directory exists
                string outputDir = Path.GetDirectoryName(jsonPath);
                if (!string.IsNullOrEmpty(outputDir) && !Directory.Exists(outputDir))
                {
                    try
                    {
                        Directory.CreateDirectory(outputDir);
                    }
                    catch (Exception dirEx)
                    {
                        Console.Error.WriteLine($"Failed to create output directory: {dirEx.Message}");
                        return;
                    }
                }

                List<MessageVotingInfo> results = new List<MessageVotingInfo>();

                // Open PST file
                using (PersonalStorage pst = PersonalStorage.FromFile(pstPath))
                {
                    // Process root folder and its subfolders recursively
                    ProcessFolder(pst.RootFolder, results, pst);
                }

                // Serialize results to JSON
                try
                {
                    string json = JsonSerializer.Serialize(results, new JsonSerializerOptions { WriteIndented = true });
                    File.WriteAllText(jsonPath, json);
                    Console.WriteLine($"Voting button data exported to: {jsonPath}");
                }
                catch (Exception writeEx)
                {
                    Console.Error.WriteLine($"Failed to write JSON file: {writeEx.Message}");
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Unexpected error: {ex.Message}");
            }
        }

        private static void ProcessFolder(FolderInfo folder, List<MessageVotingInfo> results, PersonalStorage pst)
        {
            // Enumerate messages in the current folder
            foreach (MessageInfo messageInfo in folder.EnumerateMessages())
            {
                using (MapiMessage message = pst.ExtractMessage(messageInfo))
                {
                    string[] votingButtons = FollowUpManager.GetVotingButtons(message);
                    // Voting count is not directly available; set to 0 as placeholder
                    results.Add(new MessageVotingInfo
                    {
                        Subject = message.Subject,
                        VotingButtons = votingButtons,
                        VoteCount = 0
                    });
                }
            }

            // Recursively process subfolders
            foreach (FolderInfo subFolder in folder.GetSubFolders())
            {
                ProcessFolder(subFolder, results, pst);
            }
        }
    }
}
