using System;
using System.IO;
using System.Collections.Generic;
using Aspose.Email;
using Aspose.Email.Mapi;

class Program
{
    static void Main()
    {
        try
        {
            // Input MSG file path
            string msgPath = "sample.msg";

            // Ensure the input file exists; if not, create a minimal placeholder message
            if (!File.Exists(msgPath))
            {
                try
                {
                    using (MapiMessage placeholder = new MapiMessage("from@example.com", "to@example.com", "Sample Subject", "Sample body"))
                    {
                        placeholder.Save(msgPath);
                    }
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to create placeholder MSG file: {ex.Message}");
                    return;
                }
            }

            // Load the message
            MapiMessage message;
            try
            {
                message = MapiMessage.Load(msgPath);
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Failed to load MSG file: {ex.Message}");
                return;
            }

            using (message)
            {
                // Retrieve voting buttons
                string[] votingButtons;
                try
                {
                    votingButtons = FollowUpManager.GetVotingButtons(message);
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Error retrieving voting buttons: {ex.Message}");
                    votingButtons = new string[0];
                }

                // Retrieve follow‑up options
                FollowUpOptions followUpOptions;
                try
                {
                    followUpOptions = FollowUpManager.GetOptions(message);
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Error retrieving follow‑up options: {ex.Message}");
                    followUpOptions = null;
                }

                // Export voting buttons to CSV
                string votingCsvPath = "voting_buttons.csv";
                try
                {
                    using (StreamWriter votingWriter = new StreamWriter(votingCsvPath, false))
                    {
                        votingWriter.WriteLine("Button");
                        foreach (string button in votingButtons)
                        {
                            votingWriter.WriteLine($"\"{button}\"");
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to write voting buttons CSV: {ex.Message}");
                }

                // Export follow‑up options to CSV
                string flagCsvPath = "followup_flags.csv";
                try
                {
                    using (StreamWriter flagWriter = new StreamWriter(flagCsvPath, false))
                    {
                        // Header
                        flagWriter.WriteLine("FlagRequest,ReminderTime,DueDate,StartDate,Categories,VotingButtons");

                        if (followUpOptions != null)
                        {
                            // Prepare fields, handling possible nulls
                            string flagRequest = followUpOptions.FlagRequest ?? string.Empty;
                            string reminderTime = followUpOptions.ReminderTime != DateTime.MinValue ? followUpOptions.ReminderTime.ToString("o") : string.Empty;
                            string dueDate = followUpOptions.DueDate != DateTime.MinValue ? followUpOptions.DueDate.ToString("o") : string.Empty;
                            string startDate = followUpOptions.StartDate != DateTime.MinValue ? followUpOptions.StartDate.ToString("o") : string.Empty;
                            string categories = followUpOptions.Categories ?? string.Empty;
                            string voting = followUpOptions.VotingButtons ?? string.Empty;

                            // Write a single line with the collected data
                            flagWriter.WriteLine($"\"{flagRequest}\",\"{reminderTime}\",\"{dueDate}\",\"{startDate}\",\"{categories}\",\"{voting}\"");
                        }
                        else
                        {
                            // No options available; write empty line
                            flagWriter.WriteLine(",,,,,");
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to write follow‑up flags CSV: {ex.Message}");
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
