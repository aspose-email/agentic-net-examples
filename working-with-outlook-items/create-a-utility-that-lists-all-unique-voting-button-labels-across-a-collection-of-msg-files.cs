using System;
using System.IO;
using System.Collections.Generic;
using Aspose.Email;
using Aspose.Email.Mapi;

namespace VotingButtonLister
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                // Define the directory containing MSG files
                string msgDirectory = "MsgFiles";

                // Verify the directory exists
                if (!Directory.Exists(msgDirectory))
                {
                    Console.Error.WriteLine($"Error: Directory not found – {msgDirectory}");
                    return;
                }

                // Get all .msg files in the directory
                string[] msgFiles = Directory.GetFiles(msgDirectory, "*.msg");

                // Collect unique voting button labels
                HashSet<string> uniqueButtons = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

                foreach (string filePath in msgFiles)
                {
                    // Guard against missing files (should not happen after GetFiles, but safe)
                    if (!File.Exists(filePath))
                    {
                try
                {
                    using (MapiMessage placeholder = new MapiMessage(
                        "from@example.com",
                        "to@example.com",
                        "Placeholder Subject",
                        "Placeholder body."))
                    {
                        placeholder.Save(filePath);
                    }
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Error creating placeholder MSG: {ex.Message}");
                    return;
                }

                        Console.Error.WriteLine($"Warning: File not found – {filePath}");
                        continue;
                    }

                    try
                    {
                        // Load the MSG file
                        using (MapiMessage message = MapiMessage.Load(filePath))
                        {
                            // Retrieve voting buttons for the message
                            string[] buttons = FollowUpManager.GetVotingButtons(message);

                            if (buttons != null)
                            {
                                foreach (string button in buttons)
                                {
                                    if (!string.IsNullOrEmpty(button))
                                    {
                                        uniqueButtons.Add(button);
                                    }
                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.Error.WriteLine($"Error processing '{filePath}': {ex.Message}");
                    }
                }

                // Output the collected unique voting button labels
                Console.WriteLine("Unique voting button labels:");
                foreach (string button in uniqueButtons)
                {
                    Console.WriteLine(button);
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Unexpected error: {ex.Message}");
            }
        }
    }
}
