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
            const string msgPath = "readonly_message.msg";

            // Ensure the MSG file exists; create a minimal placeholder if missing.
            if (!File.Exists(msgPath))
            {
                try
                {
                    MapiMessage placeholder = new MapiMessage(
                        "sender@example.com",
                        "recipient@example.com",
                        "Placeholder Subject",
                        "Placeholder body");
                    placeholder.Save(msgPath);
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Error creating placeholder MSG file: {ex.Message}");
                    return;
                }
            }

            // Mark the file as read‑only to simulate a read‑only MSG.
            try
            {
                File.SetAttributes(msgPath, FileAttributes.ReadOnly);
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Error setting file attribute: {ex.Message}");
                return;
            }

            // Load the MSG file and attempt to set a follow‑up flag.
            try
            {
                using (MapiMessage message = MapiMessage.Load(msgPath))
                {
                    try
                    {
                        FollowUpManager.SetFlag(message, "Follow up");
                        // Attempt to save the modified message back to the same read‑only file.
                        message.Save(msgPath);
                        Console.WriteLine("Follow‑up flag set and saved successfully.");
                    }
                    catch (Exception ex)
                    {
                        Console.Error.WriteLine($"Failed to set or save follow‑up flag: {ex.Message}");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Error loading MSG file: {ex.Message}");
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
