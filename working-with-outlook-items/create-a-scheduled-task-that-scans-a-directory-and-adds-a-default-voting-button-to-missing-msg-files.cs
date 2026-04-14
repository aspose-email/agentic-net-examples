using System;
using System.IO;
using Aspose.Email;
using Aspose.Email.Mapi;

class Program
{
    static void Main()
    {
        try
        {
            string directoryPath = @"C:\Emails";

            if (!Directory.Exists(directoryPath))
            {
                Console.Error.WriteLine($"Error: Directory not found – {directoryPath}");
                return;
            }

            string[] msgFiles;
            try
            {
                msgFiles = Directory.GetFiles(directoryPath, "*.msg");
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Error retrieving files: {ex.Message}");
                return;
            }

            foreach (string msgFilePath in msgFiles)
            {
                if (!File.Exists(msgFilePath))
                {
                try
                {
                    using (MapiMessage placeholder = new MapiMessage(
                        "from@example.com",
                        "to@example.com",
                        "Placeholder Subject",
                        "Placeholder body."))
                    {
                        placeholder.Save(msgFilePath);
                    }
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Error creating placeholder MSG: {ex.Message}");
                    return;
                }

                    Console.Error.WriteLine($"Skipping missing file: {msgFilePath}");
                    continue;
                }

                try
                {
                    using (MapiMessage message = MapiMessage.Load(msgFilePath))
                    {
                        string[] existingButtons;
                        try
                        {
                            existingButtons = FollowUpManager.GetVotingButtons(message);
                        }
                        catch (Exception ex)
                        {
                            Console.Error.WriteLine($"Failed to get voting buttons for {msgFilePath}: {ex.Message}");
                            continue;
                        }

                        bool hasDefaultButton = false;
                        if (existingButtons != null)
                        {
                            foreach (string btn in existingButtons)
                            {
                                if (string.Equals(btn, "Approve", StringComparison.OrdinalIgnoreCase))
                                {
                                    hasDefaultButton = true;
                                    break;
                                }
                            }
                        }

                        if (!hasDefaultButton)
                        {
                            try
                            {
                                FollowUpManager.AddVotingButton(message, "Approve");
                            }
                            catch (Exception ex)
                            {
                                Console.Error.WriteLine($"Failed to add voting button to {msgFilePath}: {ex.Message}");
                                continue;
                            }

                            try
                            {
                                message.Save(msgFilePath);
                                Console.WriteLine($"Added voting button to: {msgFilePath}");
                            }
                            catch (Exception ex)
                            {
                                Console.Error.WriteLine($"Failed to save updated message {msgFilePath}: {ex.Message}");
                            }
                        }
                        else
                        {
                            Console.WriteLine($"Voting button already present in: {msgFilePath}");
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Error processing file {msgFilePath}: {ex.Message}");
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
