using System;
using System.IO;
using Aspose.Email;
using Aspose.Email.Mapi;

public class Program
{
    public static void Main(string[] args)
    {
        try
        {
            string inputFolderPath = "Messages";
            string outputFolderPath = "ModifiedMessages";

            if (!Directory.Exists(inputFolderPath))
            {
                Console.Error.WriteLine($"Input folder '{inputFolderPath}' does not exist.");
                return;
            }

            if (!Directory.Exists(outputFolderPath))
            {
                try
                {
                    Directory.CreateDirectory(outputFolderPath);
                }
                catch (Exception dirEx)
                {
                    Console.Error.WriteLine($"Failed to create output folder '{outputFolderPath}': {dirEx.Message}");
                    return;
                }
            }

            string[] msgFilePaths;
            try
            {
                msgFilePaths = Directory.GetFiles(inputFolderPath, "*.msg");
            }
            catch (Exception getFilesEx)
            {
                Console.Error.WriteLine($"Failed to enumerate MSG files in '{inputFolderPath}': {getFilesEx.Message}");
                return;
            }

            foreach (string msgFilePath in msgFilePaths)
            {
                try
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

                        Console.Error.WriteLine($"File '{msgFilePath}' does not exist.");
                        continue;
                    }

                    using (MapiMessage message = MapiMessage.Load(msgFilePath))
                    {
                        // Add a voting button to the message
                        FollowUpManager.AddVotingButton(message, "Approve");

                        string outputFilePath = Path.Combine(outputFolderPath, Path.GetFileName(msgFilePath));

                        try
                        {
                            message.Save(outputFilePath);
                        }
                        catch (Exception saveEx)
                        {
                            Console.Error.WriteLine($"Failed to save modified message to '{outputFilePath}': {saveEx.Message}");
                        }
                    }
                }
                catch (Exception fileEx)
                {
                    Console.Error.WriteLine($"Error processing file '{msgFilePath}': {fileEx.Message}");
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unhandled exception: {ex.Message}");
        }
    }
}
