using System;
using System.IO;
using System.Text;
using Aspose.Email;
using Aspose.Email.Mapi;

class Program
{
    static void Main()
    {
        try
        {
            // Input MSG file path
            string inputMsgPath = "sample.msg";

            // Verify input file exists
            if (!File.Exists(inputMsgPath))
            {
                try
                {
                    using (MapiMessage placeholder = new MapiMessage(
                        "from@example.com",
                        "to@example.com",
                        "Placeholder Subject",
                        "Placeholder body."))
                    {
                        placeholder.Save(inputMsgPath);
                    }
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Error creating placeholder MSG: {ex.Message}");
                    return;
                }

                Console.Error.WriteLine($"Input file not found: {inputMsgPath}");
                return;
            }

            // Output CSV paths
            string votingCsvPath = "voting_buttons.csv";
            string optionsCsvPath = "followup_options.csv";

            // Ensure output directories exist
            try
            {
                string votingDir = Path.GetDirectoryName(votingCsvPath);
                if (!string.IsNullOrEmpty(votingDir) && !Directory.Exists(votingDir))
                {
                    Directory.CreateDirectory(votingDir);
                }

                string optionsDir = Path.GetDirectoryName(optionsCsvPath);
                if (!string.IsNullOrEmpty(optionsDir) && !Directory.Exists(optionsDir))
                {
                    Directory.CreateDirectory(optionsDir);
                }
            }
            catch (Exception dirEx)
            {
                Console.Error.WriteLine($"Failed to prepare output directories: {dirEx.Message}");
                return;
            }

            // Load the MSG file
            using (MapiMessage message = MapiMessage.Load(inputMsgPath))
            {
                // ----- Export voting buttons -----
                string[] votingButtons = FollowUpManager.GetVotingButtons(message);

                try
                {
                    using (StreamWriter writer = new StreamWriter(votingCsvPath, false, Encoding.UTF8))
                    {
                        writer.WriteLine("Button");
                        if (votingButtons != null)
                        {
                            foreach (string button in votingButtons)
                            {
                                // Escape double quotes by doubling them
                                string escaped = button?.Replace("\"", "\"\"");
                                writer.WriteLine($"\"{escaped}\"");
                            }
                        }
                    }
                }
                catch (Exception writeEx)
                {
                    Console.Error.WriteLine($"Failed to write voting buttons CSV: {writeEx.Message}");
                    // Continue to attempt writing options
                }

                // ----- Export follow‑up options -----
                FollowUpOptions options = FollowUpManager.GetOptions(message);

                try
                {
                    using (StreamWriter writer = new StreamWriter(optionsCsvPath, false, Encoding.UTF8))
                    {
                        writer.WriteLine("FlagRequest,DueDate,ReminderTime,Categories,VotingButtons,IsCompleted");
                        string flagRequest = options.FlagRequest ?? string.Empty;
                        string dueDate = options.DueDate != DateTime.MinValue ? options.DueDate.ToString("o") : string.Empty;
                        string reminderTime = options.ReminderTime != DateTime.MinValue ? options.ReminderTime.ToString("o") : string.Empty;
                        string categories = options.Categories ?? string.Empty;
                        string votingButtonsList = options.VotingButtons ?? string.Empty;
                        string isCompleted = options.IsCompleted.ToString();

                        // Escape commas in fields by surrounding with double quotes
                        writer.WriteLine($"\"{flagRequest}\",\"{dueDate}\",\"{reminderTime}\",\"{categories}\",\"{votingButtonsList}\",\"{isCompleted}\"");
                    }
                }
                catch (Exception writeEx)
                {
                    Console.Error.WriteLine($"Failed to write follow‑up options CSV: {writeEx.Message}");
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
