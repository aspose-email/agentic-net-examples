using System;
using System.IO;
using System.Collections.Generic;
using System.Text.Json;
using Aspose.Email;
using Aspose.Email.Mapi;

class Program
{
    static void Main(string[] args)
    {
        try
        {
            // Define input JSON file and output MSG file paths
            string jsonPath = "votingButtons.json";
            string msgPath = "outputMessage.msg";

            // Ensure the JSON file exists; if not, create a minimal placeholder
            if (!File.Exists(jsonPath))
            {
                try
                {
                    File.WriteAllText(jsonPath, "[]");
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to create placeholder JSON file: {ex.Message}");
                    return;
                }
            }

            // Read and deserialize the JSON containing voting button display names
            List<string> votingButtons;
            try
            {
                string jsonContent = File.ReadAllText(jsonPath);
                votingButtons = JsonSerializer.Deserialize<List<string>>(jsonContent);
                if (votingButtons == null)
                {
                    Console.Error.WriteLine("Deserialized voting buttons list is null.");
                    return;
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Error reading or deserializing JSON: {ex.Message}");
                return;
            }

            // Create a new MapiMessage
            MapiMessage message = new MapiMessage(
                "sender@example.com",
                "recipient@example.com",
                "Sample Message with Voting Buttons",
                "This message contains voting buttons defined in JSON."
            );

            // Add each voting button to the message
            foreach (string button in votingButtons)
            {
                if (string.IsNullOrWhiteSpace(button))
                {
                    continue; // Skip empty entries
                }

                try
                {
                    FollowUpManager.AddVotingButton(message, button);
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to add voting button \"{button}\": {ex.Message}");
                }
            }

            // Ensure the output directory exists
            string outputDirectory = Path.GetDirectoryName(Path.GetFullPath(msgPath));
            if (!Directory.Exists(outputDirectory))
            {
                try
                {
                    Directory.CreateDirectory(outputDirectory);
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to create output directory: {ex.Message}");
                    return;
                }
            }

            // Save the message as MSG
            try
            {
                message.Save(msgPath);
                Console.WriteLine($"Message saved successfully to {msgPath}");
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Error saving MSG file: {ex.Message}");
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
