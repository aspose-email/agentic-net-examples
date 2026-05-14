using System;
using System.IO;
using Aspose.Email;
using Aspose.Email.Mapi;

class Program
{
    static void Main(string[] args)
    {
        try
        {
            // Folder containing the MSG files
            string inputFolder = "Messages";

            // Verify the input folder exists
            if (!Directory.Exists(inputFolder))
            {
                Console.Error.WriteLine($"Input folder does not exist: {inputFolder}");
                return;
            }

            // Get all MSG files in the folder
            string[] msgFiles = Directory.GetFiles(inputFolder, "*.msg");

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

                    Console.Error.WriteLine($"File not found: {msgFilePath}");
                    continue;
                }

                try
                {
                    // Load the MSG file
                    using (MapiMessage msg = MapiMessage.Load(msgFilePath))
                    {
                        // Retrieve the PidTagPriority property (custom priority)
                        int priorityValue = 0;
                        bool hasPriority = msg.TryGetPropertyInt32(KnownPropertyList.Priority.Tag, ref priorityValue);

                        if (hasPriority)
                        {
                            // Map custom priority to standard importance (0=Low, 1=Normal, 2=High)
                            int importance;
                            if (priorityValue <= 0)
                                importance = 0;          // Low
                            else if (priorityValue >= 2)
                                importance = 2;          // High
                            else
                                importance = 1;          // Normal

                            // Set the PidTagImportance property accordingly
                            msg.SetProperty(new MapiProperty(KnownPropertyList.Importance, importance));

                            // Save the updated MSG back to the same file
                            msg.Save(msgFilePath);

                            Console.WriteLine($"Updated importance for: {Path.GetFileName(msgFilePath)}");
                        }
                        else
                        {
                            Console.WriteLine($"Priority property not found in: {Path.GetFileName(msgFilePath)}");
                        }
                    }
                }
                catch (Exception ex)
                {
                    // Handle errors for individual files without stopping the whole process
                    Console.Error.WriteLine($"Error processing '{msgFilePath}': {ex.Message}");
                }
            }
        }
        catch (Exception ex)
        {
            // Top‑level exception guard
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
