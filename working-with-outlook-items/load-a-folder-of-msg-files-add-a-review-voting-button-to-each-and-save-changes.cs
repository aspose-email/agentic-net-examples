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
            // Specify the folder containing MSG files
            string folderPath = @"C:\MsgFolder";

            // Verify that the folder exists
            if (!Directory.Exists(folderPath))
            {
                Console.Error.WriteLine("The specified folder does not exist: " + folderPath);
                return;
            }

            // Get all MSG files in the folder
            string[] msgFiles = Directory.GetFiles(folderPath, "*.msg");

            foreach (string filePath in msgFiles)
            {
                // Guard each file operation with its own try/catch
                try
                {
                    // Ensure the file still exists before processing
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

                        Console.Error.WriteLine("File not found, skipping: " + filePath);
                        continue;
                    }

                    // Load the MSG file into a MapiMessage instance
                    using (MapiMessage message = MapiMessage.Load(filePath))
                    {
                        // Add the "Review" voting button
                        FollowUpManager.AddVotingButton(message, "Review");

                        // Save the modified message back to the same file
                        message.Save(filePath);
                    }

                    Console.WriteLine("Processed file: " + filePath);
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine("Error processing file '" + filePath + "': " + ex.Message);
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine("Unexpected error: " + ex.Message);
        }
    }
}
