using System;
using System.Collections.Generic;
using System.IO;
using Aspose.Email;
using Aspose.Email.Storage.Pst;
using Aspose.Email.Mapi;

class Program
{
    static void Main()
    {
        try
        {
            string pstPath = "mailbox.pst";

            // Ensure a PST file exists; create a minimal one if missing
            if (!File.Exists(pstPath))
            {
                using (PersonalStorage pst = PersonalStorage.Create(pstPath, FileFormatVersion.Unicode))
                {
                    FolderInfo inbox = pst.RootFolder.AddSubFolder("Inbox");

                    // Create a sample message with voting options embedded in the body
                    MapiMessage sample = new MapiMessage
                    {
                        Subject = "Sample Voting Message",
                        Body = "Please vote.\nVotingOptions: Approve;Reject;Maybe"
                    };

                    inbox.AddMessage(sample);
                }

                Console.WriteLine($"Created placeholder PST file at '{pstPath}'.");
            }

            // Dictionary to hold voting button counts
            Dictionary<string, int> votingCounts = new Dictionary<string, int>(StringComparer.OrdinalIgnoreCase);

            // Open PST and process messages
            using (PersonalStorage pst = PersonalStorage.FromFile(pstPath))
            {
                ProcessFolder(pst.RootFolder, votingCounts);
            }

            // Display the dashboard
            Console.WriteLine("\nVoting Button Usage Dashboard:");
            if (votingCounts.Count == 0)
            {
                Console.WriteLine("No voting buttons found in the mailbox.");
            }
            else
            {
                foreach (KeyValuePair<string, int> entry in votingCounts)
                {
                    Console.WriteLine($"Button \"{entry.Key}\": {entry.Value} message(s)");
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }

    // Recursively process folders to collect voting button usage
    private static void ProcessFolder(FolderInfo folder, Dictionary<string, int> votingCounts)
    {
        foreach (MapiMessage message in folder.EnumerateMapiMessages())
        {
            // Attempt to extract voting options from the body.
            // Expected format: a line starting with "VotingOptions:" followed by semicolon‑separated options.
            string[] lines = message.Body?.Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries) ?? Array.Empty<string>();
            foreach (string line in lines)
            {
                if (line.StartsWith("VotingOptions:", StringComparison.OrdinalIgnoreCase))
                {
                    string optionsPart = line.Substring("VotingOptions:".Length).Trim();
                    if (!string.IsNullOrEmpty(optionsPart))
                    {
                        string[] buttons = optionsPart.Split(new[] { ';' }, StringSplitOptions.RemoveEmptyEntries);
                        foreach (string btn in buttons)
                        {
                            string trimmed = btn.Trim();
                            if (string.IsNullOrEmpty(trimmed))
                                continue;

                            if (votingCounts.ContainsKey(trimmed))
                                votingCounts[trimmed]++;
                            else
                                votingCounts[trimmed] = 1;
                        }
                    }
                }
            }
        }

        // Recurse into subfolders
        foreach (FolderInfo subFolder in folder.GetSubFolders())
        {
            ProcessFolder(subFolder, votingCounts);
        }
    }
}
