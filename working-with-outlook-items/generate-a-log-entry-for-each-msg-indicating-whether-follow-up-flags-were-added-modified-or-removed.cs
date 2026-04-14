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
            // Directory containing MSG files (adjust as needed)
            string messagesFolder = "Messages";

            // Ensure the directory exists; if not, create it to avoid unguarded IO errors
            if (!Directory.Exists(messagesFolder))
            {
                try
                {
                    Directory.CreateDirectory(messagesFolder);
                }
                catch (Exception dirEx)
                {
                    Console.Error.WriteLine($"Failed to create directory '{messagesFolder}': {dirEx.Message}");
                    return;
                }
            }

            // Get all .msg files in the folder
            string[] msgFiles;
            try
            {
                msgFiles = Directory.GetFiles(messagesFolder, "*.msg");
            }
            catch (Exception getFilesEx)
            {
                Console.Error.WriteLine($"Error retrieving MSG files: {getFilesEx.Message}");
                return;
            }

            foreach (string msgPath in msgFiles)
            {
                // Guard against missing files
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

                    Console.Error.WriteLine($"File not found: {msgPath}");
                    continue;
                }

                // Process each MSG file
                try
                {
                    using (MapiMessage message = MapiMessage.Load(msgPath))
                    {
                        // Add a follow‑up flag
                        FollowUpManager.SetFlag(message, "Follow up");
                        Console.WriteLine($"Added follow‑up flag to '{Path.GetFileName(msgPath)}'.");

                        // Modify the flag by adding start and due dates
                        DateTime startDate = DateTime.Now;
                        DateTime dueDate = startDate.AddDays(2);
                        FollowUpManager.SetFlag(message, "Follow up", startDate, dueDate);
                        Console.WriteLine($"Modified follow‑up flag for '{Path.GetFileName(msgPath)}' (Start: {startDate}, Due: {dueDate}).");

                        // Remove the follow‑up flag
                        FollowUpManager.ClearFlag(message);
                        Console.WriteLine($"Removed follow‑up flag from '{Path.GetFileName(msgPath)}'.");

                        // Save changes back to the original file
                        message.Save(msgPath);
                    }
                }
                catch (Exception msgEx)
                {
                    Console.Error.WriteLine($"Error processing '{msgPath}': {msgEx.Message}");
                    // Continue with next file without terminating the whole program
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
