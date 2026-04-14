using System;
using System.IO;
using System.Text.Json;
using System.Collections.Generic;
using Aspose.Email;
using Aspose.Email.Mapi;

class Program
{
    static void Main()
    {
        try
        {
            // Paths for input JSON and output MSG
            string jsonPath = "settings.json";
            string outputMsgPath = "output.msg";

            // Ensure JSON input exists; create minimal placeholder if missing
            if (!File.Exists(jsonPath))
            {
                try
                {
                    var placeholder = new
                    {
                        VotingButtons = new List<string>(),
                        Categories = new List<string>()
                    };
                    string placeholderJson = JsonSerializer.Serialize(placeholder, new JsonSerializerOptions { WriteIndented = true });
                    File.WriteAllText(jsonPath, placeholderJson);
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to create placeholder JSON: {ex.Message}");
                    return;
                }
            }

            // Read and deserialize JSON settings
            Settings settings;
            try
            {
                string jsonContent = File.ReadAllText(jsonPath);
                settings = JsonSerializer.Deserialize<Settings>(jsonContent);
                if (settings == null)
                {
                    Console.Error.WriteLine("Deserialized settings are null.");
                    return;
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Error reading or deserializing JSON: {ex.Message}");
                return;
            }

            // Ensure output directory exists
            try
            {
                string outputDir = Path.GetDirectoryName(Path.GetFullPath(outputMsgPath));
                if (!string.IsNullOrEmpty(outputDir) && !Directory.Exists(outputDir))
                {
                    Directory.CreateDirectory(outputDir);
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Failed to ensure output directory: {ex.Message}");
                return;
            }

            // Create a new MAPI message
            using (MapiMessage message = new MapiMessage())
            {
                message.Subject = "Sample Message with Follow‑Up Settings";
                message.Body = "This message was generated programmatically and has voting buttons and categories applied from JSON.";

                // Apply voting buttons if any
                if (settings.VotingButtons != null)
                {
                    foreach (string button in settings.VotingButtons)
                    {
                        try
                        {
                            FollowUpManager.AddVotingButton(message, button);
                        }
                        catch (Exception ex)
                        {
                            Console.Error.WriteLine($"Failed to add voting button '{button}': {ex.Message}");
                        }
                    }
                }

                // Apply categories if any
                if (settings.Categories != null)
                {
                    foreach (string category in settings.Categories)
                    {
                        try
                        {
                            FollowUpManager.AddCategory(message, category);
                        }
                        catch (Exception ex)
                        {
                            Console.Error.WriteLine($"Failed to add category '{category}': {ex.Message}");
                        }
                    }
                }

                // Save the message to MSG file
                try
                {
                    message.Save(outputMsgPath);
                    Console.WriteLine($"Message saved to '{outputMsgPath}'.");
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to save MSG file: {ex.Message}");
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }

    // Helper class matching the JSON structure
    private class Settings
    {
        public List<string> VotingButtons { get; set; }
        public List<string> Categories { get; set; }
    }
}
