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
            string inputFolder = "InputMsgs";
            string processedFolder = "Processed";

            // Ensure input folder exists
            if (!Directory.Exists(inputFolder))
            {
                Console.Error.WriteLine($"Error: Input folder not found – {inputFolder}");
                return;
            }

            // Ensure processed folder exists or create it
            if (!Directory.Exists(processedFolder))
            {
                try
                {
                    Directory.CreateDirectory(processedFolder);
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Error: Could not create processed folder – {ex.Message}");
                    return;
                }
            }

            string[] msgFiles;
            try
            {
                msgFiles = Directory.GetFiles(inputFolder, "*.msg");
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Error: Unable to enumerate MSG files – {ex.Message}");
                return;
            }

            foreach (string msgPath in msgFiles)
            {
                if (!File.Exists(msgPath))
                {
                try
                {
                    using (MapiMessage placeholder = new MapiMessage(
                        "from@example.com",
                        "to@example.com",
                        "Placeholder Subject",
                        "Placeholder body."))
                    {
                        placeholder.Save(msgPath);
                    }
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Error creating placeholder MSG: {ex.Message}");
                    return;
                }

                    Console.Error.WriteLine($"Warning: File not found – {msgPath}");
                    continue;
                }

                try
                {
                    using (MapiMessage message = MapiMessage.Load(msgPath))
                    {
                        // Add voting buttons
                        FollowUpManager.AddVotingButton(message, "Approve");
                        FollowUpManager.AddVotingButton(message, "Reject");

                        // Save changes back to the same file
                        message.Save(msgPath);
                    }

                    // Move the processed file to the "Processed" folder
                    string destinationPath = Path.Combine(processedFolder, Path.GetFileName(msgPath));
                    try
                    {
                        // Overwrite if a file with the same name already exists
                        if (File.Exists(destinationPath))
                        {
                            File.Delete(destinationPath);
                        }
                        File.Move(msgPath, destinationPath);
                    }
                    catch (Exception moveEx)
                    {
                        Console.Error.WriteLine($"Error: Could not move file '{msgPath}' – {moveEx.Message}");
                    }
                }
                catch (Exception fileEx)
                {
                    Console.Error.WriteLine($"Error processing file '{msgPath}' – {fileEx.Message}");
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unhandled exception: {ex.Message}");
        }
    }
}
