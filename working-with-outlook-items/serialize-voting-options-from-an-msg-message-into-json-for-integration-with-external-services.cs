using System;
using System.IO;
using System.Text.Json;
using Aspose.Email;
using Aspose.Email.Mapi;

class Program
{
    static void Main()
    {
        try
        {
            string msgFilePath = "sample.msg";
            string jsonFilePath = "votingOptions.json";

            // Ensure the MSG file exists; create a minimal placeholder if it does not.
            if (!File.Exists(msgFilePath))
            {
                try
                {
                    using (MapiMessage placeholder = new MapiMessage("placeholder@example.com", "recipient@example.com", "Placeholder", "This is a placeholder message."))
                    {
                        placeholder.Save(msgFilePath);
                    }
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to create placeholder MSG file: {ex.Message}");
                    return;
                }
            }

            // Load the MSG file.
            MapiMessage message;
            try
            {
                message = MapiMessage.Load(msgFilePath);
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Failed to load MSG file: {ex.Message}");
                return;
            }

            using (message)
            {
                // Retrieve voting buttons.
                string[] votingButtons;
                try
                {
                    votingButtons = FollowUpManager.GetVotingButtons(message);
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to get voting buttons: {ex.Message}");
                    return;
                }

                // Serialize voting options to JSON.
                string jsonContent;
                try
                {
                    JsonSerializerOptions options = new JsonSerializerOptions { WriteIndented = true };
                    jsonContent = JsonSerializer.Serialize(votingButtons, options);
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to serialize voting options: {ex.Message}");
                    return;
                }

                // Write JSON to file.
                try
                {
                    File.WriteAllText(jsonFilePath, jsonContent);
                    Console.WriteLine($"Voting options saved to '{jsonFilePath}'.");
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to write JSON file: {ex.Message}");
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
